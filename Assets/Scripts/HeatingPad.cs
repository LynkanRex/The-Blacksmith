using System.Collections.Generic;
using UnityEngine;

public class HeatingPad : MonoBehaviour
{
    [SerializeField] private bool isOn;
    [SerializeField] private List<GameObject> objects;

    private void Awake()
    {
        isOn = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ingot>() != null)
        {
            foreach (var t in objects)
            {
                if (t.gameObject == other.gameObject)
                    return;
            }

            objects.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isOn)
            return;

        foreach (var ingot in objects)
        {
            ingot.GetComponent<Ingot>().isBeingHeated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ingot>() != null)
        {
            if (isOn)
                other.gameObject.GetComponent<Ingot>().isBeingHeated = false;
            objects.Remove(other.gameObject);
        }
    }
}
