using UnityEngine;

public class TurboTanque : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Turbo recogido. Activando turbo...");
            ActivarTurbo();
            Destroy(gameObject);
            WebSocketController.Instance.SendMessage("speed:255");
            WebSocketController.Instance.SendMessage("forward");
        }
    }

    private void ActivarTurbo()
    {
        Debug.Log("Turbo activado con éxito.");
    }
}
