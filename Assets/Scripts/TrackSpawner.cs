using UnityEngine;
using System.Collections.Generic;

public class TrackSpawner : MonoBehaviour
{
    public GameObject trackPrefab;
    public GameObject obstaclePrefab;

    public int initialPieces = 5;
    public float pieceLength = 4f;
    public float speed = 2f;
    public float obstacleChance = 0.5f;

    private Queue<GameObject> trackQueue = new Queue<GameObject>();
    private float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < initialPieces; i++)
        {
            SpawnPiece();
        }
    }

    void Update()
    {
        foreach (GameObject piece in trackQueue)
        {
            piece.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        if (trackQueue.Peek().transform.position.z < -pieceLength)
        {
            RemovePiece();
            SpawnPiece();
        }
    }

    void SpawnPiece()
    {
        GameObject piece = Instantiate(trackPrefab);
        piece.transform.position = new Vector3(0, -1f, spawnZ);

        // Posible obstáculo
        if (Random.value < obstacleChance)
        {
            SpawnObstacle(piece.transform);
        }

        spawnZ += pieceLength;
        trackQueue.Enqueue(piece);
    }

    void SpawnObstacle(Transform parent)
    {
        float[] lanes = { -0.5f, 0f, 0.5f };
        float x = lanes[Random.Range(0, lanes.Length)];

        GameObject obstacle = Instantiate(obstaclePrefab);
        obstacle.transform.SetParent(parent);
        obstacle.transform.localPosition = new Vector3(x, 0.3f, 0);
    }

    void RemovePiece()
    {
        GameObject oldPiece = trackQueue.Dequeue();
        Destroy(oldPiece);
    }
}
