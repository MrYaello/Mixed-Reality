using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text puntosText; // Texto para mostrar los puntos
    public TMP_Text tiempoText; // Texto para mostrar el cronómetro
    public TMP_Text tiempoFinalText; // Texto para mostrar el tiempo final
    public GameObject victoriaPanel; // Panel de victoria

    private int puntos = 0;
    private int puntosMaximos = 5;
    private float tiempo = 0f;
    private bool juegoIniciado = false;

    void Start()
    {
        victoriaPanel.SetActive(false);
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
        ActualizarPuntos();

        if (puntos >= puntosMaximos)
        {
            FinalizarJuego();
        }
    }

    private void ActualizarPuntos()
    {
        puntosText.text = puntos + " / " + puntosMaximos;
    }

    private void ActualizarTiempo()
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        tiempoText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    private void FinalizarJuego()
    {
        juegoIniciado = false; // Detenemos el cronómetro
        victoriaPanel.SetActive(true);

        // Mostrar tiempo final
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        tiempoFinalText.text = "Tu tiempo: " + string.Format("{0:00}:{1:00}", minutos, segundos);
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
    }
}
