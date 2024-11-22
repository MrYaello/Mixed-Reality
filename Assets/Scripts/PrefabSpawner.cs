using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject prefabToSpawn;
    public Vector3 rotationOffset = new Vector3 (270, 0, 0);
    public Vector3 positionOffset = new Vector3(0, 0, -0.04f);

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Cuando una nueva imagen es detectada
        foreach (var trackedImage in args.added)
        {
            SpawnPrefabWithRotation(trackedImage);
        }

        // Cuando una imagen ya trackeada cambia su estado
        foreach (var trackedImage in args.updated)
        {
            UpdatePrefabPositionAndRotation(trackedImage);
        }

        // Cuando una imagen deja de ser trackeada (opcional)
        foreach (var trackedImage in args.removed)
        {
            // Opcionalmente destruye objetos relacionados
            Destroy(trackedImage.transform.GetChild(0)?.gameObject);
        }
    }

    private void SpawnPrefabWithRotation(ARTrackedImage trackedImage)
    {
        // Instancia el prefab como hijo de la imagen trackeada
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, trackedImage.transform);

        // Ajusta la posición y rotación del prefab
        spawnedPrefab.transform.localPosition = positionOffset; // Alineado al centro de la imagen
        spawnedPrefab.transform.localRotation = Quaternion.Euler(rotationOffset); // Rotado según el offset
    }

    private void UpdatePrefabPositionAndRotation(ARTrackedImage trackedImage)
    {
        // Asegúrate de que el prefab siga alineado al trackeo
        Transform prefab = trackedImage.transform.GetChild(0); // El primer hijo es el prefab
        if (prefab != null)
        {
            prefab.localPosition = positionOffset;
            prefab.localRotation = Quaternion.Euler(rotationOffset);
        }
    }
}
