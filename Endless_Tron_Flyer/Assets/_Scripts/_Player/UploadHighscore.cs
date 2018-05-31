using UnityEngine;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

public class UploadHighscore : MonoBehaviour {

    public string userName;
    public float highScore;
    HighScoreData highscoredata;

    private void Start()
    {
        UploadPlayerHighScore();
        Debug.Log(PlayerPrefs.GetString("Username"));
    }

    public void UploadPlayerHighScore()
    {
        highScore = PlayerPrefs.GetFloat("HighScore");
        userName = PlayerPrefs.GetString("Username");
        CreateToken();
    }

    private void CreateToken()
    {
        var payload = new Dictionary<string, object>
        {
            { "Username", userName },
            { "Highscore", highScore }
        };
        const string secret = "deergames1337";

        IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        var token = encoder.Encode(payload, secret);

        highscoredata = new HighScoreData(userName, highScore, token);
        var highscoredata_json = JsonUtility.ToJson(highscoredata, true);

        Debug.Log(highscoredata_json);

        SendData();
    }

    private void SendData()
    {

    }
}

public class HighScoreData
{
    public string Username;
    public float Highscore;
    public string Token;

    public HighScoreData(string userName, float highScore, string token)
    {
        Username = userName;
        Highscore = highScore;
        Token = token;
    }
}


