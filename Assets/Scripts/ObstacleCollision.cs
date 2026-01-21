using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject smokeParticlesPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Partículas
            if (smokeParticlesPrefab != null)
            {
                Instantiate(smokeParticlesPrefab, transform.position, Quaternion.identity);
            }

            // Destruir obstáculo
            Destroy(gameObject);
        }
    }
}
