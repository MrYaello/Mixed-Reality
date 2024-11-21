using System.Collections;
using UnityEngine;
using NativeWebSocket;  // Usamos WebSocket4Net en lugar de SocketIOClient
using Unity.WebRTC;
using System.Text;
using System;

public class SignalingClient : MonoBehaviour
{
    private WebSocket socket;  // Usamos WebSocket4Net en lugar de SocketIOUnity
    public RTCPeerConnection peerConnection { get; private set; }

    async void Start()
    {
        // Configurar conexión con el servidor de señalización
        socket = new WebSocket("ws://localhost:3000");  // Cambiar la URL si es necesario

        // Definir lo que pasa cuando el WebSocket se conecta
        socket.OnOpen += () =>
        {
            Debug.Log("Conectado al servidor de señalización");
        };

        var config = new RTCConfiguration
        {
            iceServers = new[] { new RTCIceServer { urls = new[] { "stun:stun.l.google.com:19302" } } }
        };
        peerConnection = new RTCPeerConnection(ref config);

        peerConnection.OnIceCandidate = candidate =>
        {
            SendSignal(new SignalData
            {
                type = "ice-candidate",
                candidate = candidate.Candidate
            });
        };

        peerConnection.OnNegotiationNeeded = () =>
        {
            StartCoroutine(CreateOffer());
        };

        // Manejar los mensajes entrantes
        socket.OnMessage += (e) =>
        {
            var data = JsonUtility.FromJson<SignalData>(Encoding.UTF8.GetString(e));
            Debug.Log($"Mensaje recibido: {data}");
            HandleSignal(data);
        };

        // Conectar el WebSocket
        await socket.Connect();

        // Configurar WebRTC
    }

    public void SendSignal(SignalData data)
    {
        string jsonData = JsonUtility.ToJson(data);  // Convertir el SignalData a JSON
        socket.SendText(jsonData);  // Enviar el mensaje al servidor
    }

    private IEnumerator CreateOffer()
    {
        var op = peerConnection.CreateOffer();
        yield return op;

        if (!op.IsError)
        {
            var offer = op.Desc;
            peerConnection.SetLocalDescription(ref offer);
            SendSignal(new SignalData
            {
                type = "offer",
                sdp = offer.sdp
            });
        }
    }

    private void HandleSignal(SignalData data)
    {
        if (data.type == "offer")
        {
            var offer = new RTCSessionDescription { type = RTCSdpType.Offer, sdp = data.sdp };
            peerConnection.SetRemoteDescription(ref offer);
            StartCoroutine(CreateAnswer());
        }
        else if (data.type == "answer")
        {
            RTCSessionDescription answer = new RTCSessionDescription { type = RTCSdpType.Answer, sdp = data.sdp };
            peerConnection.SetRemoteDescription(ref answer);
        }
        else if (data.type == "ice-candidate")
        {
            Debug.Log($"Candidato: {data.candidate}");
            RTCIceCandidateInit Candidate = new RTCIceCandidateInit
            {
                candidate = data.candidate,
                sdpMid = "video",
                sdpMLineIndex = 0
            };
            try
            {
                RTCIceCandidate candidate = new RTCIceCandidate(Candidate);
                peerConnection.AddIceCandidate(candidate);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al agregar candidato ICE: {ex.Message}");
                Debug.LogError($"Detalles del candidato: candidate='{Candidate.candidate}', sdpMid='{Candidate.sdpMid}', sdpMLineIndex='{Candidate.sdpMLineIndex}'");
            }
        }
    }

    private IEnumerator CreateAnswer()
    {
        var op = peerConnection.CreateAnswer();
        yield return op;

        if (!op.IsError)
        {
            var answer = op.Desc;
            peerConnection.SetLocalDescription(ref answer);
            SendSignal(new SignalData
            {
                type = "answer",
                sdp = answer.sdp
            });
        }
    }

    void OnApplicationQuit()
    {
        if (socket != null && socket.State == WebSocketState.Open)
        {
            socket.Close();  // Cerrar el WebSocket cuando la aplicación se cierre
        }
    }

    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
                socket?.DispatchMessageQueue();
        #endif
    }
}

[System.Serializable]
public class SignalData
{
    public string type;
    public string sdp;
    public string candidate;
}
