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
    
    public bool isBeingHeated;
    public bool malleable;
    public bool isOnAnvil;

    private int currentStrikes;
    private int currentProgressStage;
    private Material startMat;
    


    private void Awake()
    {
        startMat = GetComponent<MeshRenderer>().material;
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
        
        if (other.gameObject.GetComponent<Mallet>() != null)
            ImpartStrike();
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
        if (currentProgressStage == progressStages.Length && currentStrikes >= strikesPerProgressStage)
            return;
        
        currentStrikes++;
        
        if (currentStrikes >= strikesPerProgressStage)
        {
            currentStrikes -= strikesPerProgressStage;
            GetComponent<MeshFilter>().mesh = progressStages[currentProgressStage];
        }
    }
}
