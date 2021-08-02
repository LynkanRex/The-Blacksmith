using UnityEngine;

public class Bellows : MonoBehaviour
{
    public void HeatForge()
    {
        MessageHandler.Instance().SendMessage(new BellowsEvent());   
    }
}
