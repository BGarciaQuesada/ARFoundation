using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    [SerializeField] private GameObject hidablePanel;
    [SerializeField] private AudioSource buttonSound;

    public void StartGame()
    {
        StartCoroutine(WaitAndLoad());
    }

    public void ManageTraining()
    {
        if (hidablePanel.activeSelf)
            hidablePanel.SetActive(false);
        else
            hidablePanel.SetActive(true);
    }

    private IEnumerator WaitAndLoad()
    {
        buttonSound.Play();
        yield return new WaitForSeconds(buttonSound.clip.length); // Esperar a que termine el sonido
        SceneManager.LoadScene("GameScene");
    }

}
