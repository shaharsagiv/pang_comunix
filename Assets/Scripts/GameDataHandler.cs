using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data handler class
//takes care of saving and fetching data in the local device
public class GameDataHandler
{
    const string KEY = "leaderboard";
    int maxLeaderboardPlaces;

    //constructor
    public GameDataHandler(int maxLeaderboardPlaces)
    {
        this.maxLeaderboardPlaces = maxLeaderboardPlaces;
    }

    //return a sorted list of the top scores from local device ["player name",score];
    public List<KeyValuePair<string,int>> GetTopScores()
    {
        List<KeyValuePair<string, int>> topScores = new List<KeyValuePair<string, int>>();
        if (PlayerPrefs.HasKey(KEY))
        {
            string topScoresStr = PlayerPrefs.GetString(KEY);
            if (topScoresStr != string.Empty)
            { 
                string[] topScoresStrArray = topScoresStr.Split(',');
                foreach (string nameScore in topScoresStrArray)
                {
                    string name = nameScore.Split('-')[0];
                    int score = int.Parse(nameScore.Split('-')[1]);
                    topScores.Add(new KeyValuePair<string, int>(name, score));
                }
            }
        }
        return topScores;
    }

    //save on device a list of scores 
    void SetTopScore(List<KeyValuePair<string, int>> topScores)
    {
        string nameScoreListStr = string.Empty;
        for(int i = 0 ; i < topScores.Count; i++)
        {
            if(i == topScores.Count - 1)
            {
                nameScoreListStr += topScores[i].Key.ToString() + "-" + topScores[i].Value.ToString();
            }
            else
            {
                nameScoreListStr += topScores[i].Key.ToString() + "-" + topScores[i].Value.ToString() + ",";
            }
        }
        if (nameScoreListStr != string.Empty)
        {
            PlayerPrefs.SetString(KEY, nameScoreListStr);
        }
    }

    // return true if a score has a place in the top scores
    public bool IsScoreInTopFive(int score)
    {
        List<KeyValuePair<string, int>> topScores = GetTopScores();
        if (topScores.Count < maxLeaderboardPlaces)
        {
            return true;
        }
        
        foreach (KeyValuePair<string, int> pair in topScores)
        {
            if (pair.Value < score)
            {
                return true;
            }
        }
        return false;
    }

    // adds an entry to the leaderboard
    public void AddEntryToLeaderboard(string name, int score)
    {
        KeyValuePair<string, int> entry = new KeyValuePair<string, int>(name,score);
        List<KeyValuePair<string, int>> topScores = GetTopScores();
        List<KeyValuePair<string, int>> newTopScores = new List<KeyValuePair<string, int>>();
        bool entryAdded = false;

        for (int i = 0; i<topScores.Count; i++)
        {
            if(topScores[i].Value < score && !entryAdded)
            {
                newTopScores.Add(entry);
                entryAdded = true;
            }
            if (newTopScores.Count < maxLeaderboardPlaces)
            {
                newTopScores.Add(topScores[i]);
            }
        }
        if (!entryAdded)
        {
            newTopScores.Add(entry);
        }

        SetTopScore(newTopScores);
    }
}
