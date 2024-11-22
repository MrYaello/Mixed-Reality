using System.Collections;
using UnityEngine;

public class TurboTanque : MonoBehaviour
{
    public GameObject turbo;
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
        }
    }

    private void ActivarTurbo()
    {
        Debug.Log("Turbo activado con éxito.");
        StartCoroutine(speedUp());

    }

    IEnumerator speedUp()
    {
        turbo.SetActive(true);
        WebSocketController.Instance.SendMessage("speed:255");
        yield return new WaitForSeconds(5);
        WebSocketController.Instance.SendMessage("speed:200");
        turbo.SetActive(false);
    }
}
