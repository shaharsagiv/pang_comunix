using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

//animation options for the player
enum PLAYER_ANIMATION
{
    RUN,
    SHOOT,
    DIE
};

//player num to identify a player in multiplayer mode
public enum PLAYER_NUM
{
    PLAYER1,
    PLAYER2
}

// view class for the player
public class PlayerView : MonoBehaviour
{
    [SerializeField] AudioClip shootSound;

    private float speed = 8f;
    private float maxVelocity = 7f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool canShoot;
    private bool canWalk;
    private bool isGameRunning;
    private bool rightBtnPressed;
    private bool leftBtnPressed;

    private PLAYER_NUM playerNum;

    public Action<PLAYER_NUM> onPlayerShootArrow;
    public Action onPlayerGetHit;

    //properties
    public PLAYER_NUM PlayerNum 
    {
        get { return playerNum; } 
        set { playerNum = value; }
    }

    //initialize 
    void Awake()
    {
        InitVars();
    }

    //tries to walk 
    private void FixedUpdate()
    {
        PlayerWalk();
    }

    //check for input from keyboard
    private void Update()
    {
        InputCheck();
    }

    //walk if possible to a desired direction (right/left)
    void PlayerWalk()
    {
        float force = 0f;
        float velocity = Mathf.Abs(rb.velocity.x);

        if (canWalk)
        {

            if ((playerNum == PLAYER_NUM.PLAYER1 && Input.GetKey(KeyCode.RightArrow)) ||
                (playerNum == PLAYER_NUM.PLAYER2 && Input.GetKey(KeyCode.D)) || rightBtnPressed)
            {
                //Moving right
                if (velocity < maxVelocity)
                {
                    force = speed;
                }

                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;

                anim.SetBool(PLAYER_ANIMATION.RUN.ToString(), true);
            }
            else if ((playerNum == PLAYER_NUM.PLAYER1 && Input.GetKey(KeyCode.LeftArrow)) ||
                (playerNum == PLAYER_NUM.PLAYER2 && Input.GetKey(KeyCode.A)) || leftBtnPressed)
            {
                //Moving left
                if (velocity < maxVelocity)
                {
                    force = -speed;
                }

                Vector3 scale = transform.localScale;
                scale.x = -1 * Mathf.Abs(scale.x);
                transform.localScale = scale;

                anim.SetBool(PLAYER_ANIMATION.RUN.ToString(), true);
            }
            else 
            {
                anim.SetBool(PLAYER_ANIMATION.RUN.ToString(), false);
            }

            rb.AddForce(new Vector2(force, 0));
        }
    }

    // initialize variables
    void InitVars()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canShoot = true;
        canWalk = true;
        isGameRunning = true;
        rightBtnPressed = false;
        leftBtnPressed = false;
    }

    // shoots an arrow if possible
    public void OnShootBtnPressed()
    {
        if (canShoot)
        {
            StartCoroutine(ShootTheArrow());
        }
    }

    // checks keyboard input for shooting arrows
    void InputCheck()
    {
        if ((playerNum == PLAYER_NUM.PLAYER1 && Input.GetKeyDown(KeyCode.Space)) || (playerNum == PLAYER_NUM.PLAYER2 && Input.GetKeyDown(KeyCode.LeftShift)))
        {
            OnShootBtnPressed();
        }
    }

    //shoots an arrow coroutine
    IEnumerator ShootTheArrow()
    {
        canWalk = false;
        canShoot = false;
        anim.Play(PLAYER_ANIMATION.SHOOT.ToString());

        onPlayerShootArrow.Invoke(playerNum);

        yield return new WaitForSeconds(0.1f);
        canWalk = true;

        yield return new WaitForSeconds(0.1f);
        anim.SetBool(PLAYER_ANIMATION.SHOOT.ToString(), false);        

        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }

    // detect a ball hit
    // notify a ball hit
    // play die animation
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.GetComponent<BallView>() != null && isGameRunning)
        {
            canWalk = false;
            anim.Play(PLAYER_ANIMATION.DIE.ToString());
            onPlayerGetHit.Invoke();
        }
    }

    // set flags when a player die
    public void OnPlayerDied()
    {
        canShoot = false;
        isGameRunning = false;
    }

    // set flags when a right button is down
    public void OnRightBtnDown()
    {
        rightBtnPressed = true;
    }

    // set flags when a left button is down
    public void OnLeftBtnDown()
    {
        leftBtnPressed = true;
    }

    // set flags when a right button is up
    public void OnRightBtnUp()
    {
        rightBtnPressed = false;
    }

    // set flags when a left button is up
    public void OnLeftBtnUp()
    {
        leftBtnPressed = false;
    }

    // ignore collisions with another player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(gameObject.tag))
        {
            Collider2D myCol = gameObject.GetComponent<Collider2D>();
            Collider2D targetCollider = collision.gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(myCol,targetCollider);
        }
    }
}
