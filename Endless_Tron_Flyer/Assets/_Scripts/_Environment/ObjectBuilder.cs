using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{

    [Tooltip("Insert TunnelSystems with ObjectInformation.cs script attached to it.")] [SerializeField] private GameObject[] TunnelSystems;
    [Tooltip("Insert the 'Environment' GameObject from the Scene.")] [SerializeField] private GameObject Environment;
    [Tooltip("Insert the 'FlyingObject' (Player) from the Scene.")] [SerializeField] private GameObject FlyingObject;
    private enum Direction { Zero, Ninety, OneEighty, TwoSeventy};
    private Direction d;
    private int LastTunnel = 0;
    private int NextTunnel = 0;
    private ObjectInformation LastTunnelInfo;
    private ObjectInformation NextTunnelInfo;


    [Tooltip("How many 'TunnelSystems' should be built in advance at the start of the game?")] [SerializeField] private int TunnelInAdvance = 0;
    private float timesbuilt = 0;

    // Use this for initialization
    void Start()
    {
        // Get InformationScripts of Last and Next TunnelSystem
        LastTunnelInfo = TunnelSystems[LastTunnel].GetComponent<ObjectInformation>();
        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();

        // Build amount of Tunnels at Start of Game (Set 'TunnelInAdvance' value in inspector)
        while(TunnelInAdvance > 0)
        {
            InstantiateTunnel();
            TunnelInAdvance--;
        }

        InvokeRepeating("InstantiateTunnel", 1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateBuilderPosition()
    {
        // Change the position of the builder to the end of the tunnel after instantiating, using LastTunnelinfo.length/width
        switch (d)
        {
            case Direction.Zero:
                transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
                break;
            case Direction.Ninety:
                transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
                break;
            case Direction.OneEighty:
                transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z - LastTunnelInfo.length);
                break;
            case Direction.TwoSeventy:
                transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z - LastTunnelInfo.width);
                break;
        }
    }

    public void InstantiateTunnel()
    {
        // Change the position of the builder to the position, where the object has to be instantiated, using NextTunnelInfo.pivotlength
        switch (d)
        {
            case Direction.Zero:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + NextTunnelInfo.pivotlength);
                break;
            case Direction.Ninety:
                transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                break;
            case Direction.OneEighty:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - NextTunnelInfo.pivotlength);
                break;
            case Direction.TwoSeventy:
                transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                break;
        }

        // Instantiating the TunnelSystem
        GameObject NewTunnel = Instantiate(TunnelSystems[NextTunnel], new Vector3(transform.position.x, TunnelSystems[NextTunnel].transform.position.y, transform.position.z), transform.rotation);

        // Revert the position of the builder to the position, where the object has to be instantiated, using NextTunnelInfo.pivotlength
        switch (d)
        {
            case Direction.Zero:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - NextTunnelInfo.pivotlength);
                break;
            case Direction.Ninety:
                transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                break;
            case Direction.OneEighty:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + NextTunnelInfo.pivotlength);
                break;
            case Direction.TwoSeventy:
                transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                break;
        }
        
        LastTunnel = NextTunnel;
        LastTunnelInfo = TunnelSystems[LastTunnel].GetComponent<ObjectInformation>();

        // If the TunnelSystem is changing the general direction, change the Direction-enum and rotate the object
        if (LastTunnelInfo.goingRight)
        {
            if (d == Direction.Zero)
            {
                d = Direction.Ninety;
            }
            else if (d == Direction.Ninety)
            {
                d = Direction.OneEighty;
            }
            else if (d == Direction.OneEighty)
            {
                d = Direction.TwoSeventy;
            }
            else if (d == Direction.TwoSeventy)
            {
                d = Direction.Zero;
            }
            transform.Rotate(0, 90, 0);
        }

        if (LastTunnelInfo.goingLeft)
        {
            if (d == Direction.Zero)
            {
                d = Direction.TwoSeventy;
            }
            else if (d == Direction.Ninety)
            {
                d = Direction.Zero;
            }
            else if (d == Direction.OneEighty)
            {
                d = Direction.Ninety;
            }
            else if (d == Direction.TwoSeventy)
            {
                d = Direction.OneEighty;
            }
            transform.Rotate(0, -90, 0);
        }

        // Use the Environment object as parent for instantiated tunnelsystems
        NewTunnel.transform.parent = Environment.transform;
        timesbuilt += 1;

        UpdateBuilderPosition();
        ChooseNextTunnel();
    }

    private void ChooseNextTunnel()
    {
        // Choose the next tunnel based on settings given here
        while (NextTunnel == LastTunnel)
        {
            NextTunnel = Random.Range(0, TunnelSystems.Length);
        }
        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();  
    }
}
