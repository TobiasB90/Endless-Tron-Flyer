using UnityEngine;

public class ObjectInformation : MonoBehaviour {

    [Tooltip("Insert _GameManager Object.")] public GameObject GameManager;
    private GameMng GMng;
    public int TunnelNumber = 1;
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
    [Tooltip("Insert Layer of the PlayerObject.")] [SerializeField] public LayerMask LMask;

    private void Start()
    {
        if(GameManager == null)
        {
            GameManager = GameObject.Find("_GameManager");
        }
        GMng = GameManager.GetComponent<GameMng>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((LMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Debug.Log("Direction change - adjusting rotation");
            CollisionDetection CDet = other.GetComponent<CollisionDetection>();
            CDet.ChangeDir(TunnelDir);
            GMng.TunnelSystemsSolved++;
        }
    }
}
