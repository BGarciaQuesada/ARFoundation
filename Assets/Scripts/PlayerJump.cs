using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("SFX Entorno")]
    [SerializeField] private AudioSource startSFX;
    [SerializeField] private AudioSource movementSFX;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource collectSFX;

    [Header("Estado del jugador")]
    public bool isGrounded = true;
    public int vidas = 3;

    Rigidbody rb;
    public GameObject smokeParticlesPrefab;

    void Start()
    {
        // Automáticamente obtener el Rigidbody
        rb = GetComponent<Rigidbody>();

        // Sonido de inicio del juego
        if (startSFX != null)
            startSFX.Play();
        // Sonido constante del carro
        if (movementSFX != null)
        {
            movementSFX.loop = true;
            movementSFX.Play();
        }
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
        Debug.Log("Colisión con: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Obstacle"))
        {
            // Sonido de golpe
            if (hitSFX != null)
                hitSFX.Play();

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
        if (other.gameObject.CompareTag("Collectible"))
        {
            // Sonido de coleccionable
            if (collectSFX != null)
                collectSFX.Play();

            Debug.Log("¡Recogiste un coleccionable!");
            // Sumar puntuación (nuevamente, hardcodeado, pero es fácil de escalar)
            GameEvents.OnCollectiblePicked?.Invoke(10); // Así me evito que PlayerJump conozca el texto de UI
            Destroy(other.gameObject);
        }
    }

    void Muerte()
    {
        // Parar el sonido de movimiento y reproducir el de muerte
        if (movementSFX != null)
            movementSFX.Stop();
        if (deathSFX != null)
            deathSFX.Play();

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
