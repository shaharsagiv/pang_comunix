using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// view class for leaderboard
public class LeaderboardView : MonoBehaviour
{
    [SerializeField] GameObject[] entries;
    [SerializeField] LeaderBoardInputView LeaderBoardInputView;

    public Action<string> onAddButtonClicked;
    public Action onBackToMainMenuClicked;


    // initialize leaderboard by topscores list and a boolean states if a user can enter the top scores list
    public void SetLeaderboard(List<KeyValuePair<string,int>> topScores, bool hasInput)
    {
        for (int i = 0 ; i < topScores.Count ; i++ )
        {
            entries[i].SetActive(true);
            LeaderBoardEntryView entryView = entries[i].GetComponent<LeaderBoardEntryView>();
            entryView.SetEntry(i + 1, topScores[i]);
        }
        if (hasInput)
        {
            LeaderBoardInputView.gameObject.SetActive(true);
            LeaderBoardInputView.onAddButtonClicked += OnAddButtonClicked;
        }
    }

    // notify pang view that add button was clicked
    private void OnAddButtonClicked(string name)
    {
        onAddButtonClicked.Invoke(name);
    }

    // notify pang view that back to main menu button was clicked
    // destroy leaderboard
    public void OnBackToMainMenuClicked()
    {
        onBackToMainMenuClicked.Invoke();
        Destroy(gameObject);
    }

    // remove listener
    private void OnDestroy()
    {
        LeaderBoardInputView.onAddButtonClicked -= OnAddButtonClicked;
    }
}
