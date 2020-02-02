using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeroCoinCollector : MonoBehaviour
{
    public static HeroCoinCollector m_Instance;
    public Transform m_CollectPoint = null;
    public List<Transform> m_Coins = new List<Transform>();

    private void OnEnable() {
        if(m_Instance == null)
            m_Instance = this;

        GlobalEvents.OnCollectCoins +=  delegate{ CollectAllCoins(); };
    }

    private void OnDisable() {
        GlobalEvents.OnCollectCoins -=  delegate{ CollectAllCoins(); };
    }

    private void Start() {
        if(m_Instance != this)
            Destroy(this.gameObject);
    }

    public void CollectAllCoins() {
        foreach(Transform t in m_Coins)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(Random.Range(0.5f, 2.5f));
            seq.Append(t.DOLocalMove(m_CollectPoint.transform.position, Random.Range(1f, 1.5f)).SetEase(Ease.InQuad));
            seq.Append(t.DOScale(Vector3.one * 0.05f, 0.5f));
        }
        StartCoroutine(ClearCoins());
    }

    IEnumerator ClearCoins() {
        yield return new WaitForSeconds(10);
        foreach(Transform t in m_Coins)
            Destroy(t.gameObject);
        m_Coins.Clear();
    }
}
