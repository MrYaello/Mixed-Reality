using UnityEngine;
using System.Collections;

public class Obstaculos : MonoBehaviour
{
    private void Start()
    {       
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Chocaste contra un obstáculo...");
            ActivarChoque();
            Destroy(gameObject);
        }
    }

    private void ActivarChoque()
    {
        Debug.Log("Turbo activado con éxito.");
        StartCoroutine(speedDown());

    }

    IEnumerator speedDown()
    {
        WebSocketController.Instance.SendMessage("speed:100");
        yield return new WaitForSeconds(5);
        WebSocketController.Instance.SendMessage("speed:200");
    }
}
