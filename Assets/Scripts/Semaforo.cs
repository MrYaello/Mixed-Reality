using System.Collections;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    public GameObject rojo;
    public GameObject amarillo;
    public GameObject verde;
    public GameObject button;
    public GameManager gameManager;

    public void IniciarCuentaAtras()
    {
        StartCoroutine(CuentaAtras());
    }

    private IEnumerator CuentaAtras()
    {
        button.SetActive(false);
        rojo.SetActive(true);
        amarillo.SetActive(false);
        verde.SetActive(false);
        yield return new WaitForSeconds(1f);

        rojo.SetActive(false);
        amarillo.SetActive(true);
        yield return new WaitForSeconds(1f);

        amarillo.SetActive(false);
        verde.SetActive(true);
        yield return new WaitForSeconds(1f);

        verde.SetActive(false);
        gameManager.IniciarJuego();
    }
}
