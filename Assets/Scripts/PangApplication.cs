using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Application class for the game
public class PangApplication : MonoBehaviour
{
    [SerializeField] PangView view;
    PangModel model;
    private PangController controller;
                     
    // initialize vars
    private void Awake()                                                                                                                                                                                                                                
    {
        model = new PangModel();
        controller = new PangController(view, model);
    }

    // checks if user pressed Esc and leave game
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}   