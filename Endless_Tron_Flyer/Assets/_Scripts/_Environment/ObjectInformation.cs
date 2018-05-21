using UnityEngine;

public class ObjectInformation : MonoBehaviour {

    public float length;
    public float width;
    public float pivotlength;
    public float pivotwidth;
    public bool goingLeft;
    public bool goingRight;
    public bool goingUp;
    public bool goingDown;
    public bool goingForward;
    public enum TunnelDirection { Forward, Up, Down, Left, Right, UpRight, UpLeft, DownRight, DownLeft, DoNothing };
    public TunnelDirection TunnelDir;
    //public PlayerController pcont;


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");

        CollisionDetection CDet = other.GetComponent<CollisionDetection>();
        CDet.ChangeDir(TunnelDir);
    }
}
