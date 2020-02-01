using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBarController : MonoBehaviour
{
    public Transform heroProgress;
    private Image heroImage;
    private Animator heroAnim;
    private Vector3 progressBarPos;

    public Transform startAnchor;
    public Transform endAnchor;
    public List<Transform> eventAnchors;

    private float gameTime;
    private int rotation = 1;

    private void Awake()
    {
        heroImage = heroProgress.GetComponentInChildren<Image>();
        progressBarPos = startAnchor.localPosition;

        GlobalEvents.OnRepairGameplayStart += UpdateTimer;
        GlobalEvents.OnRepairReturnDuration += UpdateTimer;

        gameTime = 10;
        StartProgressBar(endAnchor.localPosition);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            heroProgress.localPosition = startAnchor.localPosition;
            UpdateTimer(Random.Range(5, 10));
        }
    }

    private void ResetProgressBar()
    {
        Count = 0;
    }

    private void StartProgressBar(Vector3 destination)
    {
        Count++;
        heroProgress.DOLocalMove(destination, gameTime).OnComplete(ReachedEnd);
    }

    private int Count = 0;
    private void ReachedEnd()
    {
        GlobalEvents.SendHeroQuestComplete();

        rotation *= -1;
        heroImage.transform.DOScaleX(rotation, 0.3f);

        if (Count > 0 && Count < 2)
            StartProgressBar(startAnchor.localPosition);
    }

    private void UpdateTimer(float timer)
    {
        gameTime = timer;

        progressBarPos = heroProgress.localPosition;
        progressBarPos.y = 0;
    }

}
