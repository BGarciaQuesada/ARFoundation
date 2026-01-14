using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceRunnerController : MonoBehaviour
{

    public ARFace face;
    public Transform player;

    public float lateralSensitivity = 0.1f;
    public float maxX = 2f;

    void Update()
    {
        if (face == null || player == null)
            return;
        Vector3 headRotation = face.transform.localEulerAngles;

        float giroCabeza = headRotation.y;
        if (giroCabeza > 180) giroCabeza -= 360;

        // Mover al jugador lateralmente según la rotación de la cabeza
        float targetX = -giroCabeza * lateralSensitivity;
        targetX = Mathf.Clamp(targetX, -maxX, maxX);

        // Antigua forma de mover al jugador:
        // player.Translate(new Vector3(giroCabeza * Time.deltaTime, 0, 0));

        Vector3 pos = player.position;
        pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * 5f);
        player.position = pos;
    }
}
