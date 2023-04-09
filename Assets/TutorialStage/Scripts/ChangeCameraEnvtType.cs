using UnityEngine;

/// <summary>
/// Allows changing of the camera background to skybox or solid colour.
/// </summary>
public class ChangeCameraEnvtType : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public void ChangeToSolidColour()
    {
        cam.clearFlags = CameraClearFlags.SolidColor;
    }
    public void ChangeToSkybox()
    {
        cam.clearFlags = CameraClearFlags.Skybox;
    }
}
