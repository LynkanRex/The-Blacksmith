using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private int currentMoney = 100;
    [SerializeField] private TextMeshProUGUI moneyText;
    
    [SerializeField] private GameObject ironIngotObject;
    [SerializeField] private GameObject ironDaggerObject;

    private GameObject currentlySpawnedIronIngotObject;
    private GameObject currentlySpawnedIronDaggerObject;
    
    [SerializeField] private int daggerSellPrice = 15;
    [SerializeField] private int ingotBuyPrice = 15;

    [SerializeField] private Transform poolSpawnPoint;
    [SerializeField] private Transform ingotSpawnPoint;
    
    void Start()
    {
        UpdateUI();
        
        SpawnIngotAndTranslate();
        SpawnDagger();
    }

    public void SwapIngotForDagger()
    {
        var desiredTransform = currentlySpawnedIronIngotObject.transform.position;
        currentlySpawnedIronIngotObject.transform.position = currentlySpawnedIronDaggerObject.transform.position;
        currentlySpawnedIronDaggerObject.transform.position = desiredTransform;
        Destroy(currentlySpawnedIronIngotObject);
    }

    public void SpawnDagger()
    {
        currentlySpawnedIronDaggerObject = Instantiate(ironDaggerObject, poolSpawnPoint.transform);
    }

    public void SpawnIngotAndTranslate()
    {
        currentlySpawnedIronIngotObject = Instantiate(ironIngotObject);
        currentlySpawnedIronIngotObject.transform.position = ingotSpawnPoint.transform.position;
    }

    public void SellDagger()
    {
        UpdateMoneyAmountAndDisplay(daggerSellPrice);
        Destroy(currentlySpawnedIronDaggerObject);
    }

    public void BuyIngot()
    {
        if (!CheckAffordability(ingotBuyPrice))
            return;
        
        UpdateMoneyAmountAndDisplay(-ingotBuyPrice);
        SpawnIngotAndTranslate();
        SpawnDagger();
    }

    private bool CheckAffordability(int value)
    {
        if (value > currentMoney)
            return false;
        return true;
    }

    public void UpdateMoneyAmountAndDisplay(int value)
    {
        currentMoney += value;
        UpdateUI();
    }

    private void UpdateUI()
    {
        moneyText.text = "Gold: " + currentMoney;
    }
}
