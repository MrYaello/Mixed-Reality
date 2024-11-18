using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultiPrefabSpawner : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [Header("Prefabs por imagen detectada")]
    public GameObject arcoPrefab;
    public GameObject metaPrefab;
    public GameObject objetoRandomPrefab;

    void Awake()
    {
        // Obtenemos el componente ARTrackedImageManager del mismo objeto
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            SpawnPrefabForImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdatePrefabForImage(trackedImage);
        }
    }

    private void SpawnPrefabForImage(ARTrackedImage trackedImage)
    {
        GameObject prefabToSpawn = null;

        // Selecciona el prefab según el nombre de la imagen detectada
        switch (trackedImage.referenceImage.name)
        {
            case "arco":
                prefabToSpawn = arcoPrefab;
                break;
            case "meta":
                prefabToSpawn = metaPrefab;
                break;
            case "objetoRandom":
                prefabToSpawn = objetoRandomPrefab;
                break;
        }

        if (prefabToSpawn != null)
        {
            // Instancia el prefab en la posición y rotación de la imagen detectada
            GameObject instance = Instantiate(prefabToSpawn, trackedImage.transform.position, trackedImage.transform.rotation);
            instance.transform.parent = trackedImage.transform; // Vincula el prefab a la imagen detectada
        }
    }

    private void UpdatePrefabForImage(ARTrackedImage trackedImage)
    {
        // Opcional: Puedes actualizar algo cuando la imagen rastreada cambie
    }
}
