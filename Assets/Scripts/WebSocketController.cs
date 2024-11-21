using UnityEngine;
using System;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using System.Collections;

public class WebSocketController : MonoBehaviour
{
    private WebSocket websocket;
    public String ip = "192.168.1.85";
    public GameObject confirmation;

    async void Start()
    {
        // Inicializar conexión WebSocket
        websocket = new WebSocket("ws://" + ip + ":81"); // Cambia a la IP y puerto de tu ESP32

        // Eventos de WebSocket
        websocket.OnOpen += () =>
        {
            Debug.Log("Conexión WebSocket abierta");
            StartCoroutine(ShowConfirmation());
        };

        websocket.OnMessage += (bytes) =>
        {
            // Recibir mensaje
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log($"Mensaje recibido: {message}");
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError($"Error en WebSocket: {e}");
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Conexión WebSocket cerrada");
        };

        // Conectar al servidor
        await websocket.Connect();
    }

    public async void SendMessage(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Enviar mensaje al servidor
            await websocket.SendText(message);
        }
    }

    private IEnumerator ShowConfirmation()
    {
        confirmation.SetActive(true);
        yield return new WaitForSeconds(1f);
        confirmation.SetActive(false);
    }

    private async void OnApplicationQuit()
    {
        // Cerrar la conexión al salir de la aplicación
        if (websocket != null)
        {
            await websocket.Close();
        }
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        // Ejecutar tareas de WebSocket en plataformas que no sean WebGL
        websocket?.DispatchMessageQueue();
#endif
    }
}
