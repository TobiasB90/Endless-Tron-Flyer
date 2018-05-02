using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float flyingspeed;
    public float rotationspeed;
    public bool invertedmovement = true;
    public bool backToDefPos;
    public float currentVelocity;

    public float Vert;
    public float Horz;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vert = Input.GetAxis("Vertical");
        Horz = Input.GetAxis("Horizontal");


        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            transform.Rotate(Input.GetAxis("Vertical") * rotationspeed, 0.0f, -Input.GetAxis("Horizontal") * rotationspeed);
        }
        else transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, Mathf.SmoothDamp(transform.rotation.z, 0, ref currentVelocity, 1), transform.rotation.w);
        // player, destroyer and builder flying forward at the speed of 'flyingspeed'
        float step = flyingspeed * Time.deltaTime;
        transform.position += transform.forward * step;
        // EndlessBuilderDestroyer.transform.position = new Vector3(0, 0, transform.position.z);
    }
}