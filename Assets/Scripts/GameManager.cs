using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para acceso global

    [Header("UI Elements")]
    public TMP_Text puntosText; // Texto para mostrar los puntos
    public TMP_Text tiempoText; // Texto para mostrar el cronómetro
    public TMP_Text tiempoFinalText; // Texto para mostrar el tiempo final
    public GameObject victoriaPanel; // Panel de victoria

    [Header("Game Settings")]
    public int puntosMaximos = 5; // Puntos necesarios para ganar
    private int puntos = 0; // Puntos actuales
    private float tiempo = 0f; // Tiempo transcurrido
    private bool juegoIniciado = false; // Bandera para saber si el juego está activo

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
        victoriaPanel.SetActive(false); // Asegúrate de que el panel de victoria esté oculto al inicio
        ActualizarPuntos();
        ActualizarTiempo();
        tiempoFinalText.text = ""; // Limpia el texto inicial
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

        // Mostrar tiempo final
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        int milisegundos = Mathf.FloorToInt((tiempo * 1000) % 1000);

        tiempoFinalText.text = "Tu tiempo: " + string.Format("{0:00}:{1:00}:{2:000}", minutos, segundos, milisegundos);
        Debug.Log($"Juego terminado. Tiempo final: {tiempoFinalText.text}");
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
