using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHacksss : MonoBehaviour
{
    private Vector3 startPos;
    public bool isPaused = false;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //************************************ TIME ************************************//
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            Time.timeScale = (!isPaused) ? 1 : 0.0001f;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            isPaused = false;
            Time.timeScale /= 2;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            isPaused = false;
            Time.timeScale *= 2;
        }


        //************************************ POSITION ************************************//
        if (Input.GetKeyDown(KeyCode.R))
            transform.position = startPos;

    }
}
