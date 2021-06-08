using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//all options for anchoring
public enum ANCHOR
{
    LEFT,
    RIGHT,
    TOP,
    BOTTOM
}

// a class to anchor a gameobject at the borders of the screen using its BoxCollider 
public class BoxCollider2DScreenAnchor : MonoBehaviour
{

    [SerializeField] BoxCollider2D col;
    [SerializeField] ANCHOR anchor;

    void Awake()
    {
        int xVal, yVal;
        switch (anchor)
        {
            case ANCHOR.LEFT:
                {
                    xVal = 0;
                    Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(xVal, 0, 0));
                    mPos.x -= col.bounds.size.x / 2;
                    mPos.y = transform.position.y;
                    mPos.z = transform.position.z;
                    transform.position = mPos;
                    break;
                }
            case ANCHOR.RIGHT:
                {
                    xVal = 1;
                    Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(xVal, 0, 0));
                    mPos.x += col.bounds.size.x / 2;
                    mPos.y = transform.position.y;
                    mPos.z = transform.position.z;
                    transform.position = mPos;
                    break;
                }
            case ANCHOR.BOTTOM:
                {
                    yVal = 0;
                    Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(0, yVal, 0));
                    mPos.x = transform.position.x;
                    mPos.y -= col.bounds.size.y / 2;
                    mPos.z = transform.position.z;
                    transform.position = mPos;
                    break;
                }
            case ANCHOR.TOP:
                {
                    xVal = 1;
                    Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(xVal, 0, 0));
                    mPos.x = transform.position.x;
                    mPos.y += col.bounds.size.y / 2;
                    mPos.z = transform.position.z;
                    transform.position = mPos;
                    break;
                }
        }
    }
}
