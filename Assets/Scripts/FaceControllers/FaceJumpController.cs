using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceJumpController : MonoBehaviour
{
    public ARFace face;
    public Rigidbody playerRb;
    public PlayerJump player;

    [Header("Jump Settings")]
    public float jumpForce = 4f;
    public float pitchThreshold = -15f;
    public float cooldown = 0.8f;

    private bool isGrounded = true;
    private float lastJumpTime;

    void Update()
    {
        if (face == null || playerRb == null) return;

        Vector3 rotation = face.transform.localEulerAngles;
        float pitch = rotation.x;
        if (pitch > 180) pitch -= 360;

        if (pitch < pitchThreshold && player.isGrounded && Time.time - lastJumpTime > cooldown)
        {
            Jump();
        }
    }

    void Jump()
    {
        // UIManager.Instance.MostrarMensaje("Jump");
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        player.isGrounded = false;
        lastJumpTime = Time.time;
    }
}
