using System;
using System.Timers;
using UnityEngine;

public class SellArea : MonoBehaviour
{

    private bool soldRecently;
    private float remainingTimer;
    private float cooldownTimer;

    private void Update()
    {
        if (soldRecently)
            remainingTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Dagger>() != null)
        {
            if (soldRecently)
                return;
            soldRecently = true;
            remainingTimer = cooldownTimer;
            FindObjectOfType<GameStateManager>().SellDagger(other.gameObject.GetComponent<Dagger>().index);
        }
    }
}
