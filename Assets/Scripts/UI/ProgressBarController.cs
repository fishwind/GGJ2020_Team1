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
    public static ProgressBarController Instance;

    public int progressionTier = 1;
    public float tweenDuration = 0.3f;

    [Header("Background")]
    public Image barBG;
    public Sprite[] BGSprites;

    [Header("Terrain")]
    public TerrainController terrainController;

    [Header("Hero")]
    public Transform heroProgress;
    private Image heroImage;
    private Animator heroAnim;
    private Vector3 progressBarPos;

    public Transform startAnchor;
    public Transform endAnchor;

    [Header("Obstacles")]
    public ProgressEventController eventController;

    private float gameTime;
    private int rotation = 1;
    private ProgressState state = 0;

    private void Awake()
    {
        if (ProgressBarController.Instance != null)
        {
            Destroy(this);
        }
        Instance = this;

        heroImage = heroProgress.GetComponentInChildren<Image>();
        heroAnim = heroImage.transform.GetComponent<Animator>();
        progressBarPos = startAnchor.localPosition;

        GlobalEvents.OnRepairGameplayStart += UpdateTimer;
        GlobalEvents.OnRepairReturnDuration += UpdateReturnTimer;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnRepairGameplayStart -= UpdateTimer;
        GlobalEvents.OnRepairReturnDuration -= UpdateReturnTimer;
    }

    private void Update()
    {
        if (state == ProgressState.start &&
            GameStateManager.Instance.m_GameState == GameState.REPAIR)
        {
            terrainController.scrollingTerrain = true;
            eventController.SpawnRandomEvents(heroProgress);
            eventController.ShowEvent();

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
        heroAnim.SetBool("startWalk", true);
        heroProgress.DOLocalMove(destination, gameTime).OnComplete(ReachedEnd);
    }

    private void ReachedEnd()
    {
        if (state > ProgressState.start && state < ProgressState.returning)
        {
            GlobalEvents.SendHeroQuestComplete();

            state = ProgressState.returning;
            StartProgressBar(startAnchor.localPosition);
        }
        else
        {
            GlobalEvents.SendHeroReturningToHouse();

            state = ProgressState.end;

            ResetProgressBar();
            gameTime = m_ReturnTime;
            heroAnim.SetBool("startWalk", false);
            terrainController.scrollingTerrain = false;
            eventController.ResetEvent();
            progressionTier++;
        }

        HandleFlip();
    }

    private void HandleFlip()
    {
        rotation *= -1;

        barBG.overrideSprite = BGSprites[state == ProgressState.returning ? 1 : 0];
        barBG.transform.DOScaleX(rotation, tweenDuration * 0.5f);

        heroImage.transform.DOScaleX(rotation, tweenDuration);
    }

    private void UpdateTimer(float timer)
    {
        gameTime = timer;

        progressBarPos = heroProgress.localPosition;
        progressBarPos.y = 0;
    }

    float m_ReturnTime;
    void UpdateReturnTimer(float timer)
    {
        m_ReturnTime = timer;
    }

}
