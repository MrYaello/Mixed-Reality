using UnityEngine;
using NativeWebSocket;
using System.Text;

public class VideoSender : MonoBehaviour
{
    private WebSocket socket;
    private Texture2D cameraTexture;
    public Camera mainCamera;

    async void Start()
    {
        // Conectar WebSocket al servidor
        socket = new WebSocket("ws://192.168.1.91:3000");

        socket.OnOpen += () => { Debug.Log("Conectado al servidor de WebSockets"); SendVideoFrame(); };
        socket.OnError += (error) => { Debug.LogError($"Error en WebSocket: {error}"); };
        socket.OnClose += (closeCode) => { Debug.Log($"WebSocket cerrado con código: {closeCode}"); };

        // Crear una textura para capturar los fotogramas de la cámara
        cameraTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Enviar el video en intervalos
        InvokeRepeating("SendVideoFrame", 0f, 0.033f); // Enviar cada 33ms (30fps)

        await socket.Connect();
    }

    void SendVideoFrame()
    {
        // Renderizar la cámara a la textura
        mainCamera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24);
        mainCamera.Render();

        // Leer los píxeles de la cámara renderizada
        RenderTexture.active = mainCamera.targetTexture;
        cameraTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        cameraTexture.Apply();

        // Convertir la textura a un formato comprimido (JPEG o PNG)
        byte[] bytes = cameraTexture.EncodeToJPG(); // Usa PNG si prefieres menos compresión

        // Enviar los bytes del fotograma por WebSocket
        socket.Send(bytes);
        Debug.Log(bytes);

        // Liberar la textura temporal después de renderizar
        RenderTexture.ReleaseTemporary(mainCamera.targetTexture);
        mainCamera.targetTexture = null;
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
