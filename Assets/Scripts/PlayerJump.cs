using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public bool isGrounded = true;
    public int vidas = 3;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Tocó el suelo");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ontrigger");
        if (other.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("Colisión con: " + other.gameObject.name);
            vidas--;
            Debug.Log("Vidas restantes: " + vidas);
            if(vidas <= 0)
                Time.timeScale = 0;  // Pausa el juego
        }
    }
}
