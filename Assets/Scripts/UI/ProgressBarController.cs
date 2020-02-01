using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Transform progressBar;
    private Vector3 progressStartPos;
    private Vector3 progressBarPos;
    public Transform eventAnchor;

    private float currTime;
    private float gameTime;

    private void Awake()
    {
        progressStartPos = progressBar.localPosition;
        UpdateTimer(5);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            progressBar.localPosition = progressStartPos;
            UpdateTimer(Random.Range(5, 10));
        }

        if (currTime < gameTime)
            currTime += Time.deltaTime;
        else currTime = gameTime;

        if (gameTime < float.Epsilon) return;
        progressBar.localPosition = Vector3.Lerp(progressBarPos, eventAnchor.localPosition, currTime / gameTime);
    }

    private void UpdateTimer(float timer)
    {
        gameTime = timer;
        currTime = 0;

        progressBarPos = progressBar.localPosition;
        progressBarPos.y = 0;
    }

}
