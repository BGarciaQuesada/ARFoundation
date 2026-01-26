using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puntaje;

    private float elapsedTime;
    private bool muerto = false;

    // Activo: escucha evento de muerte del jugador
    void OnEnable()
    {
        GameEvents.OnPlayerDeath += Muerte;
    }

    // Desactivo: deja de escuchar evento de muerte del jugador
    void OnDisable()
    {
        GameEvents.OnPlayerDeath -= Muerte;
    }

    private void Muerte()
    {
        muerto = true;
    }

    private void Update()
    {
        if (muerto) return;

        elapsedTime += Time.deltaTime;
        puntaje.text = "Mts: " + Mathf.FloorToInt(elapsedTime);
    }
}
