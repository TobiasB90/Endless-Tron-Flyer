using UnityEngine;

public class userManager : MonoBehaviour {

    public string Username;
    public string sToken;

    public bool invertedmovement = false;

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
