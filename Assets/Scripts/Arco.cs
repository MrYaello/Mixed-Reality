using UnityEngine;

public class Arco : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            FindObjectOfType<GameManager>().IncrementarPuntos();
        }
    }
}
