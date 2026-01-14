using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPiece()
    {
        GameObject piece = Instantiate(trackPrefab);
        piece.transform.position = new Vector3(0, -0.3f, spawnZ);

        // Posible obstáculo
        if(Random.value < obstacleSpawnChance)
        {
            SpawnObstacle(piece.transform);
        }
        spawnZ += pieceLength;
        trackQueue()
    }
}
