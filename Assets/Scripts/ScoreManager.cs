using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public int score = 0;

    // Actualizar UI para empezar en 0
    private void Awake()
    {
        scoreText.text = "Pts: 0";
    }

    private void OnEnable()
    {
        GameEvents.OnCollectiblePicked += AddScore;
    }

    private void OnDisable()
    {
        GameEvents.OnCollectiblePicked -= AddScore;
    }

    public void AddScore(int amount)
    {
        Debug.Log("Score: " + score);
        scoreText.text = "Pts: " + (score+=amount);
    }
}
