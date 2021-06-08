using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//A level has different modes 
public enum GAME_MODE
{
    SINGLE_PLAYER,
    MULTIPLAYER2
}

//All events that happens in the level being used to notify PangLevelController
public enum LEVEL_EVENTS
{
    BACK_TO_MENU_CLICKED,
    ADD_BALL_EVENT,
    PLAYER_HIT_EVENT,
    BALL_POPED,
    PLAYER_SHOOT,
    POWERUP_COLLECTED
};

//A view class for a level
public class PangLevelView : MonoBehaviour
{
    PlayerView player1View;
    PlayerView player2View;

    [SerializeField] Text timerTxt;
    [SerializeField] Text scoreTxt;
    [SerializeField] Text messageText;

    [SerializeField] GameObject fireBtn1;
    [SerializeField] GameObject leftBtn1;
    [SerializeField] GameObject rightBtn1;
    [SerializeField] GameObject fireBtn2;
    [SerializeField] GameObject leftBtn2;
    [SerializeField] GameObject rightBtn2;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject[] powerupsPrefabs;

    [SerializeField] AudioClip[] popSounds;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip powerupCollectSound;
    [SerializeField] AudioClip loopMusic;

    private GAME_MODE gameMode;

    List<GameObject> ballsList;
    int ballIdCounter = 0;

    public Action<LEVEL_EVENTS,System.Object[]> onLevelViewEvent;

    private void OnPlayerGetHit()
    {
        onLevelViewEvent.Invoke(LEVEL_EVENTS.PLAYER_HIT_EVENT, null);
    }

    //level initialization
    //instanciating players and balls 
    //playing background music
    //shows buttons for android users
    public void InitializeLevelView(GameObject player1InitPos, GameObject player2InitPos, BallInitData[] ballsInitPosSize, GAME_MODE mode)
    {
        gameMode = mode;

        player1View = Instantiate(playerPrefab, player1InitPos.transform.position, Quaternion.identity, transform).GetComponent<PlayerView>();
        player1View.gameObject.transform.localScale = Vector3.one * 0.55f;
        player1View.onPlayerShootArrow += OnPlayerShootArrow;
        player1View.onPlayerGetHit += OnPlayerGetHit;
        player1View.PlayerNum = PLAYER_NUM.PLAYER1;

        if(gameMode == GAME_MODE.MULTIPLAYER2)
        {
            player2View = Instantiate(playerPrefab, player2InitPos.transform.position, Quaternion.identity, transform).GetComponent<PlayerView>();
            player2View.gameObject.transform.localScale = Vector3.one * 0.55f;
            player2View.onPlayerShootArrow += OnPlayerShootArrow;
            player2View.onPlayerGetHit += OnPlayerGetHit;
            player2View.PlayerNum = PLAYER_NUM.PLAYER2;
            SpriteRenderer player2Sprite = player2View.GetComponent<SpriteRenderer>();
            player2Sprite.color = Color.green;
        }

        ballsList = new List<GameObject>();

        foreach (BallInitData ballInitData in ballsInitPosSize)
        {
            BallView ball = Instantiate(ballPrefab, ballInitData.gameObject.transform.position, Quaternion.identity, ballInitData.gameObject.transform.parent).GetComponent<BallView>();
            ball.BallSize = ballInitData.ballSize;
            ballsList.Add(ball.gameObject);
            ball.onBallPop += OnBallPop;
            ball.id = ballIdCounter;
            ball.SetMoveLeft(ballInitData.isMovingLeft);
            ballIdCounter++;
            AddBall(ball.gameObject);
        }

        SoundManager.Instance.PlayMusic(loopMusic);

#if UNITY_ANDROID
        switch (gameMode)
        {
            case GAME_MODE.SINGLE_PLAYER:
                fireBtn2 .SetActive(true);
                leftBtn1.SetActive(true);
                rightBtn1.SetActive(true);
                break;
            case GAME_MODE.MULTIPLAYER2:
                fireBtn1.SetActive(true);
                leftBtn1.SetActive(true);
                rightBtn1.SetActive(true);
                fireBtn2.SetActive(true);
                leftBtn2.SetActive(true);
                rightBtn2.SetActive(true);
                break;
        }
#endif
    }

    //pop all balls that are currently visible to the player
    public void PopAllBalls()
    {
        //don't want to pop the new balls that are being created
        int count = ballsList.Count;
        for( int i = 0 ; i < count ; i++ )
        {
            if(ballsList[i].activeSelf)
            {
                BallView ballView = ballsList[i].GetComponent<BallView>();
                OnBallPop(ballView);
            }
        }
    }

    //Drops powerup for the player to collect
    public void DropPowerup(POWERUP_TYPE type, int id)
    {
        if (type != POWERUP_TYPE.SIZE)
        {
            GameObject ball = getBallById(id);
            PowerupView powerupView = Instantiate(powerupsPrefabs[(int)type], ball.transform.position, Quaternion.identity, ball.transform.parent).GetComponent<PowerupView>();
            powerupView.onPowerupCollected += OnPowerupCollected;
        }
    }

    //pops a ball by id
    public void PopBall(int id)
    {
        GameObject ball = getBallById(id);
        ball.SetActive(false);
        SoundManager.Instance.RandomSoundEffect(popSounds);
    }

    //split a ball into 2 balls by id and size
    public void SplitBall(int id, BALL_SIZE ball_size)
    {
        GameObject ball = getBallById(id);

        BallView ball1 = Instantiate(ballPrefab, ball.transform.position, Quaternion.identity, ball.transform.parent).GetComponent<BallView>();
        BallView ball2 = Instantiate(ballPrefab, ball.transform.position, Quaternion.identity, ball.transform.parent).GetComponent<BallView>();

        ball1.SetMoveLeft(true);
        ball2.SetMoveLeft(false);

        ball1.BallSize = ball_size;
        ball2.BallSize = ball_size;

        ball1.onBallPop += OnBallPop;
        ball2.onBallPop += OnBallPop;

        ball1.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3f);
        ball2.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3f);

        ball1.id = ballIdCounter;
        ball2.id = ballIdCounter + 1;

        ballIdCounter += 2;

        AddBall(ball1.gameObject);
        AddBall(ball2.gameObject);

        SoundManager.Instance.RandomSoundEffect(popSounds);

        ball.SetActive(false);
    }

    //Add ball to ball list and notify controller
    private void AddBall(GameObject ball)
    {
        onLevelViewEvent.Invoke(LEVEL_EVENTS.ADD_BALL_EVENT, null);
        ballsList.Add(ball);
    }

    //gets a ball from balls list
    private GameObject getBallById(int id)
    {
        foreach(GameObject ball in ballsList)
        {
            BallView ballView = ball.GetComponent<BallView>();
            if(ballView != null && ballView.id == id)
            {
                return ball;
            }
        }
        return null;
    }

    //notify controller for ball poped
    private void OnBallPop(BallView ball)
    {
        System.Object[] args = new System.Object[3];
        args[0] = ball.id;
        args[1] = ball.BallSize;
        args[2] = ball.gameObject.transform.position;
        onLevelViewEvent.Invoke(LEVEL_EVENTS.BALL_POPED,args);
    }

    //instantiate an arrow
    public void ShootArrow(PLAYER_NUM playerNum)
    {
        PlayerView playerView;
        if (playerNum == PLAYER_NUM.PLAYER1)
        {
            playerView = player1View;
        }
        else
        {
            playerView = player2View;
        }

        SoundManager.Instance.Play(shootSound);
        Instantiate(arrowPrefab, playerView.gameObject.transform.position, Quaternion.identity, transform);
    }

    //notify controller that an arrow is being shot
    private void OnPlayerShootArrow(PLAYER_NUM playerNum)
    {
        object[] parameters = { playerNum };
        onLevelViewEvent.Invoke(LEVEL_EVENTS.PLAYER_SHOOT, parameters);
    }

    //updates timer label
    public void UpdateTimer(int timeLeft)
    {
        timerTxt.text = SecondsToMinutesSeconds(timeLeft);
    }
    
    //updates score label
    public void UpdateScore(int score)
    {
        scoreTxt.text = score.ToString("N0");
    }

    //notifys controller that a back to menu button has been pressed
    //a callback function for the main menu button
    public void OnMenuButtonPressed()
    {
        onLevelViewEvent.Invoke(LEVEL_EVENTS.BACK_TO_MENU_CLICKED, null);
    }

    //remove all registered listeners and stop all coroutines
    private void OnDestroy()
    {
        player1View.onPlayerShootArrow -= OnPlayerShootArrow;
        player1View.onPlayerGetHit -= OnPlayerGetHit;
        if(gameMode == GAME_MODE.MULTIPLAYER2)
        {
            player2View.onPlayerShootArrow -= OnPlayerShootArrow;
            player2View.onPlayerGetHit -= OnPlayerGetHit;
        }
        SoundManager.Instance.StopMusic();
        StopAllCoroutines();
    }

    //convert seconds to minutes and seconds
    //90 secs => 1:30
    private string SecondsToMinutesSeconds(int secs)
    {
        TimeSpan t = TimeSpan.FromSeconds(secs);

        return string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds); 
    }
     
    //show a message to the user
    public void ShowMessage(string msg, float time)
    {
        StartCoroutine(ShowMessageRoutine(msg, time));
    }

    //show a message to the user coroutines
    private IEnumerator ShowMessageRoutine(string msg, float time)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = msg;

        yield return new WaitForSeconds(time);

        messageText.gameObject.SetActive(false);
    }

    //disable players
    public void DisablePlayers()
    {
        player1View.OnPlayerDied();
        if(gameMode == GAME_MODE.MULTIPLAYER2)
        {
            player2View.OnPlayerDied();
        }
    }

    //callback function for right button when pressed
    public void OnRightBtnDown(int btnNum)
    {
        if( btnNum == 1 || gameMode == GAME_MODE.SINGLE_PLAYER)
        {
            player1View.OnRightBtnDown();
        }
        else
        {
            player2View.OnRightBtnDown();
        } 
    }

    //callback function for left button when pressed
    public void OnLeftBtnDown(int btnNum)
    {
        if (btnNum == 1 || gameMode == GAME_MODE.SINGLE_PLAYER)
        {
            player1View.OnLeftBtnDown();
        }
        else
        {
            player2View.OnLeftBtnDown();
        }
    }

    //callback function for right button when user stop pressing the button
    public void OnRightBtnUp(int btnNum)
    {
        if (btnNum == 1 || gameMode == GAME_MODE.SINGLE_PLAYER)
        {
            player1View.OnRightBtnUp();
        }
        else
        {
            player2View.OnRightBtnUp();
        }
    }

    //callback function for left button when user stop pressing the button
    public void OnLeftBtnUp(int btnNum)
    {
        if (btnNum == 1 || gameMode == GAME_MODE.SINGLE_PLAYER)
        {
            player1View.OnLeftBtnUp();
        }
        else
        {
            player2View.OnLeftBtnUp();
        }
    }

    //callback function for shoot button pressed
    public void OnShootBtnPressed(int btnNum)
    {
        if (btnNum == 1 || gameMode == GAME_MODE.SINGLE_PLAYER)
        {
            player1View.OnShootBtnPressed();
        }
        else
        {
            player2View.OnShootBtnPressed();
        }
    }

    //notifys controller when powerup is being collected 
    private void OnPowerupCollected(POWERUP_TYPE type,PowerupView powerupView)
    {
        powerupView.onPowerupCollected -= OnPowerupCollected;
        object[] parameters = { type };
        onLevelViewEvent.Invoke(LEVEL_EVENTS.POWERUP_COLLECTED, parameters);
        SoundManager.Instance.Play(powerupCollectSound);
    }
}

