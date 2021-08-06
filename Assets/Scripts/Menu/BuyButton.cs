using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public bool simulateBuyButtonPress;
    public bool simulateHammerButtonPress;
    public bool simulateRespawnIngotButtonPress;
    
    public void BuyIngotButton()
    {
        FindObjectOfType<GameStateManager>().BuyIngot();
    }


    public void RewpawnHammer()
    {
        FindObjectOfType<GameStateManager>().RespawnHammer();
    }

    public void RespawnAllIngots()
    {
        FindObjectOfType<GameStateManager>().RespawnAllIngots();
    }

    private void Update()
    {
        if (simulateBuyButtonPress)
        {
            BuyIngotButton();
            simulateBuyButtonPress = false;
        }

        if (simulateHammerButtonPress)
        {
            RewpawnHammer();
            simulateHammerButtonPress = false;
        }

        if (simulateRespawnIngotButtonPress)
        {
            RespawnAllIngots();
            simulateRespawnIngotButtonPress = false;
        }
    }
}
