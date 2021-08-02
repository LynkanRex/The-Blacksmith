using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public void BuyIngotButton()
    {
        FindObjectOfType<GameStateManager>().BuyIngot();
    }
}
