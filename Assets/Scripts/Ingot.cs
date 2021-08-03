using UnityEngine;

public class Ingot : MonoBehaviour
{
    public float Heat
    {
        get => heat;
        set => heat = Mathf.Clamp(value, 0.0f, 100.0f);
    }

    private float heat;
    [SerializeField] private float heatingSpeed;
    [SerializeField] private float heatingThreshold;
    [SerializeField] private Mesh[] progressStages;
    [SerializeField] private int strikesPerProgressStage;
    [SerializeField] private Material heatedMaterial;

    private ParticleSystem particleSystem;
    public bool isBeingHeated;
    public bool malleable;
    public bool isOnAnvil;
    private int currentStrikes;
    private int currentProgressStage;
    private Material startMat;

    public int index;
    
    private void Awake()
    {
        startMat = GetComponent<MeshRenderer>().material;
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (isBeingHeated)
            Heat += Time.deltaTime * heatingSpeed;
        else
            Heat -= Time.deltaTime * heatingSpeed;
        
        if (Heat >= heatingThreshold)
            StartIsMalleable();
        else
            EndIsMalleable();
    }

    public void StartIsMalleable()
    {
        GetComponent<MeshRenderer>().material = heatedMaterial;
        malleable = true;
    }

    public void EndIsMalleable()
    {
        GetComponent<MeshRenderer>().material = startMat;
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
                //TODO: Play particle effect and sound of dinging metal + sparks
                ImpartStrike(); 
                hammer.GetComponentInParent<Mallet>().TriggerCoolDown();
            }
            else
            {
                //TODO: play a weak "tink" sound in stead
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
        //TODO: also play audio
        
        if (currentStrikes >= strikesPerProgressStage)
        {
            currentProgressStage++;
            
            if (currentProgressStage == progressStages.Length)
            {
                Debug.Log("Swapping");
                FindObjectOfType<GameStateManager>().SwapIngotForDagger(this.index);
                return;
            }
            
            GetComponent<MeshFilter>().mesh = progressStages[currentProgressStage];
            currentStrikes -= strikesPerProgressStage;
        }
    }
}