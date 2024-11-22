using UnityEngine;

public class TurboTanque : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Turbo recogido. Activando turbo...");
            ActivarTurbo(); // Activar lógica del turbo
            Destroy(gameObject); // Destruir el objeto turbo
            WebSocketController.Instance.SendMessage("speed:255");
            WebSocketController.Instance.SendMessage("forward");
        }
    }

    private void ActivarTurbo()
    {
        // Aquí puedes implementar la lógica del turbo
        Debug.Log("Turbo activado con éxito.");
    }
}
