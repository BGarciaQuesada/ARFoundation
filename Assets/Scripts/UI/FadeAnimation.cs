using UnityEngine;

public class FadeAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;

    private bool fadeIn = false;

    // Activo: escucha evento de muerte del jugador
    void OnEnable()
    {
        GameEvents.OnPlayerDeath += ShowUI;
    }

    // Desactivo: deja de escuchar evento de muerte del jugador
    void OnDisable()
    {
        GameEvents.OnPlayerDeath -= ShowUI;
    }


    // [!] Que esto podría hacerse con FREE DOTween... Pero como es una sola animación tan chica, a lo tradicional
    public void ShowUI()
    {
        fadeIn = true;
    }

    // [!] No quería usar update porque siento que son miles de llamadas, pero las formas que he probado sin delta.Time van mal
    private void Update()
    {
        if(fadeIn)
            if(myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime;

                if(myUIGroup.alpha >= 1 )
                    fadeIn = false;
            }
    }
}