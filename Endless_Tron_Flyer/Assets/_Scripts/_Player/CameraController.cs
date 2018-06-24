using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera MainCamera;
    public float camdistance;
    [HideInInspector] public float basecamdistance;
    public float increasingspeed_maxdistance;
    public float TargetCamToPlayerDistance;
    public float camupoffset;
    public float smoothtime;
    userManager usrMng;

    private void Start()
    {
        usrMng = GameObject.Find("_userManager").GetComponent<userManager>();
        basecamdistance = camdistance;

        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
    }

    void FixedUpdate()
    {
        switch (usrMng.CameraDistance)
        {
            case userManager.Option_CameraDistance.Close:
                TargetCamToPlayerDistance = usrMng.PlayerViewOptions[0].CamDistance;
                break;
            case userManager.Option_CameraDistance.Medium:
                TargetCamToPlayerDistance = usrMng.PlayerViewOptions[1].CamDistance;
                break;
            case userManager.Option_CameraDistance.Far:
                TargetCamToPlayerDistance = usrMng.PlayerViewOptions[2].CamDistance;
                break;
        }

        Vector3 moveCamTo = transform.position - transform.forward * camdistance + camupoffset * transform.up;

        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, moveCamTo, smoothtime);

        MainCamera.transform.localRotation = transform.localRotation;
    }
}