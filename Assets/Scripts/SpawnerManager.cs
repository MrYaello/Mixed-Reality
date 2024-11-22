using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabTurbo; // Prefab del turbo
    public GameObject prefabObstaculo; // Prefab del obstáculo

    [Header("Configuraciones")]
    public float distanciaFrenteJugador = 3f; // Distancia frente al jugador
    public Transform jugador; // Referencia al jugador o cámaras

    private float tiempoSiguienteGeneracion;

    private void Start()
    {
        // Configurar el primer intervalo aleatorio
        tiempoSiguienteGeneracion = Random.Range(5f, 10f);
    }

    private void Update()
    {
        // Reducir el tiempo para la próxima generación
        tiempoSiguienteGeneracion -= Time.deltaTime;

        if (tiempoSiguienteGeneracion <= 0f)
        {
            var generate = Random.Range(1f, 3f);
            if (generate > 1.5f)
            {
                GenerarObjeto();
            }
            tiempoSiguienteGeneracion = Random.Range(7f, 15f); // Configurar el siguiente intervalo
        }
    }

    private void GenerarObjeto()
    {
        if (jugador == null) return;

        // Determinar aleatoriamente qué generar: turbo o obstáculo
        GameObject prefabAGenerar = Random.value > 0.5f ? prefabTurbo : prefabObstaculo;

        // Posicionar frente al jugador
        Vector3 posicionGeneracion = jugador.position + jugador.forward * distanciaFrenteJugador;

        // Instanciar el prefab
        Instantiate(prefabAGenerar, posicionGeneracion, Quaternion.identity);
        Debug.Log($"{prefabAGenerar.name} generado frente al jugador.");
    }
}
