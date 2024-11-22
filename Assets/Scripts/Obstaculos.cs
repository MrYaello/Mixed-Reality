using UnityEngine;

public class Obstaculos : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Chocaste contra un obstaculo...");
            ActivarChoque(); 
            Destroy(gameObject);
            WebSocketController.Instance.SendMessage("speed:100");
            WebSocketController.Instance.SendMessage("forward");
        }
    }

    private void ActivarChoque()
    {
        
        Debug.Log("Choque, vas mas lento.");
    }
}
