using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private List<AudioSource> audioSourcesInScene;
    
    private void ReactivateAudioSources()
    {
        
    }
    
    private void Awake()
    {
        spawnedDaggers = new Dictionary<int, GameObject>();
        spawnedIngots = new Dictionary<int, GameObject>();
        UpdateUI();
        
        audioSourcesInScene = new List<AudioSource>();
    }
    
    void Start()
    {
        var sources = FindObjectsOfType<AudioSource>();
        foreach (var source in sources)
        {
            audioSourcesInScene.Add(source);
        }

        ToggleAudioSourcesInScene(false);

        StartCoroutine(StartAudioSourcesAfter1Second());
    }

    private void ToggleAudioSourcesInScene(bool value)
    {
        foreach (var source in audioSourcesInScene)
        {
            source.enabled = value;
        }
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
                entry.Value.GetComponent<AudioSource>().enabled = true;
                entry.Value.GetComponent<Dagger>().PlayFinishedSound();
            }   
        }
    }

    public void SpawnIngotAndDagger()
    {
        var keyOfLastIndex = spawnedIngots.LastOrDefault().Key;
        
        currentlySpawnedIronIngotObject = Instantiate(ironIngotObject);
        currentlySpawnedIronDaggerObject = Instantiate(ironDaggerObject);

        currentlySpawnedIronIngotObject.GetComponent<Ingot>().index = keyOfLastIndex+1;
        currentlySpawnedIronDaggerObject.GetComponent<Dagger>().index = keyOfLastIndex+1;

        currentlySpawnedIronDaggerObject.transform.position = poolSpawnPoint.transform.position;
        currentlySpawnedIronIngotObject.transform.position = poolSpawnPoint.transform.position;

        bool pairExists = spawnedIngots.TryGetValue(currentlySpawnedIronIngotObject.GetComponent<Ingot>().index, out GameObject value);
        if(pairExists)
        {
            currentlySpawnedIronIngotObject.GetComponent<Ingot>().index = keyOfLastIndex+2;
            currentlySpawnedIronDaggerObject.GetComponent<Dagger>().index = keyOfLastIndex+2;
            
            spawnedIngots.Add(keyOfLastIndex+2, currentlySpawnedIronIngotObject);
            spawnedDaggers.Add(keyOfLastIndex+2, currentlySpawnedIronDaggerObject);
        }
        else
        {
            spawnedIngots.Add(keyOfLastIndex+1, currentlySpawnedIronIngotObject);
            spawnedDaggers.Add(keyOfLastIndex+1, currentlySpawnedIronDaggerObject);    
        }

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
        
        // TODO: Recreate dictionaries with new entries and give them new Indexes
        
        
        UpdateMoneyAmountAndDisplay(daggerSellPrice);
    }

    public void BuyIngot()
    {
        if (!CheckAffordability(ingotBuyPrice))
            return;
        
        UpdateMoneyAmountAndDisplay(-ingotBuyPrice);
        SpawnIngotAndDagger();
        FindObjectOfType<SellArea>().TriggerExchangeSound();
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
        if(!hammerRef.GetComponent<Mallet>().isHeld)
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

    private IEnumerator StartAudioSourcesAfter1Second()
    {
        yield return new WaitForSeconds(1);
        ToggleAudioSourcesInScene(true);
    }
}