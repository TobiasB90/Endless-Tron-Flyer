using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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
    private int defaulttunnelchance = 70;
    private ObjectInformation LastTunnelInfo;
    private ObjectInformation NextTunnelInfo;
    private float directioncounterLeftRight = 0;
    private float directioncounterUpDown = 0;
    [HideInInspector] public int currentHeight;
    private int currentLeftRight;
    private int currentForward;
    private int antiBuildInYourselfCounter = 0;
    private int maxCurvesWithoutForward = 3;
    private bool NextTunnelChosen = false;

    [System.Serializable] public struct zone_number
    {
        public int Zone_TunnelNumber;
        public int TunnelDifficulty_Easy_Chance;
        public Vector2 TunnelDifficulty_Easy_FromTo;
        public Vector2 TunnelDifficulty_Hard_FromTo;
    }
    public zone_number[] zones;
    private int zone_current;
    private int zone_tunnel_counter;

    [SerializeField] private bool DoubleCurves = false;
    [SerializeField] private bool ScriptedTunnels = false;
    [SerializeField] private GameObject[] ScriptedTunnelSystems;

    private ObjectInformation NTunnel;
    private GameObject NewTunnel;
    [Tooltip("Insert the maximum height (upwards&downwards) of the TunnelSystems. (working a little bit)")] public int maxUpHeight;
    [Tooltip("Insert the maximum Left & Right direction possible. (not working yet)")] public int maxLeftRight;
    [Tooltip("How many 'TunnelSystems' should be built in advance at the start of the game?")] [SerializeField] private int TunnelInAdvance = 0;
    [Tooltip("How many 'DefaultTunnels' should be built at the start?")] [SerializeField] private int DefaultTunnelInAdvance = 0;

    private int timesbuilt = 0;
    public int StartDestroyTunnelAsPlayerHitsTunnelNumber = 50;

    // Use this for initialization
    private void Start()
    {
        zone_current = 0;
        currentHeight = 0;
        currentLeftRight = 0;
        currentForward = 0;

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
    }

    public void DestroyTunnel()
    {
        if (StartDestroyTunnelAsPlayerHitsTunnelNumber <= 0) Destroy(Environment.transform.GetChild(0).gameObject);
        else StartDestroyTunnelAsPlayerHitsTunnelNumber--;
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
            // SHOULD REALLY WORK NOW OR PROBLEM
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
            // PLEASE WORK
            else if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftRotated)
        {
            // SHOULD WORK
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
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
            if (LastTunnelInfo.goingDown) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
            else if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y - LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
        }
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightRotated)
        {
            // SHOULD WORK
            if (LastTunnelInfo.goingUp) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y + LastTunnelInfo.width, transform.localPosition.z);
            else transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.length, transform.localPosition.y, transform.localPosition.z + LastTunnelInfo.width);
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
            // SHOULD WORK OR USE ELSE
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y + LastTunnelInfo.length, transform.localPosition.z);
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
            // SHOULD REALLY WORK
            else if (LastTunnelInfo.goingRight) transform.localPosition = new Vector3(transform.localPosition.x - LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
            // SHOULD REALLY WORK
            else if (LastTunnelInfo.goingLeft) transform.localPosition = new Vector3(transform.localPosition.x + LastTunnelInfo.width, transform.localPosition.y - LastTunnelInfo.length, transform.localPosition.z);
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
            ChooseDodgeTunnel();
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

        // TunnelDirection for rotationchange, currentForward/LeftRight/Height & antiBuildInYourselfCounter (Resets on Forward)
        switch (curDir)
        {
            case CurrentDirection.Forward:

                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Up;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Down;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
                }
                break;
            case CurrentDirection.Up:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                    currentHeight++;
                }
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
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.UpLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                    antiBuildInYourselfCounter++;
                    currentLeftRight--;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.UpRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                    antiBuildInYourselfCounter++;
                    currentLeftRight++;
                }
                break;
            case CurrentDirection.UpRotated:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRotated;
                    currentHeight++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.ForwardRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
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
                    antiBuildInYourselfCounter++;
                    currentLeftRight++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.DownLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
                    antiBuildInYourselfCounter++;
                    currentLeftRight--;
                }
                break;
            case CurrentDirection.Down:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                    currentHeight--;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Forward;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
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
                    antiBuildInYourselfCounter++;
                    currentLeftRight--;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.DownRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
                    antiBuildInYourselfCounter++;
                    currentLeftRight++;
                }
                break;
            case CurrentDirection.DownRotated:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                    currentHeight--;
                }
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
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.UpRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                    antiBuildInYourselfCounter++;
                    currentLeftRight++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.UpLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                    antiBuildInYourselfCounter++;
                    currentLeftRight--;
                }
                break;
            case CurrentDirection.Right:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                    currentLeftRight++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.RightUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                    antiBuildInYourselfCounter++;
                    currentHeight++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.RightDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                    antiBuildInYourselfCounter++;
                    currentHeight--;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Forward;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Forward;
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.RightRotated:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightRotated;
                    currentLeftRight++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.LeftDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.LeftUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
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
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                break;
            case CurrentDirection.Left:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                    currentLeftRight--;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.LeftUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                    antiBuildInYourselfCounter++;
                    currentHeight++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.LeftDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                    antiBuildInYourselfCounter++;
                    currentHeight--;
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
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                break;
            case CurrentDirection.LeftRotated:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftRotated;
                    currentLeftRight--;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.RightDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                    antiBuildInYourselfCounter++;
                    currentHeight--;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.RightUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                    antiBuildInYourselfCounter++;
                    currentHeight++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.ForwardRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.LeftUp:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                    currentHeight++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.RightRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightRotated;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
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
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                break;
            case CurrentDirection.UpLeft:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                    currentLeftRight--;
                }
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
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.DownRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Up;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                break;
            case CurrentDirection.LeftDown:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                    currentHeight--;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
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
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                break;
            case CurrentDirection.DownLeft:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
                    currentLeftRight--;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.ForwardLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
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
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Down;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Down;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                break;
            case CurrentDirection.RightUp:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                    currentHeight++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.LeftRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftRotated;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.ForwardRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.UpRight:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                    currentLeftRight++;
                }
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
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.Up;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Up;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.DownRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                break;
            case CurrentDirection.RightDown:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                    currentHeight--;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.Right;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Right;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.Left;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Left;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.ForwardLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.Back;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.Back;
                    Debug.Log("Building in myself, HELP!");
                }
                break;
            case CurrentDirection.DownRight:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
                    currentLeftRight++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.ForwardRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                    currentForward++;
                    antiBuildInYourselfCounter = 0;
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
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.UpRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRotated;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                break;
            case CurrentDirection.ForwardLeft:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardLeft;
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.UpRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRight;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.DownLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownLeft;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.LeftUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftUp;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.RightDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightDown;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                break;
            case CurrentDirection.ForwardRight:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRight;
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.UpLeft;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpLeft;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.DownRight;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRight;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.LeftDown;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftDown;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.RightUp;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightUp;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                break;
            case CurrentDirection.ForwardRotated:
                if (LastTunnelInfo.NoDirectionalChange)
                {
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.ForwardRotated;
                    antiBuildInYourselfCounter = 0;
                    currentForward++;
                }
                else if (LastTunnelInfo.goingUp)
                {
                    curDir = CurrentDirection.DownRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.DownRotated;
                    currentHeight--;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingDown)
                {
                    curDir = CurrentDirection.UpRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.UpRotated;
                    currentHeight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingLeft)
                {
                    curDir = CurrentDirection.RightRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.RightRotated;
                    currentLeftRight++;
                    antiBuildInYourselfCounter++;
                }
                else if (LastTunnelInfo.goingRight)
                {
                    curDir = CurrentDirection.LeftRotated;
                    NTunnel.TunnelDir = ObjectInformation.TunnelDirection.LeftRotated;
                    currentLeftRight--;
                    antiBuildInYourselfCounter++;
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
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Forward && !NextTunnelChosen)
        {
            // At maximum Right -> any direction except RIGHT possible
            if(currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction except LEFT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except UP possible
            if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except DOWN possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = Random.Range(0, TunnelSystems.Length);
                NextTunnel = i;
                NextTunnelChosen = true;
            }           
        }
        // Going ForwardLeft -> any direction possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardLeft && !NextTunnelChosen)
        {
            // At maximum Right -> any direction except UP possible
            if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction except DOWN possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except LEFT possible
            if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except RIGHT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = Random.Range(0, TunnelSystems.Length);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Going ForwardRight -> any direction possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRight && !NextTunnelChosen)
        {
            // At maximum Right -> any direction except DOWN possible
            if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction except UP possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except RIGHT possible
            if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except LEFT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = Random.Range(0, TunnelSystems.Length);
                NextTunnel = i;
                NextTunnelChosen = true;
            }            
        }
        // Going ForwardRotated
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.ForwardRotated && !NextTunnelChosen)
        {
            // At maximum Right -> any direction except LEFT possible
            if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Right -> any direction except RIGHT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except DOWN possible
            if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except UP possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // Any direction possible
            else
            {
                int i = Random.Range(0, TunnelSystems.Length);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went Right --> DOWN/LEFT/UP/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Right && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 1;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except UP/RIGHT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 3, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except DOWN/RIGHT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except FORWARD/RIGHT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction except RIGHT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went RightRotated --> DOWN/RIGHT/UP Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightRotated && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 2;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except DOWN/LEFT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except UP/LEFT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 3, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except FORWARD/LEFT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction except LEFT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went Left --> DOWN/RIGHT/UP/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Left && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 2;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except UP/LEFT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 3, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except DOWN/LEFT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except LEFT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except FORWARD/LEFT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went LeftRotated --> DOWN/LEFT/UP/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftRotated && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 1;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except DOWN/RIGHT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except UP/RIGHT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 3, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except RIGHT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except FORWARD/RIGHT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went Up --> DOWN/LEFT/RIGHT/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Up && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 4;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except FORWARD/UP possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except UP possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except RIGHT/UP possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except LEFT/UP possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went UpRotated --> UP/LEFT/RIGHT/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRotated && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 3;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except FORWARD/DOWN possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except DOWN possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except LEFT/DOWN possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except RIGHT/DOWN possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went Down --> UP/LEFT/RIGHT/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.Down && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 3;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except DOWN possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except FORWARD/DOWN possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except RIGHT/DOWN possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except LEFT/DOWN possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went DownRotated --> DOWN/LEFT/RIGHT/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRotated && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 4;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except UP possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except FORWARD/UP possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except LEFT/UP possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except RIGHT/UP possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went UpRight --> DOWN/RIGHT/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpRight && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 4;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except LEFT/UP possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except RIGHT/UP possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except FORWARD/UP possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except UP possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went RightUp --> LEFT/FORWARD/UP/DOWN Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightUp && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 1;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except FORWARD/RIGHT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except RIGHT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except DOWN/RIGHT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except RIGHT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went UpLeft --> DOWN/LEFT/RIGHT/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.UpLeft && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 4;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except RIGHT/UP possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except LEFT/UP possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except UP possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except FORWARD/UP possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 3);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went LeftUp --> RIGHT/FORWARD/UP/DOWN Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftUp && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 2;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except FORWARD/LEFT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except LEFT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except UP/LEFT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 3, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except DOWN/LEFT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went DownRight --> UP/RIGHT/LEFT/FORWARD Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownRight && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 3;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except RIGHT/DOWN possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except LEFT/DOWN possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except FORWARD/DOWN possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except DOWN possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went RightDown --> LEFT/FORWARD/UP Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.RightDown && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 1;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except DOWN/RIGHT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except FORWARD/DOWN/RIGHT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptThree(0, TunnelSystems.Length, 0, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except UP/DOWN/RIGHT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptThree(0, TunnelSystems.Length, 3, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except DOWN/RIGHT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 2);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went DownLeft --> UP/LEFT/FORWARD/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.DownLeft && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 3;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except LEFT/DOWN possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 1, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except RIGHT/DOWN possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 2, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except DOWN possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except FORWARD/DOWN possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 0, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptOne(0, TunnelSystems.Length, 4);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        // Went LeftDown --> FORWARD/UP/RIGHT Possible
        else if (NTunnel.TunnelDir == ObjectInformation.TunnelDirection.LeftDown && !NextTunnelChosen)
        {
            if (antiBuildInYourselfCounter >= maxCurvesWithoutForward)
            {
                NextTunnel = 2;
                NextTunnelChosen = true;
            }
            // At maximum Height -> any direction except DOWN/LEFT possible
            else if (currentHeight >= maxUpHeight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At minimum Height -> any direction except FORWARD/DOWN/LEFT possible
            else if (currentHeight <= -maxUpHeight)
            {
                int i = RandomExceptThree(0, TunnelSystems.Length, 0, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Right -> any direction except DOWN/LEFT possible
            else if (currentLeftRight >= maxLeftRight)
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            // At maximum Left -> any direction possible except UP/DOWN/LEFT possible
            else if (currentLeftRight <= -maxLeftRight)
            {
                int i = RandomExceptThree(0, TunnelSystems.Length, 3, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
            else
            {
                int i = RandomExceptTwo(0, TunnelSystems.Length, 4, 1);
                NextTunnel = i;
                NextTunnelChosen = true;
            }
        }
        NextTunnelInfo = TunnelSystems[NextTunnel].GetComponent<ObjectInformation>();
    }

    private void ChooseDodgeTunnel()
    {
        if(zone_tunnel_counter == zones[zone_current].Zone_TunnelNumber && zone_current != zones.Length - 1)
        {
            zone_current++;
            zone_tunnel_counter = 0;
        }

        int i = Random.Range(0, DogeTunnelSystems.Length);
        int chance = Random.Range(1, 100);
        if (chance <= zones[zone_current].TunnelDifficulty_Easy_Chance)
        {
            i = Random.Range(0, DogeTunnelSystems.Length);
            ObjectInformation tunnelInfo = DogeTunnelSystems[i].GetComponent<ObjectInformation>();
            while (isBetween(tunnelInfo.TunnelDifficulty, zones[zone_current].TunnelDifficulty_Easy_FromTo) == false)
            {
                i = Random.Range(0, DogeTunnelSystems.Length);
                tunnelInfo = DogeTunnelSystems[i].GetComponent<ObjectInformation>();
            }
        }
        else
        {
            i = Random.Range(0, DogeTunnelSystems.Length);
            ObjectInformation tunnelInfo = DogeTunnelSystems[i].GetComponent<ObjectInformation>();
            while (isBetween(tunnelInfo.TunnelDifficulty, zones[zone_current].TunnelDifficulty_Hard_FromTo) == false)
            {
                i = Random.Range(0, DogeTunnelSystems.Length);
                tunnelInfo = DogeTunnelSystems[i].GetComponent<ObjectInformation>();
            }
        }

        NewTunnel = Instantiate(DogeTunnelSystems[i], transform.localPosition, transform.localRotation);
        LastTunnel = NextTunnel;
        LastTunnelInfo = DogeTunnelSystems[i].GetComponent<ObjectInformation>();
        zone_tunnel_counter++;
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

    private bool isBetween(int input, params Vector2[] intervals)
    {
        for (int i = 0; i < intervals.Length; i++)
        {
            if (input >= intervals[i].x && input <= intervals[i].y)
                return true;
        }

        return false;
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

    private int RandomExceptThree(int fromNr, int exclusiveToNr, int exceptNr1, int exceptNr2, int exceptNr3)
    {
        int randomNr = exceptNr1;
        int randomNr2 = exceptNr2;
        int randomNr3 = exceptNr3;

        while (randomNr == exceptNr1 || randomNr == exceptNr2 || randomNr == exceptNr3)
        {
            randomNr = Random.Range(fromNr, exclusiveToNr);
        }
        return randomNr;
    }

}
