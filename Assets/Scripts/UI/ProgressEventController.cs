using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressEventController : MonoBehaviour
{
    public List<Transform> eventObjs;
    private List<Image> eventImgs = new List<Image>();
    public List<Sprite> eventSprites;

    private Transform heroObj;
    private Animator heroAnim;

    public int numSpawned;
    private int currCounter;
    private int prevCounter;

    public float onscreenPosY = -30f;
    public float offscreenPosY = -500f;

    public AnimationCurve tweenCurve;
    private float tweenDuration = 0.3f;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        for (int i = 0; i < eventObjs.Count; ++i)
        {
            eventImgs.Add(eventObjs[i].GetComponentInChildren<Image>());
        }
    }

    public void ResetEvent()
    {
        prevCounter = -1;
        currCounter = 0;

        for (int i = 0; i < eventObjs.Count; ++i)
        {
            eventObjs[i].position = new Vector3(0, offscreenPosY);
            eventObjs[i].gameObject.SetActive(false);
        }
    }

    public void SpawnRandomEvents(Transform hero)
    {
        heroObj = hero;
        heroAnim = hero.GetComponentInChildren<Animator>();

        float x = heroObj.transform.localPosition.x;
        float maxX = 450;
        numSpawned = Random.Range(3, eventObjs.Count);
        float y = Mathf.Abs(maxX - x) / (numSpawned);

        for (int i = 0; i < numSpawned; ++i)
        {
            eventImgs[i].overrideSprite = eventSprites[Random.Range(0, eventSprites.Count)];
            eventObjs[i].localPosition = new Vector3(x + (i + 1) * y, offscreenPosY);
            eventObjs[i].gameObject.SetActive(true);
        }

        prevCounter = -1;
    }

    public void ShowEvent()
    {
        if (prevCounter != -1 && prevCounter != currCounter)
            EventDie();

        prevCounter = currCounter;

        if (currCounter + 1 < eventObjs.Count)
            eventObjs[currCounter++].DOLocalMoveY(onscreenPosY, tweenDuration);
    }

    private void Update()
    {
        if (heroObj != null)
        {
            if (prevCounter >= 0 &&
                heroObj.transform.position.x >= eventObjs[prevCounter].position.x)
            {
                ShowEvent();
            }
        }
    }

    private void EventDie()
    {
        int i = prevCounter;
        var pos = eventObjs[i].position;

        eventObjs[i].DOScale(Vector3.one * 2, tweenDuration / 2).SetEase(tweenCurve)
            .OnComplete(() => eventObjs[i].DOScale(Vector3.one, tweenDuration / 2));

        eventObjs[i].DOLocalMoveY(onscreenPosY + 10, tweenDuration / 2)
            .OnComplete(() => eventObjs[i].DOLocalMoveY(offscreenPosY, tweenDuration));

        CameraShake.Instance.Shake(0.1f, 0.1f);

        heroAnim.SetBool("startAtk", true);

        if (c != null)
            StopCoroutine(c);
        c = StartCoroutine(resetAnim());
    }

    Coroutine c;
    IEnumerator resetAnim()
    {
        yield return new WaitForSeconds(1);
        heroAnim.SetBool("startAtk", false);
    }

}
