﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using TMPro;
using System;
using System.Security.Cryptography;
using System.Text;

public class UploadHighscore : MonoBehaviour
{
    public GameObject HighScoreOBJ_Parent;
    public GameObject HighScoreOBJ_Prefab;
    public int hScore;
    private string uploadHighscoreURL = "";
    HighScoreData highscoredata;
    public ScoreList scoreList;
    private userManager UserManager;
    public Scores PersonalScore = new Scores();

    private void Start()
    {
        UserManager = GameObject.Find("_userManager").GetComponent<userManager>();
        GetPersonalHighscore();
    }

    public void UploadPlayerHighScore(int highscore)
    {
        highscoredata = new HighScoreData();
        highscoredata.Highscore = highscore;

        string outputJSON = JsonConvert.SerializeObject(highscoredata, Formatting.None);
        string output = Encrypt(outputJSON);
        Debug.Log(output);

        SendHighscoreData(output);
    }

    IEnumerator WaitForWWW(WWW www)
    {
        yield return www;
        GetPersonalHighscore();
        Debug.Log(getResponseCode(www));
        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.text);
        }
        else
            Debug.Log("ERROR");
    }

    public void SendHighscoreData(string UploadData)
    {
        try
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", UserManager.sToken);
            byte[] pData = System.Text.Encoding.ASCII.GetBytes(UploadData.ToCharArray());
            WWW api = new WWW("http://deergames.eu-central-1.elasticbeanstalk.com/api/ranking/create", pData, headers);
            StartCoroutine(WaitForWWW(api));
        }
        catch (UnityException ex) { Debug.Log(ex.Message); }
    }

    public void GetHighscores()
    {
        StartCoroutine(GetHighscoreData());
    }

    public IEnumerator GetHighscoreData()
    {
        string url = "http://deergames.eu-central-1.elasticbeanstalk.com/api/ranking/get";
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(getResponseCode(www));
        if (www.error == null)
        {
            Debug.Log(www.text);
            scoreList = JsonConvert.DeserializeObject<ScoreList>(www.text);
            int childcount = HighScoreOBJ_Parent.transform.childCount;
            for(int i = 0; i < childcount; i++)
            {
                GameObject child = HighScoreOBJ_Parent.transform.GetChild(i).gameObject;
                Destroy(child);
            }
            foreach(Scores score in scoreList.Scores)
            {
                GameObject HighScoreOBJ = Instantiate(HighScoreOBJ_Prefab, HighScoreOBJ_Parent.transform, false);
                TMP_Text Rank = HighScoreOBJ.transform.GetChild(0).GetComponent<TMP_Text>();
                Rank.text = score.Rank.ToString();
                TMP_Text Name = HighScoreOBJ.transform.GetChild(1).GetComponent<TMP_Text>();
                Name.text = score.Name;
                TMP_Text Score = HighScoreOBJ.transform.GetChild(2).GetComponent<TMP_Text>();
                Score.text = score.Score.ToString();
            }
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }

    public void GetPersonalHighscore()
    {
        if (UserManager.Username == "") Debug.Log("OFFLINE MODE: No User Found");
        else
        {
            string url = "http://deergames.eu-central-1.elasticbeanstalk.com/api/ranking/get";
            WWWForm form = new WWWForm();
            Dictionary<string, string> headers = form.headers;
            form.AddField("name", UserManager.Username);
            byte[] rawFormData = form.data;
            WWW request = new WWW(url, rawFormData, headers);
            StartCoroutine(GetPersonalHighscoreResponse(request));
        }
    }

    public IEnumerator GetPersonalHighscoreResponse(WWW www)
    {
        yield return www;
        if(www.text != "null") PersonalScore = JsonConvert.DeserializeObject<Scores>(www.text);
        if(www.text == "null")
        {
            PersonalScore.Name = UserManager.Username;
            PersonalScore.Rank = 0;
            PersonalScore.Score = 0;
        }
    }

    public class HighScoreData
    {
        public int Highscore { get; set; }
    }

    public class ScoreList
    {
        public IList<Scores> Scores { get; set; }
    }

    public class Scores
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
    }

    private static void Main(string[] args)
    {
        string encrypted = Encrypt("Something to decrypt");
        Console.Out.WriteLine(encrypted);

        string decrypted = Decrypt(encrypted);
        Console.Out.WriteLine(decrypted);

        Console.Out.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    private static string Encrypt(string raw)
    {
        using (var csp = new AesCryptoServiceProvider())
        {
            ICryptoTransform e = GetCryptoTransform(csp, true);
            byte[] inputBuffer = Encoding.UTF8.GetBytes(raw);
            byte[] output = e.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string encrypted = Convert.ToBase64String(output);

            return encrypted;
        }
    }

    public static string Decrypt(string encrypted)
    {
        using (var csp = new AesCryptoServiceProvider())
        {
            var d = GetCryptoTransform(csp, false);
            byte[] output = Convert.FromBase64String(encrypted);
            byte[] decryptedOutput = d.TransformFinalBlock(output, 0, output.Length);

            string decypted = Encoding.UTF8.GetString(decryptedOutput);
            return decypted;
        }
    }

    private static ICryptoTransform GetCryptoTransform(AesCryptoServiceProvider csp, bool encrypting)
    {
        csp.Mode = CipherMode.CBC;
        csp.Padding = PaddingMode.PKCS7;
        var passWord = "D33rG@mes1ee7";
        var salt = "DasIstEinS4lt";

        //a random Init. Vector. just for testing
        String iv = "e675f725e675f725";

        var spec = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(passWord), Encoding.UTF8.GetBytes(salt), 65536);
        byte[] key = spec.GetBytes(16);


        csp.IV = Encoding.UTF8.GetBytes(iv);
        csp.Key = key;
        if (encrypting)
        {
            return csp.CreateEncryptor();
        }
        return csp.CreateDecryptor();
    }

    public static int getResponseCode(WWW request)
    {
        int ret = 0;
        if (request.responseHeaders == null)
        {
            Debug.LogError("no response headers.");
        }
        else
        {
            if (!request.responseHeaders.ContainsKey("STATUS"))
            {
                Debug.LogError("response headers has no STATUS.");
            }
            else
            {
                ret = parseResponseCode(request.responseHeaders["STATUS"]);
            }
        }

        return ret;
    }

    public static int parseResponseCode(string statusLine)
    {
        int ret = 0;

        string[] components = statusLine.Split(' ');
        if (components.Length < 3)
        {
            Debug.LogError("invalid response status: " + statusLine);
        }
        else
        {
            if (!int.TryParse(components[1], out ret))
            {
                Debug.LogError("invalid response code: " + components[1]);
            }
        }

        return ret;
    }

}
