using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public TextMeshProUGUI m_Score = null;

    private void OnEnable() {
        GlobalEvents.OnPlayerStartDestroyAll += delegate { DisplayScore(); };
    }

    
    private void OnDisable() {
        GlobalEvents.OnPlayerStartDestroyAll -= delegate { DisplayScore(); };
    }

    // return 0 ~ 100
    public float CalculateScore() {
        ScoreTester[] tester = GameObject.FindObjectsOfType<ScoreTester>();
        float score = 0;
        foreach(ScoreTester t in tester) {
            score += t.CalculateScore();
        }
        return score / tester.Length;
    }

    public void DisplayScore() {
        float score = CalculateScore();
        float currScore = 0;
        m_Score.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(()=>{m_Score.DOFade(1, 0.5f);});
        seq.AppendCallback(()=>{DOTween.To(()=> currScore, x=> {
            currScore = x;
            m_Score.text = "SCORE:  " + (currScore * 100).ToString("F1");
            }, score, 3f);});
        seq.AppendInterval(3f);
        seq.Append(m_Score.transform.DOPunchScale(Vector3.one, 0.2f));
        seq.AppendInterval(1f);
        seq.Append(m_Score.DOFade(0, 0.6f));
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.H))
        DisplayScore();
    }
}
