using System;
using UnityEngine;

public class Bellows : MonoBehaviour
{
    private IMessageHandler MessageHandler;

    private void Awake()
    {
        MessageHandler = FindObjectOfType<MessageHandler>();
    }

    public void HeatForge()
    {
        Debug.Log("Bellows activated");
        MessageHandler.SendMessage(new BellowsEvent());   
    }
}
