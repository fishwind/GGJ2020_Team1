using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ProgressState
{
    start = 0,
    questing = 1,
    returning = 2,
    end = 3
}

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
    }

    private void OnDestroy()
    {
        GlobalEvents.OnRepairGameplayStart -= UpdateTimer;
        GlobalEvents.OnRepairReturnDuration -= UpdateTimer;
    }

    private void Update()
    {
        if (state == ProgressState.start &&
            GameStateManager.Instance.m_GameState == GameState.REPAIR)
        {
            state = ProgressState.questing;
            StartProgressBar(endAnchor.localPosition);
        }
    }

    private void ResetProgressBar()
    {
        state = ProgressState.start;
    }

    private void StartProgressBar(Vector3 destination)
    {
        heroProgress.DOLocalMove(destination, gameTime).OnComplete(ReachedEnd);
    }

    private ProgressState state = 0;
    private void ReachedEnd()
    {
        GlobalEvents.SendHeroQuestComplete();

        rotation *= -1;
        heroImage.transform.DOScaleX(rotation, 0.3f);

        if (state > ProgressState.start && state < ProgressState.returning)
        {
            state = ProgressState.returning;
            StartProgressBar(startAnchor.localPosition);
        }
        else
        {
            GlobalEvents.SendHeroReturningToHouse();

            state = ProgressState.end;

            ResetProgressBar();
            // disable hero anim?
        }
    }

    private void UpdateTimer(float timer)
    {
        gameTime = timer;

        progressBarPos = heroProgress.localPosition;
        progressBarPos.y = 0;
    }

}
