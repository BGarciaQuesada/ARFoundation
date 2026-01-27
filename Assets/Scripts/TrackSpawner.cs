using UnityEngine;
using System.Collections.Generic;

public class TrackSpawner : MonoBehaviour
{
    public GameObject trackPrefab;
    public GameObject[] obstaclePrefab;

    public int initialPieces = 5;
    public float pieceLength = 5f;
    public float speed = 2f;
    public float obstacleChance = 0.5f;

    public float speedIncreaseInterval = 5f;
    public float speedIncreaseAmount = 0.2f;

    private float speedTimer = 0f;


    private Queue<GameObject> trackQueue = new Queue<GameObject>();
    private float spawnZ = 0f;

    public bool gameOver = false;

    // Activo: escucha evento de muerte del jugador
    void OnEnable()
    {
        GameEvents.OnPlayerDeath += OnGameOver;
    }

    // Desactivo: deja de escuchar evento de muerte del jugador
    void OnDisable()
    {
        GameEvents.OnPlayerDeath -= OnGameOver;
    }

    // Poner boolean a true que se emplea en Update para dejar de mover las piezas
    void OnGameOver()
    {
        gameOver = true;
    }

    void Start()
    {
        for (int i = 0; i < initialPieces; i++)
        {
            SpawnPiece();
        }
    }

    void Update()
    {
        if (gameOver) return;

        foreach (GameObject piece in trackQueue)
        {
            piece.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        if (trackQueue.Peek().transform.position.z < -pieceLength)
        {
            RemovePiece();
            SpawnPiece();
        }

        // AUMENTO DE VELOCIDAD CADA X SEGUNDOS
        speedTimer += Time.deltaTime;

        if (speedTimer >= speedIncreaseInterval)
        {
            // Ha pasado el intervalo, aumentar la velocidad
            speed += speedIncreaseAmount;
            speedTimer = 0f; // Reiniciar el temporizador

            Debug.Log("Velocidad aumentada a: " + speed);
        }
    }

    void SpawnPiece()
    {
        // Con el sistema anterior no funcionaba bien el que cada x tiempo, se aumentase la velocidad:
        // SpawnZ continuamente aumentaba pero las piezas se quedaban atrás, por lo que había huecos.
        // Ahora lo que hago es ver la última pieza, leer su Z y colocarla justo después.

        GameObject piece = Instantiate(trackPrefab);

        spawnZ = 0f;

        // ¿No hay pieza (aka. inicio)? Comenzar en 0 (luego evitar que aparezca un obstáculo en la primera pieza)
        if (trackQueue.Count == 0)
        {
            spawnZ = 0f;
        }
        else
        {
            // Resetea la "última pieza" y la busca en el queue
            GameObject lastPiece = null;
            foreach (var p in trackQueue) // final de bucle -> ultima pieza
                lastPiece = p;

            // Coge la Z de la última pieza y le suma la longitud de pieza
            spawnZ = lastPiece.transform.position.z + pieceLength;
        }

        piece.transform.position = new Vector3(0, -0.3f, spawnZ);

        // Posible obstáculo
        if (Random.value < obstacleChance)
        {
            SpawnObstacle(piece.transform);
        }

        trackQueue.Enqueue(piece);
    }


    void SpawnObstacle(Transform parent)
    {
        float[] lanes = { -0.4f, 0f, 0.4f };
        float x = lanes[Random.Range(0, lanes.Length)];

        GameObject obstacle = Instantiate(obstaclePrefab[Random.Range(0, obstaclePrefab.Length)]);
        obstacle.transform.SetParent(parent);
        obstacle.transform.localPosition = new Vector3(x, 0.3f, 0);
    }

    void RemovePiece()
    {
        GameObject oldPiece = trackQueue.Dequeue();
        Destroy(oldPiece);
    }
}
