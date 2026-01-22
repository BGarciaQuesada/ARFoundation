using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceDetector : MonoBehaviour
{
    public ARFaceManager faceManager;
    public FaceRunnerController controller;
    public FaceJumpController jumpController;

    void OnEnable()
    {
        faceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        faceManager.facesChanged -= OnFacesChanged;
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            controller.face = args.added[0];
            jumpController.face = args.added[0];
        }
    }
}
