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
    private enum CurrentDirection { Forward, ForwardLeft, ForwardRight, ForwardRotated, Back, Up, UpRotated, LeftUp, RightUp, Down, DownRotated, DownLeft, Left, LeftRotated, UpLeft, Right, DownRight, UpRight, RightRotated, RightDown, LeftDown };
    private CurrentDirection curDir;
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
    public bool DoubleCurves = false;
    private ObjectInformation NTunnel;
    private GameObject NewTunnel;
    [Tooltip("Insert the maximum height (upwards) of the TunnelSystems. (working a little bit)")] public int maxUpHeight;
    [Tooltip("Insert the maximum height (downwards) of the TunnelSystems. (working a little bit)")] public int maxDownHeight;
    [Tooltip("Insert the maximum Left & Right direction possible. (not working yet)")] public int maxLeftRight;
    [Tooltip("How many 'TunnelSystems' should be built in advance at the start of the game?")] [SerializeField] private int TunnelInAdvance = 0;
    [Tooltip("How many 'DefaultTunnels' should be built at the start?")] [SerializeField] private int DefaultTunnelInAdvance = 0;

    private int timesbuilt = 0;

    // Use this for initialization
    private void Start()
    {
        // Get InformationScripts of Last and Next TunnelSystem
        LastTunnelInfo = TunnelSystems[LastTunnel].GetComponent<ObjectInformation>();
        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();
        NTunnel = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();
        curDir = CurrentDirection.Forward;

        // Build amount of Tunnels at Start of Game(Set 'TunnelInAdvance' value in inspector)
        while (DefaultTunnelInAdvance > 0)
        {
            NextTunnel = 0;
            InstantiateDefaultTunnel();
            DefaultTunnelInAdvance--;
        }

        // Build amount of Tunnels at Start of Game (Set 'TunnelInAdvance' value in inspector)
        while (TunnelInAdvance > 0)
        {
            InstantiateTunnel();
            TunnelInAdvance--;
        }

        InvokeRepeating("InstantiateTunnel", 1f, 1f);
    }

    private void UpdateBuilderPosition()
    {

        if(NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Forward)
        {
            if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRotated)
        {
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardLeft)
        {
            if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
            else transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRight)
        {
            if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
        }
        else if(NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Left)
        {
            transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftRotated)
        {
            transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpLeft)
        {
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownLeft)
        {
            if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Right)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightRotated)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRight)
        {
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRight)
        {
            if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Up)
        {
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRotated)
        {
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
            else transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftUp)
        {
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightUp)
        {
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Down)
        {
            if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRotated)
        {
            if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftDown)
        {   
            if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z + LastTunnelInfo.length);
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightDown)
        {
            if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z + LastTunnelInfo.width);
        }
    }

    public void InstantiateTunnel()
    {   
        // Change the position of the builder to the position, where the object has to be instantiated, using NextTunnelInfo.pivotlength
        Vector3 OldPosition = transform.localPosition;
        Vector3 NewPosition = new Vector3(0, 0, 0);

        if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Up || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftUp || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightUp || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRotated)
        {
            //FACING UP
            NewPosition = new Vector3(0, NextTunnelInfo.pivotlength, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Down || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftDown || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightDown || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRotated)
        {
            //FACING DOWN
            NewPosition = new Vector3(0, -NextTunnelInfo.pivotlength, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Right || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRight || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRight || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightRotated)
        {
            //FACING RIGHT
            NewPosition = new Vector3(NextTunnelInfo.pivotlength, 0, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Left || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownLeft || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpLeft || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftRotated)
        {
            //FACING LEFT
            NewPosition = new Vector3(-NextTunnelInfo.pivotlength, 0, 0);
            transform.Translate(NewPosition, Space.World);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Forward || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardLeft || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRight || NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRotated)
        {
            //FACING FORWARD
            NewPosition = new Vector3(0, 0, NextTunnelInfo.pivotlength);
            transform.Translate(NewPosition, Space.World);
        }

        // Instantiating the TunnelSystem
        if (NextTunnel != 0)
        {
            NewTunnel = Instantiate(TunnelSystems[NextTunnel], transform.localPosition, transform.localRotation);
            LastTunnel = NextTunnel;
            LastTunnelInfo = TunnelSystems[LastTunnel].GetComponent<ObjectInformation>();
        }
        else if (NextTunnel == 0 && DefaultTunnelInAdvance == 0)
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

        NTunnel = NewTunnel.GetComponent<ObjectInformation>();

        // TunnelDirection for rotationchange
        switch (curDir)
        {
            case CurrentDirection.Forward:

                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Up;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Down;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                }
                break;
            case CurrentDirection.Up:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Forward;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.UpLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.UpRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                }
                break;
            case CurrentDirection.UpRotated:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRotated;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.ForwardRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.DownRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.DownLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
                }
                break;
            case CurrentDirection.Down:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Forward;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.DownLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.DownRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
                }
                break;
            case CurrentDirection.DownRotated:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.ForwardRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                    
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.UpRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.UpLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                }
                break;
            case CurrentDirection.Right:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.RightUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.RightDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Forward;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.RightRotated:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightRotated;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.LeftDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.LeftUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.ForwardRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                }
                break;
            case CurrentDirection.Left:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.LeftUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.LeftDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Forward;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                }
                break;
            case CurrentDirection.LeftRotated:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftRotated;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.RightDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.RightUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.ForwardRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.LeftUp:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.ForwardLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                }
                break;
            case CurrentDirection.UpLeft:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.ForwardRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.DownRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Up;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                }
                break;
            case CurrentDirection.LeftDown:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.ForwardRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                }
                break;
            case CurrentDirection.DownLeft:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.ForwardLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.UpRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRotated;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Down;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                }
                break;
            case CurrentDirection.RightUp:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.ForwardRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.UpRight:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.ForwardLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Up;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.DownRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                }
                break;
            case CurrentDirection.RightDown:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.ForwardLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.DownRight:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.ForwardRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Down;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.UpRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRotated;
                }
                break;
                //PROBLEM MAYBE
            case CurrentDirection.ForwardLeft:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.UpRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.DownLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.LeftUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.RightDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                }
                break;
            case CurrentDirection.ForwardRight:
                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.UpLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.DownRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.LeftDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.RightUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                }
                break;
            case CurrentDirection.ForwardRotated:

                if (LastTunnelInfo.NoDirectionalChange) NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.DownRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.UpRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRotated;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.RightRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightRotated;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.LeftRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftRotated;
                }
                break;

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
        
        // No DoubleCurves if set
        if (LastTunnelInfo.goingUp && !DoubleCurves || LastTunnelInfo.goingDown && !DoubleCurves || LastTunnelInfo.goingLeft && !DoubleCurves || LastTunnelInfo.goingRight && !DoubleCurves)
        {
            NextTunnel = 0;
            NextTunnelChosen = true;
        }
        // Going Forward -> any direction possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Forward && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(0, TunnelSystems.Length);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Going ForwardLeft -> any direction possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardLeft && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(0, TunnelSystems.Length);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Going ForwardRight -> any direction possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRight && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(0, TunnelSystems.Length);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Going ForwardRotated -> any direction possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRotated && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(0, TunnelSystems.Length);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went Right --> DOWN/LEFT/UP Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Right && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 2);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went RightRotated --> DOWN/RIGHT/UP Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightRotated && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 1);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went Left --> DOWN/RIGHT/UP Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Left && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 1);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went LeftRotated --> DOWN/LEFT/UP Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftRotated && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 2);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went Up --> DOWN/LEFT/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Up && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 3);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went UpRotated --> UP/LEFT/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRotated && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 4);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went Down --> UP/LEFT/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Down && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 4);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went DownRotated --> DOWN/LEFT/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRotated && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 3);
            NextTunnel = i;
            NextTunnelChosen = true;
        }
        // Went UpRight --> DOWN/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRight && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(1, 100);
            if (i < 50) NextTunnel = 2;
            else if (i > 50) NextTunnel = 4;
            NextTunnelChosen = true;
        }
        // Went RightUp --> LEFT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightUp && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            NextTunnel = 1;
            NextTunnelChosen = true;
        }
        // Went UpLeft --> DOWN/LEFT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpLeft && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(1, 100);
            if (i < 50) NextTunnel = 1;
            else if (i > 50) NextTunnel = 4;
            NextTunnelChosen = true;
        }
        // Went LeftUp --> RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftUp && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            NextTunnel = 2;
            NextTunnelChosen = true;
        }
        // Went DownRight --> UP/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRight && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(1, 100);
            if (i < 50) NextTunnel = 2;
            else if (i > 50) NextTunnel = 3;
            NextTunnelChosen = true;
        }
        // Went RightDown --> LEFT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightDown && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            NextTunnel = 1;
            NextTunnelChosen = true;
        }
        // Went DownLeft --> UP/LEFT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownLeft && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            int i = Random.Range(1, 100);
            if (i < 50) NextTunnel = 1;
            else if (i > 50) NextTunnel = 3;
            NextTunnelChosen = true;
        }
        // Went LeftDown --> RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftDown && LastTunnelInfo.goingForward && !NextTunnelChosen)
        {
            NextTunnel = 2;
            NextTunnelChosen = true;
        }
        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();
    }

    public void InstantiateDefaultTunnel()
    {
        Vector3 NewPosition;
        NewPosition = new Vector3(0, 0, NextTunnelInfo.pivotlength);
        transform.Translate(NewPosition, Space.World);
        NewTunnel = Instantiate(TunnelSystems[NextTunnel], transform.localPosition, transform.localRotation);
        timesbuilt++;
        NewTunnel.transform.parent = Environment.transform;
        transform.Translate(-NewPosition, Space.World);
        transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.length);
    }

    private int RandomExceptOne(int fromNr, int exclusiveToNr, int exceptNr)
    {
        int randomNr = exceptNr;

        while (randomNr == exceptNr)
        {
            randomNr = Random.Range(fromNr, exclusiveToNr);
        }

        return randomNr;
    }

    private int RandomExceptTwo(int fromNr, int exclusiveToNr, int exceptNr1, int exceptNr2)
    {
        int randomNr = exceptNr1;
        int randomNr2 = exceptNr2;

        while (randomNr == exceptNr1 || randomNr == exceptNr2)
        {
            randomNr = Random.Range(fromNr, exclusiveToNr);
        }
        return randomNr;
    }


}
