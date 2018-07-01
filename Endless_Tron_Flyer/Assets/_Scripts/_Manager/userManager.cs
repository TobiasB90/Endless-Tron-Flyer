using System;
using UnityEngine;

public class userManager : MonoBehaviour {

    public string Username;
    public string sToken;
    [NonSerialized] public CharViewList[] PlayerViewOptions = new CharViewList[3];

    public enum Option_CameraDistance { Close, Medium, Far };
    public Option_CameraDistance CameraDistance;

    public bool FirstGamesceneStart = true;    

    public bool invertedmovement = false;

    public void Start()
    {
        PlayerViewOptions[0] = new CharViewList();
        PlayerViewOptions[0].Name = "CLOSE";
        PlayerViewOptions[0].CamDistance = 10;

        PlayerViewOptions[1] = new CharViewList();
        PlayerViewOptions[1].Name = "MEDIUM";
        PlayerViewOptions[1].CamDistance = 15;

        PlayerViewOptions[2] = new CharViewList();
        PlayerViewOptions[2].Name = "FAR";
        PlayerViewOptions[2].CamDistance = 20;

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
    public class CharViewList
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
