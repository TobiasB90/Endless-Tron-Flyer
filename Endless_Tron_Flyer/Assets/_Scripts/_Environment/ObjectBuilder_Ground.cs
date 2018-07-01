using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder_Ground : MonoBehaviour {

    public GameObject GroundPrefab;
    public GameObject Environment;
    public GameObject Player;
    public float OffsetZ;
    public float InstantiateThreshold_Distance;

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update () {
        if(Player != null)
        {
            if (Player.transform.position.z + InstantiateThreshold_Distance >= transform.position.z) InstantiateGround();
        }
    }

    public void InstantiateGround()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + OffsetZ);
        GameObject Instantiated_Tunnel = Instantiate(GroundPrefab, transform.position, transform.rotation);
        Instantiated_Tunnel.transform.parent = Environment.transform;
    }
}
