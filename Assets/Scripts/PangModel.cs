using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PangModel
{

    private int nextLevel = 0;
    private int score = 0;
    private int maxLeaderboardPlaces = 5;
    private GameDataHandler gameDataHandler;

    public PangModel()
    {
        gameDataHandler = new GameDataHandler(maxLeaderboardPlaces);
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int NextLevel
    {
        get { return nextLevel; }
        set { nextLevel = value; }
    }
}
