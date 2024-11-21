using System.Collections;
using UnityEngine;
using Unity.WebRTC;
using UnityEngine.UI;

public class WebRTCReceiver : MonoBehaviour
{
    [SerializeField] private RawImage videoOutput;
    [SerializeField] private SignalingClientReceiver signalingClient;

    private RTCPeerConnection peerConnection;

    private void Start()
    {
        var receiveStream = new MediaStream();
        receiveStream.OnAddTrack = e => {
            if (e.Track is VideoStreamTrack track)
            {
                videoOutput.texture = track.Texture;
            }
        };
        // Configuración básica de ICE
        RTCConfiguration config = new RTCConfiguration
        {
            iceServers = new[] { new RTCIceServer { urls = new[] { "stun:stun.l.google.com:19302" } } }
        };

        peerConnection = new RTCPeerConnection(ref config);
        peerConnection.OnTrack = (RTCTrackEvent e) => {
            if (e.Track.Kind == TrackKind.Video)
            {
                receiveStream.AddTrack(e.Track);
            }
        };
        peerConnection.OnIceCandidate = OnIceCandidate;

        signalingClient.OnOfferReceived += HandleOffer;
        signalingClient.OnIceCandidateReceived += HandleIceCandidate;

        StartCoroutine(WebRTC.Update());
    }

    private void OnIceCandidate(RTCIceCandidate candidate)
    {
        Debug.Log($"ICE Candidate recibido: {candidate.Candidate}");
        signalingClient.SendIceCandidate(candidate.Candidate);
    }

    private void HandleOffer(string sdp)
    {
        // Procesar la oferta recibida
        var offer = new RTCSessionDescription { type = RTCSdpType.Offer, sdp = sdp };
        peerConnection.SetRemoteDescription(ref offer);
        StartCoroutine(CreateAnswer());
    }

    private IEnumerator CreateAnswer()
    {
        var op = peerConnection.CreateAnswer();
        yield return op;

        if (!op.IsError)
        {
            var answer = op.Desc;
            peerConnection.SetLocalDescription(ref answer);
            signalingClient.SendAnswer(answer.sdp);
        }
    }

    private void HandleIceCandidate(string candidate)
    {
        Debug.Log($"Candidato ICE recibido: {candidate}");
        RTCIceCandidateInit candidateInit = new RTCIceCandidateInit
        {
            candidate = candidate,
            sdpMid = "video",
            sdpMLineIndex = 0
        };
        var iceCandidate = new RTCIceCandidate(candidateInit);
        peerConnection.AddIceCandidate(iceCandidate);
    }

    private void OnDestroy()
    {
        signalingClient.OnOfferReceived -= HandleOffer;
        signalingClient.OnIceCandidateReceived -= HandleIceCandidate;

        peerConnection?.Dispose();
    }
}
