using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Application class for level
public class PangLevel : MonoBehaviour
{
    [SerializeField] PangLevelView view;
    [SerializeField] PangLevelModel model;
    [SerializeField] PangLevelController controller;
    
    public Action<bool,int> onLevelEnd;

    // sets the game mode
    public void SetGameMode(GAME_MODE gameMode)
    {
        model.Game_Mode = gameMode;
    }

    //initialize controller 
    public void InitLevel()
    {
        controller.Init(model, view);
        controller.onLevelEnded += OnLevelEnded;
    }

    // remove listeners
    // notify pangController the level has ended
    // destroy level
    private void OnLevelEnded(bool levelSuccess, int score)
    {
        onLevelEnd.Invoke(levelSuccess, score);
        controller.onLevelEnded -= onLevelEnd;
        Destroy(gameObject);
    }

    // remove listener
    private void OnDestroy()
    {
        controller.onLevelEnded -= OnLevelEnded;
    }
}
