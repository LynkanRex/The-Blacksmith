using UnityEngine;

public class Ingot : MonoBehaviour
{
    public float Heat
    {
        get => heat;
        set => heat = Mathf.Clamp(value, 0.0f, 100.0f);
    }

    private float heat;
    private int currentHeatTier;
    [SerializeField] private float heatingSpeed;
    [SerializeField] private float[] heatingThresholds;
    [SerializeField] private Mesh[] progressStages;
    [SerializeField] private int strikesPerProgressStage;
    [SerializeField] private Material[] heatedMaterials;

    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip dropSound;
    
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    public bool isBeingHeated;
    public bool malleable;
    public bool isOnAnvil;
    private int currentStrikes;
    private int currentProgressStage;
    private MeshRenderer meshRenderer;
    private Material startMat;

    public int index;
    
    private void Awake()
    {
        startMat = GetComponent<MeshRenderer>().material;
        meshRenderer = GetComponent<MeshRenderer>(); 
        particleSystem = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (isBeingHeated)
            Heat += Time.deltaTime * heatingSpeed;
        else
            Heat -= Time.deltaTime * heatingSpeed;
        
        if (Heat >= heatingThresholds[0])
            StartIsMalleable();
        else
            EndIsMalleable();

        switch (Heat >= heatingThresholds[0])
        {
            case true:
            {
                var currentTier = 0;
                foreach (var tierValue in heatingThresholds)
                {
                    if (heat >= tierValue)
                    {
                        meshRenderer.material = heatedMaterials[currentTier];
                        currentTier++;
                    }
                }
                break;
            }
            case false:
                meshRenderer.material = startMat;
                break;
        }
    }

    public void StartIsMalleable()
    {
        malleable = true;
    }

    public void EndIsMalleable()
    {
        malleable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Anvil>() != null)
            isOnAnvil = true;

        if (other.gameObject.GetComponent<HammerHead>() != null)
        {
            var hammer = other.gameObject;
            
            var timeLeft = hammer.GetComponentInParent<Mallet>().RequestCoolDownData();
            if (timeLeft <= 0.01f)
            {
                ImpartStrike(); 
                hammer.GetComponentInParent<Mallet>().TriggerCoolDown();
                audioSource.PlayOneShot(hitSounds[0]);
            }
            else
            {
                audioSource.PlayOneShot(hitSounds[1]);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Anvil>() != null)
            isOnAnvil = false;
    }
    
    public void ImpartStrike()
    {
        if (!malleable)
            return;
        if (!isOnAnvil)
            return;
        if (currentProgressStage == progressStages.Length && currentStrikes >= strikesPerProgressStage)
            return;
        
        currentStrikes++;
        particleSystem.Play();
        if(audioSource.clip != null)
            audioSource.PlayOneShot(audioSource.clip);

        if (currentStrikes >= strikesPerProgressStage)
        {
            currentProgressStage++;
            
            if (currentProgressStage == progressStages.Length)
            {
                FindObjectOfType<GameStateManager>().SwapIngotForDagger(this.index);
                return;
            }
            
            GetComponent<MeshFilter>().mesh = progressStages[currentProgressStage];
            currentStrikes -= strikesPerProgressStage;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!audioSource.enabled)
            return;
        audioSource.PlayOneShot(dropSound);
    }
}