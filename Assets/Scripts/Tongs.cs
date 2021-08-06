using UnityEngine;

public class Tongs : MonoBehaviour
{

    private GameObject heldObject;
    private bool isHoldingItem;

    private void Start()
    {
        isHoldingItem = false;
    }


    public void HoldItem()
    {
        if (isHoldingItem)
            return;
        
        
    }


    public void DropItem()
    {
        if (!isHoldingItem)
            return;

        
    }
}
