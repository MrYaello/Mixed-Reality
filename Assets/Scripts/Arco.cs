using UnityEngine;

public class Arco : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Detecta si el objeto que pasa tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador pasó por el arco.");
            GameManager.Instance.IncrementarPuntos(); // Incrementar puntos en el GameManager
        }
    }
}
