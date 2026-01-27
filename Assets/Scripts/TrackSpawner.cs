using UnityEngine;
using System.Collections.Generic;

public class TrackSpawner : MonoBehaviour
{
    public GameObject trackPrefab;
    public GameObject[] obstaclePrefabs;
    public GameObject[] collectablePrefabs;

    public int initialPieces = 5;
    public float pieceLength = 5f;
    public float speed = 2f;
    public float obstacleChance = 0.5f;
    public float collectableChance = 0.5f;

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

        // [!] Lo tengo que declarar aquí porque para cada track piece se reinicia
        // Carriles (para comprobar si están ocupados o no)
        bool[] laneOccupied = new bool[3];

        // Posible obstáculo
        if (Random.value < obstacleChance)
            SpawnObject(piece.transform, obstaclePrefabs, laneOccupied);
        
        // Posible coleccionable
        if (Random.value < collectableChance)
            SpawnObject(piece.transform, collectablePrefabs, laneOccupied);

        trackQueue.Enqueue(piece);
    }

    void SpawnObject(Transform parent, GameObject[] objectPrefab, bool[] laneOccupied)
    {
        // Lista de lanes disponibles (list porque no va a saber el tamaño hasta que llegue)
        List<int> freeLanes = new List<int>();

        // Se recorre el array de lanes pasado. ¿No está ocupada la posición? A freeLanes
        for (int i = 0; i < laneOccupied.Length; i++)
        {
            if (!laneOccupied[i])
            {
                freeLanes.Add(i);
            }
        }

        // Si no queda ningún lane libre, fuera
        if (freeLanes.Count == 0)
            return;

        // Hay lanes libres, elegir uno al azar
        int laneIndex = freeLanes[Random.Range(0, freeLanes.Count)];

        float[] lanePositions = { -0.4f, 0f, 0.4f };
        float x = lanePositions[laneIndex]; // Ya no coge una random de los lanes, solo busca EN LOS QUE HAY LIBRES

        // Instanciar el objeto como antes...
        GameObject objectSpawned = Instantiate(objectPrefab[Random.Range(0, objectPrefab.Length)]);
        objectSpawned.transform.SetParent(parent);
        objectSpawned.transform.localPosition = new Vector3(x, 0.3f, 0);

        // Marcar lane como ocupado en la posición correspondiente
        laneOccupied[laneIndex] = true;
    }

    void RemovePiece()
    {
        GameObject oldPiece = trackQueue.Dequeue();
        Destroy(oldPiece);
    }
}
