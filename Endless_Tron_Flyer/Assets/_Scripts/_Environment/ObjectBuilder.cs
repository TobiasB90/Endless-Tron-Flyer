using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour {

    public GameObject[] TunnelSystems;
    public GameObject Environment;
    public GameObject FlyingObject;
    public GameObject _EndlessObjs;
    public int LastTunnel = 0;
    public int NextTunnel = 0;
    public float TunnelLength = 900;
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
