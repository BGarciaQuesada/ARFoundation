using System;

// Para evitar referencias circulares, creo esta clase de eventos globales

public static class GameEvents
{
    public static Action OnPlayerDeath;
    public static Action<int> OnCollectiblePicked;
}
