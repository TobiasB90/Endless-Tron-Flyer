using System;
using UnityEngine;

public class userManager : MonoBehaviour {

    public string Username;
    public string sToken;
    public CharViewList[] PlayerViewOptions;

    public enum Option_CameraDistance { Close, Medium, Far };
    public Option_CameraDistance CameraDistance;
    

    public bool invertedmovement = false;

    public void Start()
    {
        if (PlayerPrefs.GetInt("InvertedMovement") == 0) invertedmovement = false;
        else if (PlayerPrefs.GetInt("InvertedMovement") == 1) invertedmovement = true;

        int camdistvalue = PlayerPrefs.GetInt("CameraDistance");

        if (camdistvalue == 0)
        {
            CameraDistance = Option_CameraDistance.Close;
        }
        else if (camdistvalue == 1)
        {
            CameraDistance = Option_CameraDistance.Medium;
        }
        else if (camdistvalue == 2)
        {
            CameraDistance = Option_CameraDistance.Far;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    [Serializable]
    public struct CharViewList
    {
        public string Name;
        public float CamDistance;
    }

    public void ToggleCameraView()
    {
        switch (CameraDistance)
        {
            case Option_CameraDistance.Close:
                CameraDistance = Option_CameraDistance.Medium;
                break;
            case Option_CameraDistance.Medium:
                CameraDistance = Option_CameraDistance.Far;
                break;
            case Option_CameraDistance.Far:
                CameraDistance = Option_CameraDistance.Close;
                break;
        }
    }
}
