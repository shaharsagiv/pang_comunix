using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sizes for ball
public enum BALL_SIZE
{
    SMALLEST,
    SMALL,
    MEDIUM,
    LARGE,
    LARGEST
};

// calss to hold a ball initial data
// will be used on a gameobject at a desired initial positio for a ball spawning
public class BallInitData : MonoBehaviour
{
    public BALL_SIZE ballSize;
    public bool isMovingLeft;
}
