using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{

    [Tooltip("Insert TunnelSystems with ObjectInformation.cs script attached to it. ([0] has to be default or dogetunnel, [1] LeftCure, [2] RightCurve, [3] UpCurve and [4] Downcurve.")] [SerializeField] private GameObject[] TunnelSystems;
    [Tooltip("Insert TunnelDogeSystems.")] [SerializeField] private GameObject[] DogeTunnelSystems;
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
    private int currentHeight;
    private int currentLeftRight;
    private bool NextTunnelChosen = false;
    private ObjectInformation NTunnel;
    private GameObject NewTunnel;
    [Tooltip("Insert the maximum height (upwards) of the TunnelSystems. (working a little bit)")] public int maxUpHeight;
    [Tooltip("Insert the maximum height (downwards) of the TunnelSystems. (working a little bit)")] public int maxDownHeight;
    [Tooltip("Insert the maximum Left & Right direction possible. (not working yet)")] public int maxLeftRight;
    [Tooltip("How many 'TunnelSystems' should be built in advance at the start of the game?")] [SerializeField] private int TunnelInAdvance = 0;
    private int timesbuilt = 0;

    // Use this for initialization
    void Start()
    {
        // Get InformationScripts of Last and Next TunnelSystem
        LastTunnelInfo = TunnelSystems[LastTunnel].GetComponent<ObjectInformation>();
        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();
        NTunnel = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();

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
                case UpDownDirection.Zero:
                    if (directioncounterLeftRight == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
                        break;
                    }
                    else if (directioncounterLeftRight == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
                    break;
                case UpDownDirection.Ninety:
                    if (directioncounterLeftRight == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
                        break;
                    }
                    else if (directioncounterLeftRight == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
                case UpDownDirection.MinusNinety:
                    if (directioncounterLeftRight == 1)
                    {
                        break;
                    }
                    else if (directioncounterLeftRight == -1)
                    {
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
            }
        }
        if (LastTunnelInfo.goingDown)
        {
            switch (dUpDown)
            {
                case UpDownDirection.Zero:
                    if (directioncounterLeftRight == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
                        break;
                    }
                    else if (directioncounterLeftRight == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
                    break;
                case UpDownDirection.Ninety:
                    if (directioncounterLeftRight == 1)
                    {
                        break;
                    }
                    else if (directioncounterLeftRight == -1)
                    {
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
                case UpDownDirection.MinusNinety:
                    if (directioncounterLeftRight == 1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
                        break;
                    }
                    else if (directioncounterLeftRight == -1)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
                        break;
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
                    break;
            }
        }
    }

    public void InstantiateTunnel()
    {
        // Change the position of the builder to the position, where the object has to be instantiated, using NextTunnelInfo.pivotlength
        Vector3 OldPosition = transform.localPosition;
        Vector3 NewPosition = new Vector3(0, 0, 0);
       
        if (directioncounterUpDown == 1 && directioncounterLeftRight == 0 || directioncounterUpDown == 1 && directioncounterLeftRight == -1 && LastTunnelInfo.goingUp || directioncounterUpDown == 1 && directioncounterLeftRight == 1 && LastTunnelInfo.goingUp)
        {
            //FACING UP
            NewPosition = new Vector3(0, NextTunnelInfo.pivotlength, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else if (directioncounterUpDown == -1 && directioncounterLeftRight == 0 || directioncounterUpDown == -1 && directioncounterLeftRight == -1 && LastTunnelInfo.goingDown || directioncounterUpDown == -1 && directioncounterLeftRight == 1 && LastTunnelInfo.goingDown)
        {
            //FACING DOWN
            NewPosition = new Vector3(0, -NextTunnelInfo.pivotlength, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else if (directioncounterUpDown == 1 && directioncounterLeftRight == 1 && LastTunnelInfo.goingRight || directioncounterUpDown == -1 && directioncounterLeftRight == 1 && LastTunnelInfo.goingRight || directioncounterLeftRight == 1 && directioncounterUpDown == 0 || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRight || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRight)
        {
            //FACING RIGHT
            NewPosition = new Vector3(NextTunnelInfo.pivotlength, 0, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else if (directioncounterUpDown == 1 && directioncounterLeftRight == -1 && LastTunnelInfo.goingLeft || directioncounterUpDown == -1 && directioncounterLeftRight == -1 && LastTunnelInfo.goingLeft || directioncounterLeftRight == -1 && directioncounterUpDown == 0 || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpLeft || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownLeft)
        {
            //FACING LEFT
            NewPosition = new Vector3(-NextTunnelInfo.pivotlength, 0, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else
        {
            //FACING FORWARD
            NewPosition = new Vector3(0, 0, NextTunnelInfo.pivotlength);
            transform.Translate(NewPosition, Space.World);
        }

        // Instantiating the TunnelSystem
        if(NextTunnel != 0)
        {
            NewTunnel = Instantiate(TunnelSystems[NextTunnel], transform.localPosition, transform.localRotation);
            LastTunnel = NextTunnel;
            LastTunnelInfo = TunnelSystems[LastTunnel].GetComponent<ObjectInformation>();
        }
        else if (NextTunnel == 0)
        {
            int i = Random.Range(0, DogeTunnelSystems.Length);
            NewTunnel = Instantiate(DogeTunnelSystems[i], transform.localPosition, transform.localRotation);
            LastTunnel = NextTunnel;
            LastTunnelInfo = DogeTunnelSystems[i].GetComponent<ObjectInformation>();
        }

        NextTunnelChosen = false;

        // Revert the positionchange to prepare for the next tunnel
        transform.Translate(-NewPosition, Space.World);

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
        NTunnel = NewTunnel.GetComponent<ObjectInformation>();
        if (NTunnel.NoDirectionalChange)
        {
            if (directioncounterUpDown == 1 && directioncounterLeftRight == 0)
            {
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                currentHeight++;
            }
            // UpRight
            else if (directioncounterUpDown == 1 && directioncounterLeftRight == 1)
            {
                currentLeftRight++;
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
            }
            // UpLeft
            else if (directioncounterUpDown == 1 && directioncounterLeftRight == -1)
            {
                currentLeftRight--;
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
            }
            // Down
            else if (directioncounterUpDown == -1 && directioncounterLeftRight == 0)
            {
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                currentHeight--;
            }
            // DownRight
            else if (directioncounterUpDown == -1 && directioncounterLeftRight == 1)
            {
                currentLeftRight++;
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
            }
            // DownLeft
            else if (directioncounterUpDown == -1 && directioncounterLeftRight == -1)
            {
                currentLeftRight--;
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
            }
            // Right
            else if (directioncounterLeftRight == 1 && directioncounterUpDown == 0)
            {
                currentLeftRight++;
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
            }
            // Left
            else if (directioncounterLeftRight == -1 && directioncounterUpDown == 0)
            {
                currentLeftRight--;
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
            }
            // Forward
            else
            {
                NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
            }
        }
        

        // Use the Environment object as parent for instantiated tunnelsystems
        NewTunnel.transform.parent = Environment.transform;
        timesbuilt += 1;
        NTunnel.TunnelNumber = timesbuilt;
        

        UpdateBuilderPosition();
        ChooseNextTunnel();
    }

    private void ChooseNextTunnel()
    {
        // Choose the next tunnel based on settings given here

        NextTunnel = Random.Range(0, TunnelSystems.Length);

        // Don't build in yourself!
        if (directioncounterLeftRight == 1 && !NextTunnelChosen)
        {
            int i = Random.Range(1, 100);
            if (i < defaulttunnelchance)
            {
                NextTunnel = 0;
            }
            else
            {
                NextTunnel = 1;
            }
            NextTunnelChosen = true;
        }
        else if (directioncounterLeftRight == -1 && !NextTunnelChosen)
        {
            int i = Random.Range(1, 100);
            if (i < defaulttunnelchance)
            {
                NextTunnel = 0;
            }
            else
            {
                NextTunnel = 2;
            }
            NextTunnelChosen = true;
        }
        else if (directioncounterUpDown == 1 && !NextTunnelChosen)
        {
            NextTunnel = RandomExcept(0, TunnelSystems.Length, 3);
            NextTunnelChosen = true;
        }
        else if (directioncounterUpDown == -1 && !NextTunnelChosen)
        {
            NextTunnel = RandomExcept(0, TunnelSystems.Length, 4);
            NextTunnelChosen = true;
        }
        
        
        // Control height of tunnels
        if (NTunnel.NoDirectionalChange == false && !NextTunnelChosen)
        {
            NextTunnel = 0;
            NextTunnelChosen = true;
        }
        else if (currentHeight >= maxUpHeight && directioncounterUpDown == 1 && directioncounterLeftRight == 0 && !NextTunnelChosen)
        {
            NextTunnel = 4;
            NextTunnelChosen = true;
        }
        else if (currentHeight >= maxUpHeight && directioncounterUpDown == 1 && directioncounterLeftRight == 1 && !NextTunnelChosen)
        {
            NextTunnel = 2;
            NextTunnelChosen = true;
        }
        else if (currentHeight >= maxUpHeight && directioncounterUpDown == 1 && directioncounterLeftRight == -1 && !NextTunnelChosen)
        {
            NextTunnel = 1;
            NextTunnelChosen = true;
        }
        else if (currentHeight >= maxDownHeight && directioncounterUpDown == -1 && directioncounterLeftRight == 0 && !NextTunnelChosen)
        {
            NextTunnel = 3;
            NextTunnelChosen = true;
        }
        else if (currentHeight <= maxDownHeight && directioncounterUpDown == -1 && directioncounterLeftRight == 1 && !NextTunnelChosen)
        {
            NextTunnel = 2;
            NextTunnelChosen = true;
        }
        else if (currentHeight <= maxDownHeight && directioncounterUpDown == -1 && directioncounterLeftRight == -1 && !NextTunnelChosen)
        {
            NextTunnel = 1;
            NextTunnelChosen = true;
        }
        else if (currentHeight <= maxDownHeight && directioncounterUpDown == 0 && directioncounterLeftRight == 0 && !NextTunnelChosen)
        {
            NextTunnel = 3;
            NextTunnelChosen = true;
        }
        else if (currentHeight >= maxUpHeight && directioncounterUpDown == 0 && directioncounterLeftRight == 0 && !NextTunnelChosen)
        {
            NextTunnel = 4;
            NextTunnelChosen = true;
        }
        

        if (!NextTunnelChosen)
        {
            int i = Random.Range(1, 100);
            if (i < defaulttunnelchance)
            {
                NextTunnel = 0;

            }            
            NextTunnelChosen = true;
        }
        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();
    }

    public int RandomExcept(int fromNr, int exclusiveToNr, int exceptNr)
    {
        int randomNr = exceptNr;

        while (randomNr == exceptNr)
        {
            randomNr = Random.Range(fromNr, exclusiveToNr);
        }

        return randomNr;
    }
}
