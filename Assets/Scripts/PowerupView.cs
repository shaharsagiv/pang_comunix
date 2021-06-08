using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Every powerup will have a type so when collected the controller will decide what to do with powerup
public enum POWERUP_TYPE
{
    EXTRA_TIME,
    POP_ALL_BALLS,
    SIZE
}

//A view class for powerup instances
public class PowerupView : MonoBehaviour
{
    public Action<POWERUP_TYPE, PowerupView> onPowerupCollected;
    [SerializeField] POWERUP_TYPE type;
    [SerializeField] int timeToCollect;

    //detects collision with a player and notify level view
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            onPowerupCollected.Invoke(type,this);
            Destroy(gameObject);
        }
    }

    // self terminate after timeToCollect is over
    IEnumerator SelfTerminate()
    {
        yield return new WaitForSeconds(timeToCollect);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(SelfTerminate());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
