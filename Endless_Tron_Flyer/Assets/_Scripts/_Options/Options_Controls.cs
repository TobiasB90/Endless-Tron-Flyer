using UnityEngine;
using TMPro;

public class Options_Controls : MonoBehaviour {

    userManager usrMng;

    public TMP_Dropdown Invertedmovement_Dropdown;

	// Use this for initialization
	void Start () {
        usrMng = GameObject.Find("_userManager").GetComponent<userManager>();
        if (PlayerPrefs.GetInt("InvertedMovement") == 0)
        {
            Invertedmovement_Dropdown.value = 0;
        }
        else Invertedmovement_Dropdown.value = 1;
    }

    public void ApplyChanges()
    {
        if (Invertedmovement_Dropdown.value == 0)
        {
            PlayerPrefs.SetInt("InvertedMovement", 0);
            usrMng.invertedmovement = false;
        }
        else if (Invertedmovement_Dropdown.value == 1)
        {
            PlayerPrefs.SetInt("InvertedMovement", 1);
            usrMng.invertedmovement = true;
        }
    }
}
