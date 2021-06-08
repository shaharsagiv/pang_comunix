using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// a view class for leaderboard name input
public class LeaderBoardInputView : MonoBehaviour
{
    [SerializeField]  InputField inputTxt;

    public Action<string> onAddButtonClicked;
    const  string NAME_CLARIFICATION_MESSAGE = @"Your name";

    // if a user entered a name then notify leaderboard view
    // else change name to a message the user can understand the funcionality 
    public void Add()
    {
        if(inputTxt.text != string.Empty)
        {
            onAddButtonClicked.Invoke(inputTxt.text);
            Destroy(gameObject);
        }
        else
        {
            inputTxt.text = NAME_CLARIFICATION_MESSAGE;
        }   
    }
}
