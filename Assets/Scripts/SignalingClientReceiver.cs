using System;
using UnityEngine;
using NativeWebSocket;  // Usamos WebSocket4Net en lugar de SocketIOClient
using System.Text;

public class SignalingClientReceiver : MonoBehaviour
{
    // Definir los eventos
    public event Action<string> OnOfferReceived;
    public event Action<string> OnIceCandidateReceived;

    private WebSocket socket;

    async void Start()
    {
        socket = new WebSocket("ws://localhost:3000");

        socket.OnOpen += () => { Debug.Log("Conectado al servidor de señalización"); };
        socket.OnMessage += (e) =>
        {
            var data = JsonUtility.FromJson<SignalData>(Encoding.UTF8.GetString(e));
            Debug.Log($"Mensaje recibido: {data}");
            HandleSignal(data);
        };
        socket.OnError += (error) => { Debug.LogError($"Error en WebSocket: {error}"); };
        socket.OnClose += (closeCode) => { Debug.Log($"WebSocket cerrado con código: {closeCode}"); };

        await socket.Connect();
    }

    private void HandleSignal(SignalData data)
    {
        if (data.type == "offer")
        {
            OnOfferReceived?.Invoke(data.sdp);  // Disparar el evento de oferta
        }
        else if (data.type == "ice-candidate")
        {
            OnIceCandidateReceived?.Invoke(data.candidate);  // Disparar el evento de candidato ICE
        }
    }

    public void SendIceCandidate(string candidate)
    {
        SendSignal(new SignalData { type = "ice-candidate", candidate = candidate });
    }

    public void SendOffer(string sdp)
    {
        SendSignal(new SignalData { type = "offer", sdp = sdp });
    }

    public void SendAnswer(string sdp)
    {
        SendSignal(new SignalData { type = "answer", sdp = sdp });
    }

    private void SendSignal(SignalData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        socket.SendText(jsonData);
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
                socket?.DispatchMessageQueue();
        #endif
    }
}
