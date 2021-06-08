using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// view class for arrows
public class ArrowView : MonoBehaviour
{
    private Rigidbody2D rb;

    private float speed = 5f;

    //initialize rb
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //as long as the arrow alive move up
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, speed);
    }

    //destroy if hit the celling or a ball
    void OnTriggerEnter2D( Collider2D target)
    {
        if (target.gameObject.CompareTag("Celling"))
        {
            Destroy(gameObject);
        }
        else if(target.gameObject.GetComponent<BallView>() != null)
        {
            Destroy(gameObject);
        }
    }

}
