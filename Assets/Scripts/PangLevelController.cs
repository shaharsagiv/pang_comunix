using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controller class for level
public class PangLevelController : MonoBehaviour
{
    PangLevelModel model;
    PangLevelView view;
    Coroutine gameRunRoutine;
    private const string LAYERS = " Layers, ";
    private const string SECONDS = " Seconds";
    private const string GAMEOVER = "Game Over";
    private const string TIMESUP = "TIMES UP";
    private const string WELLDONE = "WELL DONE!";
    public Action<bool,int> onLevelEnded;

    //initialize members
    public void Init(PangLevelModel model, PangLevelView view)
    {
        this.model = model;
        this.view = view;
        this.view.onLevelViewEvent += OnLevelViewEvent;
        this.view.InitializeLevelView(this.model.Player1InitPos, this.model.Player2InitPos, this.model.BallsInitPos, this.model.Game_Mode);
        this.view.UpdateScore(0);
        gameRunRoutine = StartCoroutine(TimerRoutine());
    }

    //show a message to the player on a level start 
    void Start()
    {
        ShowMessgaeForStartLevel();
    }

    //listens to all events from level view
    private void OnLevelViewEvent(LEVEL_EVENTS eventName, System.Object[] args)
    {
        switch (eventName)
        {
            case LEVEL_EVENTS.BACK_TO_MENU_CLICKED:
                StopCoroutine(gameRunRoutine);
                EndLevel(false, 0f);
                break;
            case LEVEL_EVENTS.ADD_BALL_EVENT:
                model.BallsNum++;
                break;
            case LEVEL_EVENTS.PLAYER_HIT_EVENT:
                StopCoroutine(gameRunRoutine);
                view.DisablePlayers();
                view.ShowMessage(GAMEOVER, 2f);
                EndLevel(false, 2f);
                break;
            case LEVEL_EVENTS.BALL_POPED:
                if (args != null && args.Length >= 2)
                {
                    model.PopedCount++;
                    CalcAndAddScoreForPoppedBall((BALL_SIZE)args[1]);
                    if (model.AreAllBallsPopped() && (BALL_SIZE)args[1] == BALL_SIZE.SMALLEST)
                    {
                        view.PopBall((int)args[0]);
                        view.ShowMessage(WELLDONE, 2f);
                        EndLevel(true, 2f);
                    }
                    else if ((BALL_SIZE)args[1] != BALL_SIZE.SMALLEST)
                    {
                        if(UnityEngine.Random.Range(0, model.ChanceForPowerup) == 0)
                        {
                            view.DropPowerup(((POWERUP_TYPE)(UnityEngine.Random.Range(0, (int)POWERUP_TYPE.SIZE))), (int)args[0]);
                        }
                        view.SplitBall((int)args[0], (BALL_SIZE)args[1] - 1);
                    }
                    else if ((BALL_SIZE)args[1] == BALL_SIZE.SMALLEST)
                    {
                        if (UnityEngine.Random.Range(0, model.ChanceForPowerup) == 0)
                        {
                            view.DropPowerup(((POWERUP_TYPE)(UnityEngine.Random.Range(0, (int)POWERUP_TYPE.SIZE))), (int)args[0]);
                        }
                        view.PopBall((int)args[0]);
                    }
                    view.UpdateScore(model.Score);
                }
                break;
            case LEVEL_EVENTS.PLAYER_SHOOT:
                if (args != null)
                {
                    PLAYER_NUM playerNum = (PLAYER_NUM)args[0];
                    view.ShootArrow(playerNum);
                }

                break;
            case LEVEL_EVENTS.POWERUP_COLLECTED:
                if (args != null)
                {
                    POWERUP_TYPE type = (POWERUP_TYPE)args[0];
                    switch (type)
                    {
                        case POWERUP_TYPE.EXTRA_TIME:
                            model.TimeLeft += 5;
                            break;
                        case POWERUP_TYPE.POP_ALL_BALLS:
                            view.PopAllBalls();
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }

    //Coroutine for updating timer
    IEnumerator TimerRoutine()
    {
        model.TimeLeft = model.TimeForLevel;
        view.UpdateTimer(model.TimeLeft);
        while (model.IsLevelRunning && model.TimeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            model.TimeLeft--;
            view.UpdateTimer(model.TimeLeft);
        }
        
        if(!model.IsLevelRunning || model.TimeLeft <= 0)
        {
            view.ShowMessage(TIMESUP, 1.5f);
            yield return new WaitForSeconds(1.5f);
            view.ShowMessage(GAMEOVER, 1.5f);
            EndLevel(false, 1.5f);
        }
    }

    //end level 
    void EndLevel(bool isLevelSuccess, float time)
    {
        if(isLevelSuccess)
        {
            model.Score += model.ScorePerSecond * (int)model.TimeLeft;
        }
        StartCoroutine(EndLevelRoutine(isLevelSuccess, time));
    }

    //Coroutine for ending level
    IEnumerator EndLevelRoutine(bool isLevelSuccess, float time)
    {
        yield return new WaitForSeconds(time);
        onLevelEnded.Invoke(isLevelSuccess, model.Score);
    }

    //calculates score for popped ball and adds it
    void CalcAndAddScoreForPoppedBall(BALL_SIZE ball_size)
    {
        int score = ((int)ball_size + 1) * model.BasicScorePopedBall;
        AddScore(score);
    }

    //Adds score to level score
    void AddScore(int score)
    {
        model.Score += score;
    }

    //remove listener
    private void OnDestroy()
    {
        view.onLevelViewEvent -= OnLevelViewEvent;
    }

    //Show the user a start message
    // X layers, Y seconds
    private void ShowMessgaeForStartLevel()
    {
        BALL_SIZE maxBallSize = BALL_SIZE.SMALLEST;
        for ( int i = 0 ; i < model.BallsInitPos.Length ; i++ )
        {
            BallInitData ballInitData = model.BallsInitPos[i];
            if (ballInitData.ballSize > maxBallSize)
            {
                maxBallSize = ballInitData.ballSize;
            }
        }
        view.ShowMessage((int)(maxBallSize+1) + LAYERS + model.TimeForLevel.ToString() + SECONDS, 1.5f);
    }
}