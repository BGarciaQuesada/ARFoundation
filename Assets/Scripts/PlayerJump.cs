using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public bool isGrounded = true;
    public int vidas = 3;

    Rigidbody rb;
    public GameObject smokeParticlesPrefab;

    void Start()
    {
        // Automáticamente obtener el Rigidbody
        rb = GetComponent<Rigidbody>();
    }

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
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Colisión con: " + other.gameObject.name);
            vidas--;
            Debug.Log("Vidas restantes: " + vidas);

            // Particulas
            if (smokeParticlesPrefab != null)
                Instantiate(smokeParticlesPrefab, transform.position, Quaternion.identity);

            // Destruir obstáculo
            Destroy(other.gameObject);

            if (vidas <= 0)
            {
                // Time.timeScale = 0;  // Pausa el juego
                Muerte();
            }
                
        }
    }

    void Muerte()
    {
        // Quitar congelación de rotación
        rb.constraints = RigidbodyConstraints.None;

        // Añadir fuerza para que salga volando (hacia atrás y un poco hacia arriba)
        Vector3 direccion = Vector3.back + Vector3.up * 0.5f;
        rb.AddForce(direccion.normalized * 6f, ForceMode.Impulse);
        // Girar como loco
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        // Avisar al sistema de que lance el método de jugador muerto
        GameEvents.OnPlayerDeath?.Invoke();

        enabled = false;
    }
}
