using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float flyingspeed;
    public float rotationspeed;
    public bool invertedmovement = true;
    public GameObject PlayerModel;
    public enum TunnelDirection { Forward, Up, Down, Left, Right, UpRight, UpLeft, DownRight, DownLeft, DoNothing };
    public TunnelDirection TunnelDir;

    // Use this for initialization
    void Start()
    {

    }

    private void Update()
    {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.Rotate(-Input.GetAxis("Vertical") * rotationspeed * Time.deltaTime, Input.GetAxis("Horizontal") * rotationspeed * Time.deltaTime, 0, Space.Self);
            PlayerModel.transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotationspeed * Time.deltaTime, Space.Self);

            if (Input.GetKey(KeyCode.Q))
            {
                PlayerModel.transform.Rotate(0, 0, rotationspeed * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.E))
            {
                PlayerModel.transform.Rotate(0, 0, -rotationspeed * Time.deltaTime, Space.Self);
            }
            // transform.Rotate(Input.GetAxis("Vertical") * rotationspeed /2 * Time.deltaTime, 0.0f, -Input.GetAxis("Horizontal") * rotationspeed * Time.deltaTime, Space.Self);
        }

        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                PlayerModel.transform.Rotate(0, 0, rotationspeed /2 * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.E))
            {
                PlayerModel.transform.Rotate(0, 0, -rotationspeed /2 * Time.deltaTime, Space.Self);
            }
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            PlayerModel.transform.localRotation = Quaternion.Lerp(PlayerModel.transform.localRotation, Quaternion.Euler(PlayerModel.transform.localRotation.x, PlayerModel.transform.localRotation.y, 0), Time.deltaTime * rotationspeed/50);
        }

        Debug.Log(transform.eulerAngles.z);
        switch (TunnelDir)
        {
            case TunnelDirection.Forward:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.Up:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.y), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.UpRight:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.UpLeft:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.Down:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.DownRight:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.DownLeft:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.Left:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.Right:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotationspeed / 10);
                break;
            case TunnelDirection.DoNothing:
                break;
        }

        float step = flyingspeed * Time.deltaTime;
        transform.position += transform.forward * step;
    }
}