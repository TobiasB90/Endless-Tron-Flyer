using DG.Tweening;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    [Tooltip("Insert PlayerObject Here.")] public GameObject Player;
    [Tooltip("Insert PlayerModelObject Here.")] public GameObject PlayerModel;

    [Tooltip("Insert Layer the Player is dying to here.")] public LayerMask LMask;
    public Mesh PlayerMesh;
    public GameObject CubePrefab;
    public GameObject Parent;
    public GameObject InterfaceManager;
    IFaceMng_Limitless ifacemng;
	// Use this for initialization
	void Start () {
        ifacemng = InterfaceManager.GetComponent<IFaceMng_Limitless>();

        Vector3[] verts = PlayerMesh.vertices;

        Parent.transform.position = PlayerModel.transform.position;
        for (int i = 0; i < verts.Length; i++)
        {
            if (i % 2 == 0)
            {
                GameObject PlayerCube = Instantiate(CubePrefab, verts[i] + transform.position, PlayerModel.transform.rotation);
                PlayerCube.transform.parent = Parent.transform;
            }
            
        }

        // Parent.transform.rotation = PlayerModel.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider c)
    {
        if ((LMask & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            Debug.Log("PlayerCollision - Game Over");

            Parent.transform.position = PlayerModel.transform.position;
            Parent.transform.rotation = PlayerModel.transform.rotation;
            Parent.SetActive(true);
            foreach (Transform child in Parent.transform)
            {
                Rigidbody rbody = child.GetComponent<Rigidbody>();
                int i = Random.Range(1, 100);
                if (i < 70)
                {
                    rbody.AddRelativeForce(Vector3.forward * 1000);
                }
                else if (i > 70) rbody.AddRelativeForce(Vector3.up * 200);
                child.transform.DOScale(new Vector3(0.25f, 0.25f, 0.25f), 3f);
            }
            Destroy(Player.gameObject);
            ifacemng.ScoreScreen();
        }
            
    }

    public void ChangeDir(ObjectInformation.TunnelDirection TunnelDirx)
    {
        PlayerController pcont = Player.GetComponent<PlayerController>();
        switch (TunnelDirx)
        {
            case ObjectInformation.TunnelDirection.Forward:
                pcont.TunnelDir = PlayerController.TunnelDirection.Forward;
                break;
            case ObjectInformation.TunnelDirection.ForwardRotated:
                pcont.TunnelDir = PlayerController.TunnelDirection.ForwardRotated;
                break;
            case ObjectInformation.TunnelDirection.ForwardLeft:
                pcont.TunnelDir = PlayerController.TunnelDirection.ForwardLeft;
                break;
            case ObjectInformation.TunnelDirection.ForwardRight:
                pcont.TunnelDir = PlayerController.TunnelDirection.ForwardRight;
                break;
            case ObjectInformation.TunnelDirection.Up:
                pcont.TunnelDir = PlayerController.TunnelDirection.Up;
                break;
            case ObjectInformation.TunnelDirection.UpRotated:
                pcont.TunnelDir = PlayerController.TunnelDirection.UpRotated;
                break;
            case ObjectInformation.TunnelDirection.LeftUp:
                pcont.TunnelDir = PlayerController.TunnelDirection.LeftUp;
                break;
            case ObjectInformation.TunnelDirection.RightUp:
                pcont.TunnelDir = PlayerController.TunnelDirection.RightUp;
                break;
            case ObjectInformation.TunnelDirection.Down:
                pcont.TunnelDir = PlayerController.TunnelDirection.Down;
                break;
            case ObjectInformation.TunnelDirection.DownRotated:
                pcont.TunnelDir = PlayerController.TunnelDirection.DownRotated;
                break;
            case ObjectInformation.TunnelDirection.LeftDown:
                pcont.TunnelDir = PlayerController.TunnelDirection.LeftDown;
                break;
            case ObjectInformation.TunnelDirection.RightDown:
                pcont.TunnelDir = PlayerController.TunnelDirection.RightDown;
                break;
            case ObjectInformation.TunnelDirection.Left:
                pcont.TunnelDir = PlayerController.TunnelDirection.Left;
                break;
            case ObjectInformation.TunnelDirection.LeftRotated:
                pcont.TunnelDir = PlayerController.TunnelDirection.LeftRotated;
                break;
            case ObjectInformation.TunnelDirection.DownLeft:
                pcont.TunnelDir = PlayerController.TunnelDirection.DownLeft;
                break;
            case ObjectInformation.TunnelDirection.UpLeft:
                pcont.TunnelDir = PlayerController.TunnelDirection.UpLeft;
                break;
            case ObjectInformation.TunnelDirection.Right:
                pcont.TunnelDir = PlayerController.TunnelDirection.Right;
                break;
            case ObjectInformation.TunnelDirection.RightRotated:
                pcont.TunnelDir = PlayerController.TunnelDirection.RightRotated;
                break;
            case ObjectInformation.TunnelDirection.UpRight:
                pcont.TunnelDir = PlayerController.TunnelDirection.UpRight;
                break;            
            case ObjectInformation.TunnelDirection.DownRight:
                pcont.TunnelDir = PlayerController.TunnelDirection.DownRight;
                break;
            case ObjectInformation.TunnelDirection.NoRotation:
                pcont.TunnelDir = PlayerController.TunnelDirection.NoRotation;
                break;
        }
    }

    
}
