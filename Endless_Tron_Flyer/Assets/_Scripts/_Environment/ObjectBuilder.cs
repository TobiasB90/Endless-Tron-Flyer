using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{

    [Tooltip("Insert TunnelSystems with ObjectInformation.cs script attached to it. ([0] has to be default, [1] LeftCure and [2] RightCurve.")] [SerializeField] private GameObject[] TunnelSystems;
    [Tooltip("Insert the 'Environment' GameObject from the Scene.")] [SerializeField] private GameObject Environment;
    [Tooltip("Insert the 'FlyingObject' (Player) from the Scene.")] [SerializeField] private GameObject FlyingObject;
    private enum Direction { Zero, Ninety, OneEighty, TwoSeventy};
    private Direction d;
    private enum UpDownDirection { Zero, Ninety, MinusNinety };
    private UpDownDirection dUpDown;
    private int LastTunnel = 0;
    private int NextTunnel = 0;
    public int defaulttunnelchance = 70;
    private ObjectInformation LastTunnelInfo;
    private ObjectInformation NextTunnelInfo;
    private float directioncounterLeftRight = 0;
    private float directioncounterUpDown = 0;



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

        InvokeRepeating("InstantiateTunnel", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateBuilderPosition()
    {
        // Change the position of the builder to the end of the tunnel after instantiating, using LastTunnelinfo.length/width

        if (LastTunnelInfo.goingLeft || LastTunnelInfo.goingRight || LastTunnelInfo.goingForward)
        {
            switch (d)
            {
                case Direction.Zero:
                    if (directioncounterUpDown == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
                        break;
                    }
                    //DONE
                    else if (directioncounterUpDown == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
                    break;
                case Direction.Ninety:
                    if (directioncounterUpDown == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
                        break;
                    }
                    //DONE
                    else if (directioncounterUpDown == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
                    break;
                case Direction.OneEighty:
                    if (directioncounterUpDown == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
                        break;
                    }
                    else if (directioncounterUpDown == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z - LastTunnelInfo.length);
                    break;
                case Direction.TwoSeventy:
                    if (directioncounterUpDown == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
                        break;
                    }
                    //DONE
                    else if (directioncounterUpDown == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z - LastTunnelInfo.width);
                    break;
            }
        }

        if (LastTunnelInfo.goingUp)
        {
            switch (dUpDown)
            {
                //DONE
                case UpDownDirection.Zero:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
                    break;
                //DONE
                case UpDownDirection.Ninety:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
                case UpDownDirection.MinusNinety:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
            }
        }
        if (LastTunnelInfo.goingDown)
        {
            switch (dUpDown)
            {
                //DONE
                case UpDownDirection.Zero:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
                    break;
                //DONE
                case UpDownDirection.Ninety:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
                case UpDownDirection.MinusNinety:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
            }
        }
    }

    public void InstantiateTunnel()
    {
        // Change the position of the builder to the position, where the object has to be instantiated, using NextTunnelInfo.pivotlength
        switch (d)
        {
            case Direction.Zero:
                if (directioncounterUpDown == 1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + NextTunnelInfo.pivotlength, transform.localPosition.z);
                    break;
                }
                else if (directioncounterUpDown == -1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - NextTunnelInfo.pivotlength, transform.localPosition.z);
                    break;
                }
                transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotwidth, transform.localPosition.y, transform.localPosition.z + NextTunnelInfo.pivotlength);
                break;
            case Direction.Ninety:
                if (directioncounterUpDown == 1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                    break;
                }
                //DONE
                else if (directioncounterUpDown == -1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                    break;
                }
                transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z - NextTunnelInfo.pivotwidth);
                break;
            case Direction.OneEighty:
                transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotwidth, transform.localPosition.y, transform.localPosition.z - NextTunnelInfo.pivotlength);
                break;
            case Direction.TwoSeventy:
                transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z + NextTunnelInfo.pivotwidth);
                break;
        }

        // Instantiating the TunnelSystem
        GameObject NewTunnel = Instantiate(TunnelSystems[NextTunnel], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

        // Revert the position of the builder to the position, where the object has to be instantiated, using NextTunnelInfo.pivotlength
        switch (d)
        {
            case Direction.Zero:
                if (directioncounterUpDown == 1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - NextTunnelInfo.pivotlength, transform.localPosition.z);
                    break;
                }
                else if (directioncounterUpDown == -1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + NextTunnelInfo.pivotlength, transform.localPosition.z);
                    break;
                }
                transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotwidth, transform.localPosition.y, transform.localPosition.z - NextTunnelInfo.pivotlength);
                break;
            case Direction.Ninety:
                if (directioncounterUpDown == 1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                    break;
                }
                //DONE
                else if (directioncounterUpDown == -1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z);
                    break;
                }
                transform.localPosition = new Vector3(transform.localPosition.x - NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z + NextTunnelInfo.pivotwidth);
                break;
            case Direction.OneEighty:
                transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotwidth, transform.localPosition.y, transform.localPosition.z + NextTunnelInfo.pivotlength);
                break;
            case Direction.TwoSeventy:
                transform.localPosition = new Vector3(transform.localPosition.x + NextTunnelInfo.pivotlength, transform.localPosition.y, transform.localPosition.z - NextTunnelInfo.pivotwidth);
                break;
        }

        LastTunnel = NextTunnel;
        LastTunnelInfo = TunnelSystems[LastTunnel].GetComponent<ObjectInformation>();

        // If the TunnelSystem is changing the general direction, change the Direction-enum and rotate the object
        if (LastTunnelInfo.goingRight)
        {
            directioncounterLeftRight++;
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
            directioncounterLeftRight--;
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

        if (LastTunnelInfo.goingUp)
        {
            directioncounterUpDown++;
            if (dUpDown == UpDownDirection.Zero)
            {
                dUpDown = UpDownDirection.Ninety;
            }
            else if (dUpDown == UpDownDirection.MinusNinety)
            {
                dUpDown = UpDownDirection.Zero;
            }
            transform.Rotate(-90, 0, 0);
        }

        if (LastTunnelInfo.goingDown)
        {
            directioncounterUpDown--;
            if (dUpDown == UpDownDirection.Zero)
            {
                dUpDown = UpDownDirection.MinusNinety;
            }
            else if (dUpDown == UpDownDirection.Ninety)
            {
                dUpDown = UpDownDirection.Zero;
            }
            transform.Rotate(90, 0, 0);
        }


        // TunnelDirection for rotationchange
        // Up
        if(LastTunnel == 0)
        {
            if (directioncounterUpDown == 1 && directioncounterLeftRight == 0)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
            }
            // UpRight
            else if (directioncounterUpDown == 1 && directioncounterLeftRight == 1)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
            }
            // UpLeft
            else if (directioncounterUpDown == 1 && directioncounterLeftRight == -1)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
            }
            // Down
            else if (directioncounterUpDown == -1 && directioncounterLeftRight == 0)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
            }
            // DownRight
            else if (directioncounterUpDown == -1 && directioncounterLeftRight == 1)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
            }
            // DownLeft
            else if (directioncounterUpDown == -1 && directioncounterLeftRight == -1)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
            }
            // Right
            else if (directioncounterLeftRight == 1 && directioncounterUpDown == 0)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
            }
            // Left
            else if (directioncounterLeftRight == -1 && directioncounterUpDown == 0)
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
            }

            // Forward
            else
            {
                ObjectInformation NTunnel = NewTunnel.GetComponent<ObjectInformation>();
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
            }
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

        NextTunnel = Random.Range(0, TunnelSystems.Length);
        

        if (directioncounterLeftRight == 1)
        {
            NextTunnel = 1;
        }
        else if (directioncounterLeftRight == -1)
        {
            NextTunnel = 2;
        }
        else if (directioncounterUpDown == 1)
        {
            while(NextTunnel == 3)
            {
                NextTunnel = Random.Range(0, TunnelSystems.Length);
            }
        }
        else if (directioncounterUpDown == -1)
        {
            while (NextTunnel == 4)
            {
                NextTunnel = Random.Range(0, TunnelSystems.Length);
            }
        }

        int i = Random.Range(1, 100);
        if (i < defaulttunnelchance)
        {
            NextTunnel = 0;
        }

        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();
    }
}
