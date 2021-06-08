using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// view class for ball
public class BallView : MonoBehaviour
{
    private float forceX;
    private float forceY;

    private Rigidbody2D rb;

    [SerializeField] bool isMovingLeft;

    [SerializeField] BALL_SIZE ballSize;

    public Action<BallView> onBallPop;

    // properties
    public BALL_SIZE BallSize
    {
        get { return ballSize; }
        set { 
            ballSize = value;
            SetScale();
            SetBallSpeed();
        }
    }

    private void SetScale()
    {
        float scale = 0;
        switch (ballSize)
        {
            case BALL_SIZE.LARGEST:
                scale = 1.3f;
                break;
            case BALL_SIZE.LARGE:
                scale = 1f;
                break;
            case BALL_SIZE.MEDIUM:
                scale = 0.7f;
                break;
            case BALL_SIZE.SMALL:
                scale = 0.5f;
                break;
            case BALL_SIZE.SMALLEST:
                scale = 0.3f;
                break;
        }
        gameObject.transform.localScale = Vector3.one * scale;
    }

    public int id { get; internal set; }

    void SetBallSpeed()
    {
        forceX = 2.5f;
        switch (ballSize)
        {
            case BALL_SIZE.LARGEST:
                forceY = 11f;
                break;
            case BALL_SIZE.LARGE:
                forceY = 11f;
                break;
            case BALL_SIZE.MEDIUM:
                forceY = 10f;
                break;
            case BALL_SIZE.SMALL:
                forceY = 8f;
                break;
            case BALL_SIZE.SMALLEST:
                forceY = 7f;
                break;
        }
    }

    // move the ball in the desiered direction (right/left)
    void MoveBall()
    {
        if(isMovingLeft)
        {
            transform.position = new Vector3(transform.position.x - forceX * Time.deltaTime,
                                             transform.position.y,
                                             transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + forceX * Time.deltaTime,
                                             transform.position.y,
                                             transform.position.z);
        }

    }

    // detect hitting the bounds or arrow
    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Ground"))
        {
            rb.velocity = new Vector2(0, forceY);
        }
        else if (target.CompareTag("LeftWall"))
        {
            isMovingLeft = false;
        }
        else if (target.CompareTag("RightWall"))
        {
            isMovingLeft = true;
        }
        else if (target.CompareTag("Arrow"))
        {
             ArrowHit();
        }
    }

    // notify level view that an arrow hit
    void ArrowHit()
    {
        onBallPop.Invoke(this);
    }

    public void SetMoveLeft(bool moveLeft)
    {
        isMovingLeft = moveLeft;
    }

    // initialize 
    void Awake()
    {
        SetBallSpeed();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();
    }
}
