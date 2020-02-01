using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;

    public List<Button> buttons;

    [Space(10)]
    public GameObject optionsPrefab;
    private GameObject optionsObj;

    private bool isOptionsDisplayed = false;
    public bool IsOptionsDisplayed { get { return isOptionsDisplayed; } }

    [Header("Tween Variables")]
    public float tweenDuration = 1f;
    public AnimationCurve tweenCurve;
    private float offscreenPos = -2500f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        RegisterMenuButtons();

        if (optionsObj == null)
        {
            optionsObj = Instantiate(optionsPrefab, this.transform);
            optionsObj.transform.localPosition = new Vector3(0, offscreenPos);
            optionsObj.transform.localRotation = Quaternion.identity;
            optionsObj.transform.localScale = Vector3.one;
        }
    }

    private void OnDestroy()
    {
        UnregisterMenuButtons();
    }

    public void RegisterMenuButtons()
    {
        buttons[0].onClick.AddListener(StartGame);
        buttons[1].onClick.AddListener(Options);
        buttons[2].onClick.AddListener(QuitGame);
    }

    public void UnregisterMenuButtons()
    {
        for (int i = 0; i < buttons.Count; ++i)
            buttons[i].onClick.RemoveAllListeners();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        isOptionsDisplayed = !isOptionsDisplayed;
        ShowOptions(isOptionsDisplayed);
    }

    private void ShowOptions(bool isShow)
    {
        optionsObj.transform.DOLocalMoveY(isShow ? 0 : offscreenPos, tweenDuration).SetEase(tweenCurve);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Update()
    {

    }

}
