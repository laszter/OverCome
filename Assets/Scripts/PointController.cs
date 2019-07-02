using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    public enum TurnSide
    {
        North, East, Sount, West
    }

    public TurnSide side;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetSide()
    {
        switch (side)
        {
            case TurnSide.North:
                return new Vector3(0, 0, 0);
            case TurnSide.East:
                return new Vector3(0, 90f, 0);
            case TurnSide.Sount:
                return new Vector3(0, 180f, 0);
            case TurnSide.West:
                return new Vector3(0, 270f, 0);
            default:
                return Vector3.zero;
        }
    }
}
