using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera MainCamera;
    public bool RotationWithCam;
    public Transform target;
    public Vector3 offset;
    public float pitch = 2f;
    public float currentYaw = 0f;
    Quaternion baseRota;
    public float multiplier;


    private void Start()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
        baseRota = MainCamera.transform.rotation;
    }

    void LateUpdate()
    {
        MainCamera.transform.position = target.position - offset;
        MainCamera.transform.LookAt(target.position + Vector3.up * pitch);
        MainCamera.transform.RotateAround(target.position, Vector3.up, currentYaw);
        if (RotationWithCam)
        {
            MainCamera.transform.rotation = Quaternion.Euler(baseRota.x, baseRota.y, target.rotation.z * multiplier);
            // MainCamera.transform.rotation = new Quaternion(baseRota.x, baseRota.y, target.rotation.z, baseRota.w);

        }
    }
}