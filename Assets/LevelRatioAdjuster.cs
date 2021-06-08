using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// component to change level scaling acording to screen ratio
public class LevelRatioAdjuster : MonoBehaviour
{
    private float baseXValue = 1280;
    private float baseYValue = 720;

    // initialize and changing scale
    private void Awake()
    {
        float defaultRatio = baseXValue / baseYValue;
        float actualRatio = getScreenRatio();
        this.gameObject.transform.localScale *= actualRatio/ defaultRatio;
    }

    // gets the screen width
    float getScreenWidth()
    {
        Vector3 xRightBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Vector3 xLeftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        return xRightBound.x - xLeftBound.x;
    }

    // gets the screen height
    float getScreenHeight()
    {
        Vector3 xRightBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Vector3 xLeftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        return xRightBound.y - xLeftBound.y;
    }

    // calculates and gets the screen ratio
    float getScreenRatio()
    {
        return getScreenWidth() / getScreenHeight();
    }
}
