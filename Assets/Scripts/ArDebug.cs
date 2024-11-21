using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARDebug : MonoBehaviour
{
    void Start()
    {
        Debug.Log("AR Session started.");
    }

    void OnEnable()
    {
        Debug.Log("AR Session enabled.");
    }

    void OnDisable()
    {
        Debug.Log("AR Session disabled.");
    }
}
