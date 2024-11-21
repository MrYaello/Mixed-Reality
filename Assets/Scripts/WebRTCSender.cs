using System.Collections;
using UnityEngine;
using Unity.WebRTC;

public class WebRTCSender : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private SignalingClient signalingClient;

    private VideoStreamTrack videoTrack;
    private AudioStreamTrack audioTrack;
   

    private void Start()
    {   
        var peerConnection = signalingClient.peerConnection;

        // Inicia captura de video y audio
        StartCapture(peerConnection);
        StartCoroutine(WebRTC.Update());
    }

    private void StartCapture(RTCPeerConnection peerConnection)
    {
        videoTrack = cam.CaptureStreamTrack(1280, 720);
        peerConnection.AddTrack(videoTrack);

        Debug.Log("Captura iniciada.");
    }

    private void OnDestroy()
    {
        videoTrack?.Dispose();
    }
}
