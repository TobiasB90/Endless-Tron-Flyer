using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class Options_Video : MonoBehaviour {

    public TMP_Dropdown DisplayMode_Dropdown;
    // public TMP_Dropdown Displays_Dropdown;
    // public TMP_Text DisplaysLabel;
    public TMP_Text ResLabel;
    public bool resolutionchanged = false;
    // Get Available Resolutions
    Sprite image;
    public TMP_Dropdown Resolution_Dropdown;
    public List<Resolutionobj> ResolutionList = new List<Resolutionobj>();

    public class Resolutionobj
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    private void Update()
    {
    }

    void Start()
    {
        UpdateResolutionDropdown();
        // GetAvailableDisplays();
    }

    public void ApplyChanges()
    {
        resolutionchanged = false;
        StartCoroutine(ChangeDisplaySettings());
    }

    public void UpdateResolutionDropdown()
    {
        // Get Available Resolutions
        Resolution[] resolutions = Screen.resolutions;
        Resolution_Dropdown.options.Clear();
        ResolutionList.Clear();
        // Resolution_Dropdown.options.Capacity = resolutions.Length;

        Resolutionobj recentres = new Resolutionobj();

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolutionobj res = new Resolutionobj();
            res.width = resolutions[i].width;
            res.height = resolutions[i].height;

            if (recentres != null && recentres.width != res.width && recentres.height != res.height)
            {
                ResolutionList.Add(res);
                string resstring = res.width.ToString() + " X " + res.height.ToString();
                var data = new TMP_Dropdown.OptionData(resstring, image);
                Resolution_Dropdown.options.Add(data);
            }

            recentres.width = resolutions[i].width;
            recentres.height = resolutions[i].height;
        }
        Debug.Log(Resolution_Dropdown.options.Count);
        for(int i = 0; i < Resolution_Dropdown.options.Count; i++)
        {
            Debug.Log(Resolution_Dropdown.options[i].text);
            string currentresolution = Screen.width + " X " + Screen.height;
            if (Resolution_Dropdown.options[i].text == currentresolution)
            {
                Resolution_Dropdown.options.RemoveAt(i);
                ResolutionList.RemoveAt(i);
                Debug.Log("REMOVED CURRENT RES");
                break;
            }
        }

        string currentres = Screen.width + " X " + Screen.height;
        var currentdata = new TMP_Dropdown.OptionData(currentres, image);
        Resolution_Dropdown.options.Add(currentdata);

        Resolutionobj currentresobj = new Resolutionobj();
        currentresobj.width = Screen.width;
        currentresobj.height = Screen.height;
        ResolutionList.Add(currentresobj);

        ResolutionList.Reverse();
        Resolution_Dropdown.options.Reverse();
        ResLabel.text = Screen.width + " X " + Screen.height;
        Resolution_Dropdown.value = 0;
        
    }

    public IEnumerator ChangeDisplaySettings()
    {
        if (DisplayMode_Dropdown.value == 0)
        {
            Screen.SetResolution(ResolutionList[Resolution_Dropdown.value].width, ResolutionList[Resolution_Dropdown.value].height, true);
            Debug.Log(ResolutionList[Resolution_Dropdown.value].width + " X " + ResolutionList[Resolution_Dropdown.value].height + " in " + Screen.fullScreenMode);
        }
        else if (DisplayMode_Dropdown.value == 1)
        {
            Screen.SetResolution(ResolutionList[Resolution_Dropdown.value].width, ResolutionList[Resolution_Dropdown.value].height, false);
            Debug.Log(ResolutionList[Resolution_Dropdown.value].width + " X " + ResolutionList[Resolution_Dropdown.value].height + " in " + Screen.fullScreenMode);
        }
        while (Screen.width != ResolutionList[Resolution_Dropdown.value].width && Screen.height != ResolutionList[Resolution_Dropdown.value].height)
        {
            if (Screen.width != ResolutionList[Resolution_Dropdown.value].width && Screen.height != ResolutionList[Resolution_Dropdown.value].height) resolutionchanged = true;
            yield return new WaitUntil(() => resolutionchanged == true);
        }
        UpdateResolutionDropdown();
    }

    //public void GetAvailableDisplays()
    //{
    //    Displays_Dropdown.options.Clear();
    //    Displays_List.Clear();
    //    Debug.Log(Display.displays[0].ToString());
    //    for (int i = 0; i < Display.displays.Length; i++)
    //    {
    //        Displays_List.Add(Display.displays[i].ToString());
    //        if(i == 0)
    //        {
    //            string displaystringmain = "DISPLAY 1 (MAIN)";
    //            var datamain = new TMP_Dropdown.OptionData(displaystringmain, image);
    //            Displays_Dropdown.options.Add(datamain);
    //        }
    //        else
    //        {
    //            int i2 = i + 1;
    //            string displaystring = "DISPLAY " + i2;
    //            var data = new TMP_Dropdown.OptionData(displaystring, image);
    //            Displays_Dropdown.options.Add(data);
    //        }
    //        if (Display.displays[i].active)
    //        {
    //            int i2 = i + 1;
    //            if(Display.displays[i].Equals(Display.main)) DisplaysLabel.text = "DISPLAY 1 (MAIN)";
    //            else DisplaysLabel.text = "DISPLAY " + i2;
    //        }
    //        Displays_Dropdown.value = i;
    //    }
    //}

}
