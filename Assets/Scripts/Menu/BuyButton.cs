using System;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public bool simulateButtonPress;
    
    public void BuyIngotButton()
    {
        FindObjectOfType<GameStateManager>().BuyIngot();
    }


    private void Update()
    {
        if (simulateButtonPress)
        {
            BuyIngotButton();
            simulateButtonPress = false;
        }
    }
}
