using UnityEngine;

public class Arco : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador pasó por el arco.");
            GameManager.Instance.IncrementarPuntos(); 
        }
    }
}
