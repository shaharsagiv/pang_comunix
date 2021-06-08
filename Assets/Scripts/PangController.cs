using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller class for the game
public class PangController
{
    private PangModel model;
    private PangView view;
    private int numberOfLevels;
    private GameDataHandler gameDataHandler;

    const int MAX_LEADERBOARD_PLACES = 5;

    //Constructor - initialize members
    public PangController(PangView view, PangModel model)
    {
        this.model = model;
        this.view = view;
        this.view.onUIEvent += OnUIEvent;
        this.view.onNewLevelCreated += OnLevelCreated;
        numberOfLevels = this.view.GetNumberOfLevelPrefabs;
        gameDataHandler = new GameDataHandler(MAX_LEADERBOARD_PLACES);
    }

    //listen to a level created event
    private void OnLevelCreated(PangLevel level)
    {
        level.onLevelEnd += OnLevelEnded;
    }

    //set the main screen ready for the user to be back from a level
    private void OnLevelEnded(bool levelSuccess, int score)
    {
        AddPlayerScore(score);

        if (levelSuccess)
        {
            if (model.NextLevel + 1 == numberOfLevels)
            {
                SetNextLevel(model.NextLevel + 1);
                if (gameDataHandler.IsScoreInTopFive(model.Score))
                {
                    view.OpenLeaderboard(gameDataHandler.GetTopScores(), true);
                }
            }
            else
            {
                SetNextLevel(model.NextLevel + 1);
            }

        }
        else
        {
            if (gameDataHandler.IsScoreInTopFive(model.Score))
            {
                view.OpenLeaderboard(gameDataHandler.GetTopScores(), true);
            }
            SetNextLevel(0);
        }

        view.OnBackToMainMenuFromLevel(GetNextLevel(), GetPlayerScore());
    }

    //listens to all events from view
    public void OnUIEvent(UIEVENTS eventName, string parameter)
    {
        switch (eventName)
        {
            case UIEVENTS.PLAY_CLICKED:
                StartLevel(model.NextLevel, GAME_MODE.SINGLE_PLAYER);
                break;
            case UIEVENTS.LEADERBOARD_CLICKED:
                view.OpenLeaderboard(gameDataHandler.GetTopScores(), false);
                break;
            case UIEVENTS.LEADERBOARD_NAME_ENTERED:
                gameDataHandler.AddEntryToLeaderboard(parameter, model.Score);
                view.UpdateLeaderboard(gameDataHandler.GetTopScores(), false);
                break;
            case UIEVENTS.LEADERBOAED_BACK_TO_MAIN:
                view.OnBackToMainMenuFromLeaderboard();
                break;
            case UIEVENTS.MULTIPLAYER_CLICKED:
                StartLevel(model.NextLevel, GAME_MODE.MULTIPLAYER2);
                break;
        }
    }

    //start a level by level number and a mode
    private void StartLevel(int level, GAME_MODE gameMode)
    {
        if (level == 0)
        {
            SetPlayerScore(0);
        }
        view.StartLevel(level,gameMode);
    }

    // set the player's score
    private void SetPlayerScore(int newScore)
    {
        model.Score = newScore;
    }
    
    // adds score to total score
    private void AddPlayerScore(int scoreToAdd)
    {
        SetPlayerScore(model.Score + scoreToAdd);
    }

    // gets player's total score
    private int GetPlayerScore()
    {
        return model.Score;
    }

    // sets next level to start
    private void SetNextLevel(int newLevel)
    {
        model.NextLevel = newLevel % numberOfLevels;
    }

    // gets the next level to start
    private int GetNextLevel()
    {
        return model.NextLevel;
    }

    //destructor
    //unregister to events
    ~PangController()
    {
        view.onUIEvent -= OnUIEvent;
        view.onNewLevelCreated -= OnLevelCreated;
    }
}
