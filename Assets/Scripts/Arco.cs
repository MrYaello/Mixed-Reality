using UnityEngine;

public class Arco : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("La cámara pasó por el arco.");
            GameManager.Instance.IncrementarPuntos(); // Asegúrate de que GameManager esté configurado como Singleton
        }
    }
}
