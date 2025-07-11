using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            GameManager.Instance.UpdateLives();
        }
    }

}
