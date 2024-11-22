using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public List<ImagePrefabPair> imagePrefabPairs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    [System.Serializable]
    public struct ImagePrefabPair
    {
        public string imageName;
        public GameObject prefab;
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            SpawnPrefab(trackedImage);
        }

        foreach (var trackedImage in args.updated)
        {
          
        }

        foreach (var trackedImage in args.removed)
        {
            RemovePrefab(trackedImage);
        }
    }

    private void SpawnPrefab(ARTrackedImage trackedImage)
    {
        foreach (var pair in imagePrefabPairs)
        {
            if (trackedImage.referenceImage.name == pair.imageName)
            {
                GameObject prefabInstance = Instantiate(pair.prefab);

                
                Camera mainCamera = Camera.main;
                prefabInstance.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2.0f;

                prefabInstance.transform.rotation = Quaternion.LookRotation(-mainCamera.transform.forward, Vector3.up);

                spawnedPrefabs[trackedImage.referenceImage.name] = prefabInstance;

                Debug.Log($"Prefab {pair.imageName} generado frente a la cámara.");
                return;
            }
        }
    }

    private void RemovePrefab(ARTrackedImage trackedImage)
    {
        if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
        {
            Destroy(spawnedPrefabs[trackedImage.referenceImage.name]);
            spawnedPrefabs.Remove(trackedImage.referenceImage.name);
            Debug.Log($"Prefab {trackedImage.referenceImage.name} eliminado.");
        }
    }
}
