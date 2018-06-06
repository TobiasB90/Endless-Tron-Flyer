using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Insert PlayerModel Object.")] public GameObject PlayerModel;

    [Tooltip("Flyingspeed of the Player")] public float FlyingSpeed;
    [Tooltip("The speed at we can move our Player up/down/left/right")] public float RotationSpeed;
    [Tooltip("The speed at the player's rotation gets adjusted depending on the current tunneldirection")] public float AdjustPlayerRotationToTunnelDirectionSpeed;

    private float rotationHorz = 0;
    [Tooltip("Horizontal rotationspeed (only visuals)")] public float HorzRotaSpeed = 15;
    [Tooltip("Maximum horizontal rotation")] public float maxHorzRota = 90;
    [Tooltip("Minimum horizontal rotation")] public float minHorzRota = -90;

    public enum TunnelDirection { Forward, ForwardLeft, ForwardRight, ForwardRotated, Back, Up, UpRotated, Down, DownRotated, Left, LeftRotated, Right, RightRotated, UpRight, UpLeft, RightUp, LeftUp, DownRight, DownLeft, RightDown, LeftDown, NoRotation };
    public TunnelDirection TunnelDir;

    private Quaternion OriginalPlayerModelRotation;

    private void Start()
    {
        OriginalPlayerModelRotation = PlayerModel.transform.rotation;
    }

    private void Update()
    {
        // Multiply every rotational oder positional changingspeed with Time.deltaTime to avoid different results at different framerates
        float AdjustPlayerRotationToTunnelDirectionSpeedDelta = AdjustPlayerRotationToTunnelDirectionSpeed * Time.deltaTime;
        float HorzRotaSpeedDelta = HorzRotaSpeed * Time.deltaTime;
        float rotationspeedDelta = RotationSpeed * Time.deltaTime;
        float FlyingSpeedDelta = FlyingSpeed * Time.deltaTime;

        // Player always moving forward at flyingspeed * Time.deltaTime
        transform.position += transform.forward * FlyingSpeedDelta;

        // Player rotation gets adjusted depending on the current TunnelDirection (known problem: no adjustments during horizontal or vertical inputs)
        switch (TunnelDir)
        {
            case TunnelDirection.Forward:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.ForwardRotated:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.ForwardLeft:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.ForwardRight:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.Up:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.y), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.UpRotated:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.y + 180), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.LeftUp:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.y - 90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.RightUp:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.y + 90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;            
            case TunnelDirection.Right:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.RightRotated:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.UpRight:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.DownRight:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.Left:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.LeftRotated:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.UpLeft:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.DownLeft:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.Down:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.DownRotated:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y + 180), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.LeftDown:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y + 90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.RightDown:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y - 90), AdjustPlayerRotationToTunnelDirectionSpeedDelta);
                break;
            case TunnelDirection.NoRotation:
                break;



        }

        // If we have any movement Input:
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            // Rotate the Player depending on input and rotationspeed, impact on the flyingdirection
            transform.Rotate(-Input.GetAxis("Vertical") * rotationspeedDelta, Input.GetAxis("Horizontal") * rotationspeedDelta, 0, Space.Self);

            // Rotate the PlayerModel on horizontal input (Left & Right) just for visuals, no impact on the flying direction
            rotationHorz += Input.GetAxis("Horizontal") * HorzRotaSpeedDelta;
            rotationHorz = Mathf.Clamp(rotationHorz, minHorzRota, maxHorzRota);
            var HorzQuaternion = Quaternion.AngleAxis(rotationHorz, Vector3.back);
            PlayerModel.transform.localRotation = OriginalPlayerModelRotation * HorzQuaternion;

        }

        // If we dont have any horizontal (left & right) inputs
        if (Input.GetAxis("Horizontal") == 0)
        {
            // Rotate the PlayerModel back to default rotation (should be '0')
            PlayerModel.transform.localRotation = Quaternion.Lerp(PlayerModel.transform.localRotation, Quaternion.Euler(PlayerModel.transform.localRotation.x, PlayerModel.transform.localRotation.y, 0), HorzRotaSpeedDelta/75);
            rotationHorz = Mathf.Lerp(rotationHorz, 0, HorzRotaSpeedDelta/75);
        }
    }
}