using UnityEngine;

public class Mallet : MonoBehaviour
{
    public bool isHeld;
    private BoxCollider collider;

    [SerializeField] private float strikeCoolDown = 1.0f;
    
    private float cooldownTimer;

    private void Awake()
    {
        this.collider = GetComponent<BoxCollider>();
    }
    
    void Update()
    {
        if (isHeld)
            this.collider.isTrigger = true;
        else
            this.collider.isTrigger = false;

        if (cooldownTimer >= 0.0f)
            cooldownTimer -= Time.deltaTime;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (cooldownTimer >= 0.0f)
            return;
        
        if (other.gameObject.GetComponent<Ingot>() != null)
            cooldownTimer = strikeCoolDown;
    }
}
