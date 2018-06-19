using UnityEngine;
using TMPro;

public class Options_Controls : MonoBehaviour {

    userManager usrMng;

    public TMP_Dropdown Invertedmovement_Dropdown;

	// Use this for initialization
	void Start () {
        usrMng = GameObject.Find("_userManager").GetComponent<userManager>();
        if (usrMng.invertedmovement == true)
        {
            Invertedmovement_Dropdown.value = 1;
        }
        else Invertedmovement_Dropdown.value = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplyChanges()
    {
        if (Invertedmovement_Dropdown.value == 0)
        {
            usrMng.invertedmovement = false;
        }
        else if (Invertedmovement_Dropdown.value == 1)
        {
            usrMng.invertedmovement = true;
        }
    }
}
