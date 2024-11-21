using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.WebRTC;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class CarStreaming : MonoBehaviour
{
    private RTCPeerConnection peerConnection;

    // Inicializa WebRTC y la captura de la cámara AR
    void Start()
    {
        SetupWebRTC();
        StartCoroutine(WebRTC.Update());
  
    }

    // Configura WebRTC y la cámara AR
    void SetupWebRTC()
    {
        // Configura la conexión WebRTC
        peerConnection = new RTCPeerConnection();
        var sendChannel = peerConnection.CreateDataChannel("sendChannel");
        sendChannel.OnOpen = () => Debug.Log("DataChannel opened.");
        sendChannel.OnClose = () => Debug.Log("DataChannel closed.");

        var camera = GetComponent<Camera>();
        var track = camera.CaptureStreamTrack(1280, 720);

        peerConnection.AddTrack(track);
        var op = peerConnection.CreateOffer();
    }
}
