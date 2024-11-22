using UnityEngine;
using System;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using System.Collections;

public class WebSocketController : MonoBehaviour
{
    public static WebSocketController Instance;
    private WebSocket websocket;
    public String ip = "192.168.1.85";
    public GameObject confirmation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        // Inicializar conexi?n WebSocket
        Debug.Log("Trying connection");
        websocket = new WebSocket("ws://" + ip + ":81"); // Cambia a la IP y puerto de tu ESP32

        // Eventos de WebSocket
        websocket.OnOpen += () =>
        {
            Debug.Log("Conexi?n WebSocket abierta en " + ip);
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
            Debug.Log("Conexi?n WebSocket cerrada");
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
        // Cerrar la conexi?n al salir de la aplicaci?n
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
