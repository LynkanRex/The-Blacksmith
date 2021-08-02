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
        MessageHandler.SendMessage(new BellowsEvent());   
    }
}
