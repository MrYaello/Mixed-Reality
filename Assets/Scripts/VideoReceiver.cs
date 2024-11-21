using UnityEngine;
using NativeWebSocket;
using UnityEngine.UI;
using System;

public class VideoReceiver : MonoBehaviour
{
    private WebSocket socket;
    private Texture2D receivedTexture;
    public RawImage rawImage;

    async void Start()
    {
        // Conectar al servidor WebSocket
        socket = new WebSocket("ws://192.168.1.91:3000");

        socket.OnOpen += () => { Debug.Log("Conectado al servidor de WebSockets"); };
        socket.OnMessage += (bytes) =>
        {
            // Recibir el fotograma y convertirlo a una textura
            receivedTexture = new Texture2D(2, 2); // Asignar tamaño temporal
            receivedTexture.LoadImage(bytes); // Cargar la imagen
            Debug.Log("Get Image");
            // Mostrarla en una RawImage
            rawImage.texture = receivedTexture;
        };

        await socket.Connect();
    }

    void OnApplicationQuit()
    {
        if (socket != null && socket.State == WebSocketState.Open)
        {
            socket.Close();
        }
    }

    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
                // Ejecutar tareas de WebSocket en plataformas que no sean WebGL
                socket?.DispatchMessageQueue();
        #endif
    }
}
