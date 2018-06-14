using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Security;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class RegisterAndLogin : MonoBehaviour {

    public TMP_InputField RegisterUsername;
    public TMP_InputField RegisterPassword;
    public TMP_InputField RegisterEMail;

    public TMP_InputField LoginUsername;
    public TMP_InputField LoginPassword;
    public TMP_InputField LoginEMail;

    public TMP_Text UIMessage;
    public Color UIMessage_FadeInColor;
    public Color UIMessage_FadeOutColor;

    private userManager UserManager;
    public string userName;

    private void Start()
    {
        UserManager = GameObject.Find("_userManager").GetComponent<userManager>();
    }

    public void LoginUser()
    {
        RegisterData logindata = new RegisterData();

        logindata.Name = LoginUsername.text;
        userName = LoginUsername.text;

        var securepw = new SecureString();
        foreach (char c in RegisterPassword.text)
        {
            securepw.AppendChar(c);
        }

        logindata.Password = LoginPassword.text;
        logindata.Email = LoginEMail.text;

        string outputJSON = JsonConvert.SerializeObject(logindata, Formatting.None);
        string output = Encrypt(outputJSON);

        try
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            byte[] pData = System.Text.Encoding.ASCII.GetBytes(output.ToCharArray());
            WWW api = new WWW("http://deergames.eu-central-1.elasticbeanstalk.com/api/users/login", pData, headers);
            StartCoroutine(WaitForLoginWWW(api));
        }
        catch (UnityException ex) { Debug.Log(ex.Message); }
    }

    public void RegisterUser()
    {
        RegisterData regdata = new RegisterData();
        regdata.Name = LoginUsername.text;

        var securepw = new SecureString();
        foreach (char c in LoginPassword.text)
        {
            securepw.AppendChar(c);
        }

        regdata.Password = LoginPassword.text;
        regdata.Email = LoginEMail.text;

        string outputJSON = JsonConvert.SerializeObject(regdata, Formatting.None);
        string output = Encrypt(outputJSON);
        Debug.Log(outputJSON);
        Debug.Log(output);

        try
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            byte[] pData = System.Text.Encoding.ASCII.GetBytes(output.ToCharArray());
            WWW api = new WWW("http://deergames.eu-central-1.elasticbeanstalk.com/api/users/sign-up", pData, headers);
            StartCoroutine(WaitForRegisterWWW(api));
        }
        catch (UnityException ex) { Debug.Log(ex.Message); }
    }

    IEnumerator WaitForRegisterWWW(WWW www)
    {
        yield return www;
        if (getResponseCode(www) == 200)
        {
            ShowUIMessage("Registration successful");
        }
        else ShowUIMessage("Unexpected Error: " + getResponseCode(www));
    }

    IEnumerator WaitForLoginWWW(WWW www)
    {
        yield return www;
        if (getResponseCode(www) == 200)
        {
            UserManager.sToken = getResponseAuth(www);
            UserManager.Username = userName;
            ShowUIMessage("Login succesful");
            SceneManager.LoadScene("MainMenu_01");
        }
        else ShowUIMessage("Unexpected Error: " + getResponseCode(www));
    }

    public class RegisterData
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class LoginData
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
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

    public static string getResponseAuth(WWW request)
    {
        string ret = "";
        if (request.responseHeaders == null)
        {
            Debug.LogError("no response headers.");
        }
        else
        {
            if (!request.responseHeaders.ContainsKey("AUTHORIZATION"))
            {
                Debug.LogError("response headers has no AUTHORIZATION.");
            }
            else
            {
                ret = request.responseHeaders["AUTHORIZATION"];
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

    public void ShowUIMessage(string uimessage)
    {
        UIMessage.text = uimessage;
        Sequence FadeInOut = DOTween.Sequence();
        FadeInOut.Append(UIMessage.DOColor(UIMessage_FadeInColor, 3f).SetEase(Ease.Linear));
        FadeInOut.Append(UIMessage.DOColor(UIMessage_FadeOutColor, 3f).SetEase(Ease.Linear));
    }

    public void StartOfflineMode()
    {
        SceneManager.LoadScene("MainMenu_01");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
