using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Model class for level
public class PangLevelModel : MonoBehaviour
{
    int score = 0;
    int poppedCount = 0;
    int ballsNum = 0;
    bool isLevelRunning = true;


    [SerializeField] int playTimeInSeconds;
    [SerializeField] int basicScorePopedBall;
    [SerializeField] int scorePerSecond;
    [SerializeField] GameObject player1InitPos;
    [SerializeField] GameObject player2InitPos;
    [SerializeField] BallInitData[] ballsInitPos;
    [SerializeField] int chanceForPowerup;

    int timeLeft;
    GAME_MODE gameMode;

    //properties
    public int ChanceForPowerup
    {
        get { return chanceForPowerup; }
        set { chanceForPowerup = value; }
    }

    public GAME_MODE Game_Mode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }

    public GameObject Player1InitPos
    {
        get { return player1InitPos; }
    }

    public GameObject Player2InitPos
    {
        get { return player2InitPos; }
    }

    public BallInitData[] BallsInitPos
    {
        get { return ballsInitPos; }
    }

    public int TimeLeft
    {
        get { return timeLeft; }
        set { timeLeft = value; }
    }

    public int TimeForLevel
    {
        get { return playTimeInSeconds; }
    }

    public bool IsLevelRunning
    {
        get { return isLevelRunning; }
        set { isLevelRunning = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int PopedCount
    {
        get { return poppedCount; }
        set { poppedCount = value; }
    }

    public int BallsNum
    {
        get { return ballsNum; }
        set { ballsNum = value; }
    }

    public int BasicScorePopedBall
    {
        get { return basicScorePopedBall; }
    }

    public int ScorePerSecond
    {
        get { return scorePerSecond; }
    }

    public bool AreAllBallsPopped()
    {
        return ballsNum == poppedCount;
    }
}

