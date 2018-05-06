using UnityEngine;
using System.Collections;

public class AlternatePlayerController : MonoBehaviour
{
    public GameObject PlayerModel;
    // var axes = RotationAxes.MouseXAndY;
    public float flyingspeed;
    public float rotationspeed;
    
    public float sensitivityX = 15;
    public float sensitivityY = 15;

    public float minimumX = -360;
    public float maximumX = 360;

    public float minimumY = -60;
    public float maximumY = 60;

    public float rotationX = 0;
    public float rotationY = 0;

    private Quaternion originalRotation;
    private Quaternion originalPlayerModelRotation;

    public void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            rotationX += Input.GetAxis("Horizontal") * sensitivityX;
            rotationY += Input.GetAxis("Vertical") * sensitivityY;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            var xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.back);
            var yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            PlayerModel.transform.localRotation = originalRotation * yQuaternion * xQuaternion;
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationspeed, 0);
        }
        else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            PlayerModel.transform.localRotation = Quaternion.Lerp(PlayerModel.transform.localRotation, originalPlayerModelRotation, Time.deltaTime * 1f);
            rotationX = Mathf.Lerp(rotationX, 0, sensitivityX * Time.deltaTime);
            rotationY = Mathf.Lerp(rotationY, 0, sensitivityY * Time.deltaTime);
        }
        float step = flyingspeed * Time.deltaTime;
        transform.position += PlayerModel.transform.forward * step;
    }

    public void Start()
    {
        originalRotation = transform.localRotation;
        originalPlayerModelRotation = PlayerModel.transform.rotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
