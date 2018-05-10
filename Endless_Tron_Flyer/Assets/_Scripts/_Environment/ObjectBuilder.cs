using System.Collections;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour {

    [Tooltip("Insert TunnelSystems with ObjectInformation.cs script attached to it.")][SerializeField] private GameObject[] TunnelSystems;
    [Tooltip("Insert the 'Environment' GameObject from the Scene.")] [SerializeField] private GameObject Environment;
    [Tooltip("Insert the 'FlyingObject' (Player) from the Scene.")] [SerializeField] private GameObject FlyingObject;
    [Tooltip("Insert the parent from this object from the Scene.")] [SerializeField] private GameObject _EndlessObjs;
    private int LastTunnel = 0;
    private int NextTunnel = 0;

    [Tooltip("How many 'TunnelSystems' should be built in advance at the start of the game?")] [SerializeField] private int TunnelInAdvance = 0;
    private float TunnelLength = 900;
    public float timesbuilt = 0;

    // Use this for initialization
    void Start () {
        StartCoroutine(spawnshit(1));
    }

    // Update is called once per frame
    void Update () {
        if (FlyingObject.transform.position.z >= timesbuilt * TunnelLength)
        {
            StartCoroutine(spawnshit(1));
        }
        _EndlessObjs.transform.position = new Vector3(0, 0, FlyingObject.transform.position.z);
    }

    IEnumerator spawnshit(float waittime)
    {
        
        GameObject NewTunnel = Instantiate(TunnelSystems[NextTunnel], transform.position, transform.rotation);
        LastTunnel = NextTunnel;
        NewTunnel.transform.parent = Environment.transform;
        timesbuilt += 1;
        while (NextTunnel == LastTunnel){
            NextTunnel = Random.Range(0, TunnelSystems.Length);
        }
        yield return new WaitForSeconds(waittime);
    }
}
