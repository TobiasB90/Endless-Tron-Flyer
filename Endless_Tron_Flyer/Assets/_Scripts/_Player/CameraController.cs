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
        Vector3 moveCamTo = target.position - transform.forward * 10f + Vector3.up * 2.0f;
        float bias = 0.96f;
        MainCamera.transform.position = MainCamera.transform.position * bias + moveCamTo * (1.0f-bias);
        MainCamera.transform.LookAt(target.position + target.forward * 5);
        // MainCamera.transform.RotateAround(target.position, Vector3.up, currentYaw);
        if (RotationWithCam)
        {
            MainCamera.transform.rotation = Quaternion.Euler(baseRota.x, baseRota.y, target.rotation.z * multiplier);
            // MainCamera.transform.rotation = new Quaternion(baseRota.x, baseRota.y, target.rotation.z, baseRota.w);

        }
    }
}