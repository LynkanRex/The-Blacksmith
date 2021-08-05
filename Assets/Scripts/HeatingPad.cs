using System.Collections.Generic;
using UnityEngine;

public class HeatingPad : MonoBehaviour
{
    [SerializeField] private bool isOn;
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private float timer = 5.0f;
    [SerializeField] private float timerSpeed = 1.0f;
    
    [SerializeField] private ParticleSystem forgeParticleSystem;
    [SerializeField] private Light forgePointLight;
    
    private AudioSource audioSource;
    
    
    private float currentTimer = 0.0f;
    
    private IMessageHandler MessageHandler;

    private void Awake()
    {
        isOn = false;
        
        MessageHandler = FindObjectOfType<MessageHandler>();
        MessageHandler.SubscribeMessage<BellowsEvent>(TurnOn);

        audioSource = GetComponent<AudioSource>();
        forgePointLight.gameObject.SetActive(false);
        forgeParticleSystem.Stop();
    }

    private void Update()
    {
        if (currentTimer > 0.0f)
            currentTimer -= Time.deltaTime * timerSpeed;
        else if (isOn)
        {
            isOn = false;
            forgeParticleSystem.Stop();
            forgePointLight.gameObject.SetActive(false);
            audioSource.Stop();
        }
            
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

            if (objects.Count == 0)
            {
                var ingotsInPlay= FindObjectsOfType<Ingot>();
                foreach (var ingot in ingotsInPlay)
                {
                    if (ingot.isBeingHeated)
                        ingot.isBeingHeated = false;
                }
            }
        }
    }

    private void TurnOn(BellowsEvent eventRef)
    {
        isOn = true;
        currentTimer = timer;
        forgePointLight.gameObject.SetActive(true);
        forgeParticleSystem.Play();
        audioSource.Play();
    }

    private void OnDestroy()
    {
        MessageHandler.UnsubscribeMessage<BellowsEvent>(TurnOn);
    }
}
