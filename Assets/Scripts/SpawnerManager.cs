using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabTurbo; 
    public GameObject prefabObstaculo; 

    [Header("Configuraciones")]
    public float distanciaFrenteJugador = 3f; 
    public Transform jugador; 

    private float tiempoSiguienteGeneracion;

    private void Start()
    {
      
        tiempoSiguienteGeneracion = Random.Range(5f, 10f);
    }

    private void Update()
    {
        
        tiempoSiguienteGeneracion -= Time.deltaTime;

        if (tiempoSiguienteGeneracion <= 0f)
        {
            var generate = Random.Range(1f, 3f);
            if (generate > 1.5f)
            {
                GenerarObjeto();
            }
            tiempoSiguienteGeneracion = Random.Range(7f, 15f); 
        }
    }

    private void GenerarObjeto()
    {
        if (jugador == null) return;

        
        GameObject prefabAGenerar = Random.value > 0.5f ? prefabTurbo : prefabObstaculo;

        
        Vector3 posicionGeneracion = jugador.position + jugador.forward * distanciaFrenteJugador;

      
        Instantiate(prefabAGenerar, posicionGeneracion, Quaternion.identity);
        Debug.Log($"{prefabAGenerar.name} generado frente al jugador.");
    }
}
