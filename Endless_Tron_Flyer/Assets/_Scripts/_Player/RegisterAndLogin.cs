using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Security;

public class RegisterAndLogin : MonoBehaviour {
    // {"Name":"asda","Password":"asdsdfsdf","Email":"asdfa@sdfsdf.net"}

    public TMP_InputField RegisterUsername;
    public TMP_InputField RegisterPassword;
    public TMP_InputField RegisterEMail;

    public TMP_InputField LoginUsername;
    public TMP_InputField LoginPassword;
    public TMP_InputField LoginEMail;

    public userManager UserManager;

    // Use this for initialization
    void Start () {

	}

    public void LoginUser()
    {
        RegisterData logindata = new RegisterData();

        logindata.Name = LoginUsername.text;

        var securepw = new SecureString();
        foreach (char c in RegisterPassword.text)
        {
            securepw.AppendChar(c);
        }

        logindata.Password = LoginPassword.text;
        logindata.Email = LoginEMail.text;

        string outputJSON = JsonConvert.SerializeObject(logindata, Formatting.None);
        string output = Encrypt(outputJSON);
        Debug.Log(outputJSON);
        Debug.Log(output);

        try
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            byte[] pData = System.Text.Encoding.ASCII.GetBytes(output.ToCharArray());
            WWW api = new WWW("http://deergames.eu-central-1.elasticbeanstalk.com/api/users/login", pData, headers);
            StartCoroutine(WaitForWWW(api));
        }
        catch (UnityException ex) { Debug.Log(ex.Message); }
    }

    public void RegisterUser()
    {
        RegisterData regdata = new RegisterData();
        regdata.Name = RegisterUsername.text;

        var securepw = new SecureString();
        foreach (char c in RegisterPassword.text)
        {
            securepw.AppendChar(c);
        }

        regdata.Password = RegisterPassword.text;
        regdata.Email = RegisterEMail.text;

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
            StartCoroutine(WaitForWWW(api));
        }
        catch (UnityException ex) { Debug.Log(ex.Message); }
    }

    IEnumerator WaitForWWW(WWW www)
    {
        yield return www;
        Debug.Log(getResponseCode(www));
        Debug.Log(getResponseAuth(www));

        if (getResponseAuth(www) != null)
        {
            UserManager.sToken = getResponseAuth(www);
        }

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.text);
        }
        else
            Debug.Log("ERROR");
    }

    // Update is called once per frame
    void Update () {
		
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

}
