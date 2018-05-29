using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SetResolution : MonoBehaviour {

    TMP_Dropdown dropdown;
    List<TMP_Dropdown.OptionData> odata = new List<TMP_Dropdown.OptionData>();
    List<string> resstrings = new List<string>();
    Sprite image;

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.options.Clear();
        dropdown.options.Capacity = resolutions.Length;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resstrings.Add(resolutions[i].width + " X " + resolutions[i].height);
            var data = new TMP_Dropdown.OptionData(resstrings[i], image);
            dropdown.options.Add(data);
        }
        dropdown.options.Reverse();
        // Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
    }
}
