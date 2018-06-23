using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Options_Gameplay : MonoBehaviour {

    userManager usrMng;
    public TMP_Dropdown CameraDistance_Dropdown;
    public TMP_Text CameraDistance_Label;

    // Use this for initialization
    void Start () {
        usrMng = GameObject.Find("_userManager").GetComponent<userManager>();
        int camdistvalue = PlayerPrefs.GetInt("CameraDistance");

        if (camdistvalue == 0)
        {
            CameraDistance_Dropdown.value = 0;
            CameraDistance_Label.text = CameraDistance_Dropdown.options[0].text;
        }
        else if (camdistvalue == 1)
        {
            CameraDistance_Dropdown.value = 1;
            CameraDistance_Label.text = CameraDistance_Dropdown.options[1].text;
        }
        else if (camdistvalue == 2)
        {
            CameraDistance_Dropdown.value = 2;
            CameraDistance_Label.text = CameraDistance_Dropdown.options[2].text;
        }
    }

    public void ApplyChanges()
    {
        ChangeCameraViewDistance();
    }

    public void ChangeCameraViewDistance()
    {
        if(CameraDistance_Dropdown.value == 0)
        {
            usrMng.CameraDistance = userManager.Option_CameraDistance.Close;
            PlayerPrefs.SetInt("CameraDistance", 0);
        }
        else if (CameraDistance_Dropdown.value == 1)
        {
            usrMng.CameraDistance = userManager.Option_CameraDistance.Medium;
            PlayerPrefs.SetInt("CameraDistance", 1);
        }
        else if (CameraDistance_Dropdown.value == 2)
        {
            usrMng.CameraDistance = userManager.Option_CameraDistance.Far;
            PlayerPrefs.SetInt("CameraDistance", 2);
        }
    }
}
