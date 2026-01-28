using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    [SerializeField] private GameObject hidablePanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ManageTraining()
    {
        if (hidablePanel.activeSelf)
            hidablePanel.SetActive(false);
        else
            hidablePanel.SetActive(true);
    }
}
