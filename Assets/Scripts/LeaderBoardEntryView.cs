using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a view class for a leaderboard entry
public class LeaderBoardEntryView : MonoBehaviour
{
    [SerializeField] Text nameTxt;
    [SerializeField] Text scoreTxt;

    // sets labels to the entry
    public void SetEntry(int place, KeyValuePair<string,int> Entry)
    {
        { 
            nameTxt.text = Entry.Key; 
            scoreTxt.text = Entry.Value.ToString();
        }
    }
}
