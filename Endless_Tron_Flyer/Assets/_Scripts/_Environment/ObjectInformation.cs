using UnityEngine;

public class ObjectInformation : MonoBehaviour {

    [Tooltip("Insert _GameManager Object.")] private GameObject GameManager;
    private GameMng GMng;
    [HideInInspector] public int TunnelNumber = 1;
    [Range(1, 10)] public int TunnelDifficulty;
    public float length;
    public float width;
    public float pivotlength;
    public float pivotwidth;
    public bool goingLeft;
    public bool goingRight;
    public bool goingUp;
    public bool goingDown;
    public bool goingForward;
    public bool NoDirectionalChange;
    bool touched = false;
    public enum TunnelDirection { Forward, ForwardLeft, ForwardRight, ForwardRotated, Back, Up, UpRotated, Down, DownRotated, Left, LeftRotated, Right, RightRotated, UpRight, UpLeft, RightUp, LeftUp, DownRight, DownLeft, RightDown, LeftDown, NoRotation };
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
            if (!touched)
            {
                CollisionDetection CDet = other.GetComponent<CollisionDetection>();
                if (NoDirectionalChange) CDet.ChangeDir(TunnelDir);
                else CDet.ChangeDir(TunnelDirection.NoRotation);
                GMng.TunnelSystemsSolved++;
                touched = true;
            }
        }
    }
}
