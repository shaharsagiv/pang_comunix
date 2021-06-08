using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// UI event names to notify the controller
public enum UIEVENTS
{
    PLAY_CLICKED,
    LEADERBOARD_CLICKED,
    LEADERBOARD_NAME_ENTERED,
    LEADERBOAED_BACK_TO_MAIN,
    MULTIPLAYER_CLICKED
}

// view class for the game
public class PangView : MonoBehaviour
{
    [SerializeField] GameObject UIGameObject;
    [SerializeField] Text levelTxt;
    [SerializeField] Text scoreTxt;
    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject leaderboardPrefab;

    public Action<UIEVENTS,string> onUIEvent;
    public Action<PangLevel> onNewLevelCreated;

    private LeaderboardView leaderboardView;

    const string LEVEL_LABEL = "Level {0}";
    const string SCORE_LABEL = "Total Score: {0}";

    // gets the number of level prefabs in the game
    public int GetNumberOfLevelPrefabs { get {
           return levels.Length;
        } 
    }

    //starts a level by number and mode
    public void StartLevel(int level, GAME_MODE gameMode)
    {
        GameObject levelObject = Instantiate(levels[level], transform);
        PangLevel pangLevel = levelObject.GetComponent<PangLevel>();
        pangLevel.SetGameMode(gameMode);
        pangLevel.InitLevel();
        onNewLevelCreated.Invoke(pangLevel);
        UIGameObject.SetActive(false);
    }

    //returning from a level to main screen and update main screen levels
    public void OnBackToMainMenuFromLevel(int nextLevel, int score)
    {
        UIGameObject.SetActive(true);
        UpdateLabels(nextLevel,score);
    }

    // update labels by level and total score
    void UpdateLabels(int nextLevel, int score)
    {
        levelTxt.text = string.Format(LEVEL_LABEL, (nextLevel+1).ToString());
        scoreTxt.text = string.Format(SCORE_LABEL, score.ToString("N0"));
    }

    // play button callback method 
    // notify the controller that play button clicked 
    public void OnPlayClicked()
    {
        onUIEvent.Invoke(UIEVENTS.PLAY_CLICKED, string.Empty);
    }

    //open the leaderboard by top scores list and a boolean stating if a user can enter the top lists
    public void OpenLeaderboard(List<KeyValuePair<string,int>> topScores, bool hasInput)
    {
        leaderboardView = Instantiate(leaderboardPrefab,transform, false).GetComponent<LeaderboardView>();
        leaderboardView.SetLeaderboard(topScores, hasInput);
        leaderboardView.onAddButtonClicked += OnLeaderboardAddButtonClicked;
        leaderboardView.onBackToMainMenuClicked += OnLeaderboardBackToMainClicked;
        UIGameObject.SetActive(false);
    }

    // update the leaderboard by top scores list and a boolean stating if a user can enter the top lists
    public void UpdateLeaderboard(List<KeyValuePair<string, int>> topScores, bool hasInput)
    {
        if (leaderboardView != null)
        {
            leaderboardView.SetLeaderboard(topScores, hasInput);
        }
    }

    //notify controller that the user pressed the add button in the leaderboard 
    void OnLeaderboardAddButtonClicked(string name)
    {
        onUIEvent.Invoke(UIEVENTS.LEADERBOARD_NAME_ENTERED, name); 
    }

    // remove listeners and notify controller that the user pressed back to main menu from the leaderboard
    void OnLeaderboardBackToMainClicked()
    {
        leaderboardView.onAddButtonClicked -= OnLeaderboardAddButtonClicked;
        leaderboardView.onBackToMainMenuClicked -= OnLeaderboardBackToMainClicked;
        onUIEvent.Invoke(UIEVENTS.LEADERBOAED_BACK_TO_MAIN, string.Empty);
    }

    // callback method for the open leaderboard button
    public void OnOpenLeaderboardClicked()
    {
        onUIEvent.Invoke(UIEVENTS.LEADERBOARD_CLICKED, string.Empty);
    }

    // set all buttons in the view back to active when returning from leaderboard
    public void OnBackToMainMenuFromLeaderboard()
    {
        UIGameObject.SetActive(true);
    }

    // callback method for multiplayer button 
    // notify controller on multiplayer button clicked
    public void OnMultiplayerButtonClick()
    {
        onUIEvent.Invoke(UIEVENTS.MULTIPLAYER_CLICKED, string.Empty);
    }
}


