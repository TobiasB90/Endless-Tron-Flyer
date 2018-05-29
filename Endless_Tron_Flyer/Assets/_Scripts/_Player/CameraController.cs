using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera MainCamera;
    public GameObject Player;
    PlayerController pcont;
    public float distance;
    public float updownoffset;
    Vector3 _velocity;
    private Vector3 moveCamTo;
    private Vector3 updownoffsetvector;


    private void Start()
    {
        pcont = Player.GetComponent<PlayerController>();
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        // Change updownoffset based on current tunneldirection
        switch (pcont.TunnelDir)
        {
            case PlayerController.TunnelDirection.Forward:
                updownoffsetvector = Vector3.up * updownoffset;
                break;
            case PlayerController.TunnelDirection.Up:
                updownoffsetvector = Vector3.back * updownoffset;
                break;
            case PlayerController.TunnelDirection.UpRight:
                updownoffsetvector = Vector3.back * updownoffset;
                break;
            case PlayerController.TunnelDirection.UpLeft:
                updownoffsetvector = Vector3.back * updownoffset;
                break;
            case PlayerController.TunnelDirection.Down:
                updownoffsetvector = Vector3.forward * updownoffset;
                break;
            case PlayerController.TunnelDirection.DownRight:
                updownoffsetvector = Vector3.forward * updownoffset;
                break;
            case PlayerController.TunnelDirection.DownLeft:
                updownoffsetvector = Vector3.forward * updownoffset;
                break;
            case PlayerController.TunnelDirection.Left:
                updownoffsetvector = Vector3.up * updownoffset;
                break;
            case PlayerController.TunnelDirection.Right:
                updownoffsetvector = Vector3.up * updownoffset;
                break;
            case PlayerController.TunnelDirection.DoNothing:
                break;
        }

        moveCamTo = transform.position - transform.forward * distance + updownoffsetvector;

        // Old Asymptotic Average function - creates heavy stuttering
        // float bias = 0.95f;
        // MainCamera.transform.position = MainCamera.transform.position * bias +moveCamTo * (1.0f - bias);

        // Placeholder Smooth-position change for camera
        MainCamera.transform.position = Vector3.SmoothDamp(MainCamera.transform.position, moveCamTo, ref _velocity, 0.5f);
        MainCamera.transform.rotation = Player.transform.rotation;
    }
}