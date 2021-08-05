
using UnityEngine;


public class Mallet : MonoBehaviour
{
    public bool isHeld;
    [SerializeField] private BoxCollider[] colliders;

    [SerializeField] private float strikeCoolDown = 1.0f;

    [SerializeField] private AudioClip audioClip;
    
    private float cooldownTimer;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (isHeld)
        {
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;    
            }
        }
            
        else
            foreach (var collider in colliders)
            {
                collider.isTrigger = false;    
            }

        if (cooldownTimer >= 0.0f)
            cooldownTimer -= Time.deltaTime;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (cooldownTimer >= 0.0f)
            return;

        if (other.gameObject.GetComponent<Ingot>() != null)
        {
            cooldownTimer = strikeCoolDown;
        }
            
        
        if(!isHeld && !audioSource.isPlaying)
            audioSource.PlayOneShot(audioClip);
    }

    public void TriggerIsHeld()
    {
        isHeld = !isHeld;
    }

    public float RequestCoolDownData()
    {
        return cooldownTimer;
    }

    public void TriggerCoolDown()
    {
        cooldownTimer = strikeCoolDown;
    }
}
