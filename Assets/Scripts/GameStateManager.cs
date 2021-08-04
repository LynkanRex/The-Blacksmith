using System.Collections.Generic;
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
    
    private Dictionary<int, GameObject> spawnedDaggers;
    private Dictionary<int, GameObject> spawnedIngots;

    void Start()
    {
        spawnedDaggers = new Dictionary<int, GameObject>();
        spawnedIngots = new Dictionary<int, GameObject>();
        UpdateUI();
    }

    public void SwapIngotForDagger(int index)
    {
        var desiredTransform = Vector3.zero;
        
        foreach (var entry in spawnedIngots)
        {
            if (entry.Key == index)
            {
                desiredTransform = entry.Value.transform.position;
                entry.Value.transform.position = poolSpawnPoint.transform.position;
            }
        }

        foreach (var entry in spawnedDaggers)
        {
            if (entry.Key == index)
            {
                entry.Value.transform.position = desiredTransform;
                entry.Value.GetComponentInChildren<ParticleSystem>().Play();
                if(entry.Value.GetComponent<AudioSource>().clip != null)
                    entry.Value.GetComponent<AudioSource>().PlayOneShot(entry.Value.GetComponent<AudioSource>().clip);
            }   
        }
    }

    public void SpawnIngotAndDagger()
    {
        currentlySpawnedIronIngotObject = Instantiate(ironIngotObject);
        currentlySpawnedIronDaggerObject = Instantiate(ironDaggerObject);

        currentlySpawnedIronIngotObject.GetComponent<Ingot>().index = spawnedIngots.Count+1;
        currentlySpawnedIronDaggerObject.GetComponent<Dagger>().index = spawnedDaggers.Count+1;

        currentlySpawnedIronDaggerObject.transform.position = poolSpawnPoint.transform.position;
        currentlySpawnedIronIngotObject.transform.position = poolSpawnPoint.transform.position;

        spawnedIngots.Add(spawnedIngots.Count+1, currentlySpawnedIronIngotObject);
        spawnedDaggers.Add(spawnedDaggers.Count+1, currentlySpawnedIronDaggerObject);
        
        TranslateIngot();
        ClearInstanceCache();
    }

    public void TranslateIngot()
    {
        currentlySpawnedIronIngotObject.transform.position = ingotSpawnPoint.transform.position;
    }

    public void ClearInstanceCache()
    {
        currentlySpawnedIronDaggerObject = null;
        currentlySpawnedIronIngotObject = null;
    }

    public void SellDagger(int index)
    {
        foreach (var entry in spawnedIngots)
        {
            if (entry.Key == index)
            {
                var ingotObject = entry.Value;
                spawnedIngots.Remove(entry.Key);
                Destroy(ingotObject);
                break;    
            }
        }
        
        foreach (var entry in spawnedDaggers)
        {
            if (entry.Key == index)
            {
                var daggerObject = entry.Value;
                spawnedDaggers.Remove(entry.Key);
                Destroy(daggerObject);
                break;
            }
        }
        
        UpdateMoneyAmountAndDisplay(daggerSellPrice);
    }

    public void BuyIngot()
    {
        if (!CheckAffordability(ingotBuyPrice))
            return;
        
        UpdateMoneyAmountAndDisplay(-ingotBuyPrice);
        SpawnIngotAndDagger();
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

    public void RespawnHammer()
    {
        var hammerRef = FindObjectOfType<Mallet>().gameObject;
        hammerRef.transform.position = ingotSpawnPoint.transform.position;
    }

    public void RespawnAllIngots()
    {
        var ingots = FindObjectsOfType<Ingot>();

        foreach (var ingot in ingots)
        {
            if (ingot.name == "Ingot Orig Instance")
                return;
            ingot.transform.position = ingotSpawnPoint.transform.position;
        }
    }
}