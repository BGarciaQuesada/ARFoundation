using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private AudioSource buttonSound;

    public void RestartGame()
    {
        StartCoroutine(WaitAndLoad());
    }

    private IEnumerator WaitAndLoad()
    {
        buttonSound.Play();
        yield return new WaitForSeconds(buttonSound.clip.length); // Esperar a que termine el sonido
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
