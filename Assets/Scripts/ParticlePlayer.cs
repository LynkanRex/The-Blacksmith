using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{

    private List<ParticleSystem> pSystems;
    
    private void Awake()
    {
        pSystems = new List<ParticleSystem>();
        
        var systemsInChildren = GetComponentsInChildren<ParticleSystem>();
        
        foreach (var system in systemsInChildren)
        {
            pSystems.Add(system);
        }
    }
    
    void Start()
    {
        foreach (var system in pSystems)
        {
            system.Play();
        }       
    }
}
