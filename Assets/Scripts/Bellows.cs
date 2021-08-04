using System;
using UnityEngine;

public class Bellows : MonoBehaviour
{
    public bool simulateAction;
    
    private IMessageHandler MessageHandler;

    [SerializeField] private AudioClip bellowsSound;
    [SerializeField] private AudioSource audioSource;
    
    
    private void Awake()
    {
        MessageHandler = FindObjectOfType<MessageHandler>();

        audioSource = GetComponent<AudioSource>();
        bellowsSound = audioSource.clip;
    }

    private void Update()
    {
        if (simulateAction)
        {
            HeatForge();
            simulateAction = false;
        }
    }

    public void HeatForge()
    {
        if(audioSource != null)
            audioSource.PlayOneShot(bellowsSound);
        MessageHandler.SendMessage(new BellowsEvent());   
    }
}
