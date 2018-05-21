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
    public float sensitivityZ = 15;

    public float minimumX = -360;
    public float maximumX = 360;

    public float minimumY = -60;
    public float maximumY = 60;

    public float minimumZ = -60;
    public float maximumZ = 60;

    public float rotationX = 0;
    public float rotationY = 0;
    public float rotationZ = 0;

    private Quaternion originalRotation;
    private Quaternion originalPlayerModelRotation;

    public void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            rotationX += Input.GetAxis("Horizontal") * sensitivityX * Time.deltaTime;
            rotationY += Input.GetAxis("Vertical") * sensitivityY * Time.deltaTime;
            rotationZ += Input.GetAxis("Horizontal") * sensitivityZ * Time.deltaTime;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            rotationZ = ClampAngle(rotationZ, minimumZ, maximumZ);

            var xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.left);
            var yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.up);
            var zQuaternion = Quaternion.AngleAxis(rotationZ, Vector3.back);


            transform.rotation = originalRotation * yQuaternion * xQuaternion;
            PlayerModel.transform.localRotation = originalPlayerModelRotation * zQuaternion;
        }
        if (Input.GetKey(KeyCode.Q))
        {

        }
        //else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        //{
        //    PlayerModel.transform.localRotation = Quaternion.Lerp(PlayerModel.transform.localRotation, originalPlayerModelRotation, Time.deltaTime * 1f);
        //    rotationX = Mathf.Lerp(rotationX, 0, sensitivityX * Time.deltaTime);
        //    rotationY = Mathf.Lerp(rotationY, 0, sensitivityY * Time.deltaTime);
        //}
        //else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        //{
        //    PlayerModel.transform.localRotation = Quaternion.Lerp(PlayerModel.transform.localRotation, Quaternion.Euler(PlayerModel.transform.localRotation.x, PlayerModel.transform.localRotation.y, 0), Time.deltaTime * 1f);
        //    rotationX = Mathf.Lerp(rotationX, 0, sensitivityX * Time.deltaTime);
        //    rotationY = Mathf.Lerp(rotationY, 0, sensitivityY * Time.deltaTime);
        //}
        float step = flyingspeed * Time.deltaTime;
        transform.position += PlayerModel.transform.forward * step;
    }

    public void Start()
    {
        originalRotation = transform.localRotation;
        originalPlayerModelRotation = PlayerModel.transform.localRotation;
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
