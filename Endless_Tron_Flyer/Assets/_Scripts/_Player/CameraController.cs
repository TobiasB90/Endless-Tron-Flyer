using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera MainCamera;
    public float camdistance;
    public float camupoffset;
    public float smoothtime;

    // Old:
    // PlayerController pcont;
    // private int updownoffsetvector;

    private void Start()
    {
        // Old:
        // pcont = Player.GetComponent<PlayerController>();
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        Vector3 moveCamTo = transform.position - transform.forward * camdistance + camupoffset * transform.up;
        float smoothtimedelta = smoothtime * Time.deltaTime;
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, moveCamTo, smoothtimedelta);
        MainCamera.transform.localRotation = transform.localRotation;

        // Old: Change updownoffset based on current tunneldirection (WorldSpace)

        //switch (pcont.TunnelDir)
        //{
        //    case PlayerController.TunnelDirection.Forward:
        //        updownoffsetvector = transform.up * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.ForwardRotated:
        //        updownoffsetvector = Vector3.down * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.ForwardLeft:
        //        updownoffsetvector = Vector3.right * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.ForwardRight:
        //        updownoffsetvector = Vector3.left * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.Up:
        //        updownoffsetvector = Vector3.back * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.UpRotated:
        //        updownoffsetvector = Vector3.forward * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.LeftUp:
        //        updownoffsetvector = Vector3.right * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.RightUp:
        //        updownoffsetvector = Vector3.left * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.Right:
        //        updownoffsetvector = Vector3.up * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.RightRotated:
        //        updownoffsetvector = Vector3.down * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.UpRight:
        //        updownoffsetvector = Vector3.back * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.DownRight:
        //        updownoffsetvector = Vector3.forward * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.Left:
        //        updownoffsetvector = Vector3.up * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.LeftRotated:
        //        updownoffsetvector = Vector3.down * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.UpLeft:
        //        updownoffsetvector = Vector3.back * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.DownLeft:
        //        updownoffsetvector = Vector3.forward * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.Down:
        //        updownoffsetvector = Vector3.forward * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.DownRotated:
        //        updownoffsetvector = Vector3.back * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.LeftDown:
        //        updownoffsetvector = Vector3.left * updownoffset;
        //        break;
        //    case PlayerController.TunnelDirection.RightDown:
        //        updownoffsetvector = Vector3.right * updownoffset;
        //        break;
        //}
    }
}