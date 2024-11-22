using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    [Header("UI Elements")]
    public TMP_Text puntosText; 
    public TMP_Text tiempoText; 
    public TMP_Text tiempoFinalText; 
    public GameObject victoriaPanel; 

    [Header("Game Settings")]
    public int puntosMaximos = 5; 
    private int puntos = 0; 
    private float tiempo = 0f; 
    private bool juegoIniciado = false; 

    void Awake()
    {
        // Configuración Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        victoriaPanel.SetActive(false); 
        ActualizarPuntos();
        ActualizarTiempo();
        tiempoFinalText.text = ""; 
    }

    void Update()
    {
        if (juegoIniciado)
        {
            tiempo += Time.deltaTime;
            ActualizarTiempo();
        }
    }

    public void IncrementarPuntos()
    {
        puntos++;
        Debug.Log($"Puntos incrementados: {puntos}");
        ActualizarPuntos();

        if (puntos >= puntosMaximos)
        {
            Debug.Log("¡Juego terminado! Has alcanzado el máximo de puntos.");
            FinalizarJuego();
        }
    }

    private void ActualizarPuntos()
    {
        puntosText.text = $"{puntos} / {puntosMaximos}";
    }

    private void ActualizarTiempo()
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        int milisegundos = Mathf.FloorToInt((tiempo * 1000) % 1000);

        tiempoText.text = string.Format("{0:00}:{1:00}:{2:000}", minutos, segundos, milisegundos);
    }

    private void FinalizarJuego()
    {
        juegoIniciado = false; // Detenemos el cronómetro
        victoriaPanel.SetActive(true);

        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        int milisegundos = Mathf.FloorToInt((tiempo * 1000) % 1000);

        tiempoFinalText.text = "Tu tiempo: " + string.Format("{0:00}:{1:00}:{2:000}", minutos, segundos, milisegundos);
        Debug.Log($"Juego terminado. Tiempo final: {tiempoFinalText.text}");
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene("SampleScene");

    }

    public void IniciarJuego()
    {
        puntos = 0;
        tiempo = 0f;
        juegoIniciado = true;
        ActualizarPuntos();
        ActualizarTiempo();
        victoriaPanel.SetActive(false);
        Debug.Log("El juego ha comenzado.");
    }
}
