using System;
using System.Collections.Generic;
using UnityEngine;

public class HeatingPad : MonoBehaviour
{
    [SerializeField] private bool isOn;
    [SerializeField] private List<GameObject> objects;


    [SerializeField] private float timer = 5.0f;
    [SerializeField] private float timerSpeed = 1.0f;

    private float currentTimer = 0.0f;
    
    private void Awake()
    {
        isOn = false;
        MessageHandler.Instance().SubscribeMessage<BellowsEvent>(TurnOn);
    }

    private void Update()
    {
        if (currentTimer > 0.00f)
            currentTimer -= Time.deltaTime * timerSpeed;
        else if(isOn)
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

    private void TurnOn(BellowsEvent eventRef)
    {
        isOn = true;
        currentTimer = timer;
    }

    private void OnDestroy()
    {
        MessageHandler.Instance().UnsubscribeMessage<BellowsEvent>(TurnOn);
    }
}
