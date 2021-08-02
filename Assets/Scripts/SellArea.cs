using UnityEngine;

public class SellArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Dagger>() != null)
        {
            FindObjectOfType<GameStateManager>().SellDagger(other.gameObject);
        }
    }
}
