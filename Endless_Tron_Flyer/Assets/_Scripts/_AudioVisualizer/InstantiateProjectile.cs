using System.Collections.Generic;
using UnityEngine;

public class InstantiateProjectile : MonoBehaviour {

    public GameObject TargetPoint;
    public GameObject Projectile;
    public float Speed;
    public float SpawnDelay;
    public float rotationSpeed;
    public bool SpeedToMusic;
    GameObject[] mySpawns;
    GameObject SpawnPoint;
    private List<GameObject> ActiveProjs;
    float step;

    // Use this for initialization
    void Start () {
        SpawnPoint = this.gameObject;
        InvokeRepeating("CastProj", 2, SpawnDelay);
        ActiveProjs = new List<GameObject>();
    }
    

    void Update()
    {
        if (SpeedToMusic)
        {
            step = Speed * AudioPeer._Amplitude * Time.deltaTime;
        }
        else
        {
            step = Speed * Time.deltaTime;
        }

        float step2 = rotationSpeed * Time.deltaTime;        

        for (int i = 0; i < ActiveProjs.Count; i++)
        {
            var proj = ActiveProjs[i];
            
            proj.transform.position = Vector3.MoveTowards(proj.transform.position, TargetPoint.transform.position, step);
            if(rotationSpeed != 0)
            {
                proj.transform.RotateAround(proj.transform.position, Vector3.up, step2);
            }

            if (proj.transform.position == TargetPoint.transform.position)
            {
                ActiveProjs.RemoveAt(i);
                Destroy(proj);
            }
        }
    }

    void CastProj()
    {
        var proj = Instantiate(Projectile, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        ActiveProjs.Add(proj);
    }
}
