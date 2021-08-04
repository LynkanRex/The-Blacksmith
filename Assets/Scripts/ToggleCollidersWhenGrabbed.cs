using System.Collections.Generic;
using UnityEngine;

public class ToggleCollidersWhenGrabbed : MonoBehaviour
{
    [SerializeField] private bool isHeld;
    private List<Collider> colliders;
    
    private void Awake()
    {
        isHeld = false;
        colliders = new List<Collider>();
            
        var tmpColliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (var collider in tmpColliders)
        {
            if (!collider.isTrigger)
            {
                colliders.Add(collider);    
            }
            
        }
    }

    public void ToggleHeld()
    {
        isHeld = !isHeld;

        foreach (var collider in colliders)
        {
            collider.enabled = !isHeld;
        }
    }
}
