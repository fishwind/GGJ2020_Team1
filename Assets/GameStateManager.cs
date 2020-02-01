using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    INIT,
    HERO,
    REPAIR,
    ANNOUNCEMENT_HERO,
    ANNOUNCEMENT_REPAIR,
    //ANNOUNCEMENT_RETURN,
    //ANNOUNCEMENT_PASS,
    //ANNOUNCEMENT_FAIL,
    ANNOUNCEMENT_GAMEOVER
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public GameState m_GameState = GameState.INIT;

    public float m_HeroAnnouncementDuration = 1.0f;
    public float m_RepairAnnouncementDuration = 2.0f;

    public float m_RepairGameDuration = 90f;
    public float m_ReturnGameDuration = 30f;
    private float m_CurrStateDuration = 0;

    public GameObject m_TextController;
    public GameObject m_BlackText;
    public GameObject m_WhiteText;

    public AnimationCurve m_TextAppearCurve;

    void Awake()
    {
        if (GameStateManager.Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        m_BText = m_BlackText.GetComponent<Text>();
        m_WText = m_WhiteText.GetComponent<Text>();
    }

    void OnEnable()
    {
        GlobalEvents.OnHeroReturningToHouse += HandleHeroReturnToHouseAnnouncement;
        GlobalEvents.OnPlayerApproachHouse += HandleHeroApproachHouse;
        GlobalEvents.OnPlayerLeaveComplete += HandleHeroLeft;
        GlobalEvents.OnHeroQuestComplete += HandleHeroQuestComplete;
    }

    void OnDisable()
    {
        GlobalEvents.OnHeroReturningToHouse -= HandleHeroReturnToHouseAnnouncement;
        GlobalEvents.OnPlayerApproachHouse -= HandleHeroApproachHouse;
        GlobalEvents.OnPlayerLeaveComplete -= HandleHeroLeft;
        GlobalEvents.OnHeroQuestComplete -= HandleHeroQuestComplete;
    }
    void HandleHeroApproachHouse()
    {
        m_GameState = GameState.HERO;
    }
    void HandleHeroLeft(int val)
    {
        m_GameState = GameState.ANNOUNCEMENT_REPAIR;
        m_CurrStateDuration = m_RepairAnnouncementDuration;
    }

    float m_CurrReturnAnnouncementDuration = 0f;
    public float m_ReturnAnnouncementDuration = 4f;
    void HandleHeroQuestComplete()
    {
        //announce the hero is returning?
        m_CurrReturnAnnouncementDuration = m_ReturnAnnouncementDuration;
    }

    void Start()
    {
        HandleHeroReturnToHouseAnnouncement();
    }

    void HandleHeroReturnToHouseAnnouncement()
    {
        //init the game.... HERO IS COMING UI TEXT
        m_GameState = GameState.ANNOUNCEMENT_HERO;
        m_CurrStateDuration = m_HeroAnnouncementDuration;
        //play announce hero audio
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (m_CurrReturnAnnouncementDuration > 0)
        {
            m_CurrReturnAnnouncementDuration -= Time.deltaTime;
            if (m_CurrReturnAnnouncementDuration <= 0)
            {
                m_CurrReturnAnnouncementDuration = 0;
            }
        }
        switch (m_GameState)
        {
            case GameState.ANNOUNCEMENT_HERO:
                Update_AnnounceHero();
                break;
            case GameState.HERO:
                Update_HeroLoop();
                break;
            case GameState.ANNOUNCEMENT_REPAIR:
                Update_AnnounceRepair();
                break;
            case GameState.REPAIR:
                Update_RepairLoop();
                break;
            //case GameState.ANNOUNCEMENT_RETURN:
            //break;
            case GameState.ANNOUNCEMENT_GAMEOVER:
                Update_GameOver();
                break;
        }
    }

    void Update_AnnounceHero()
    {
        //wait for announcement duration to end..
        m_CurrStateDuration -= Time.deltaTime;
        if (m_CurrStateDuration <= 0)
        {
            GlobalEvents.SendPlayerApproachHouse();
        }
    }

    void Update_HeroLoop()
    {
        //nada..
    }

    void Update_AnnounceRepair()
    {
        //wait for announcement duration to end..
        m_CurrStateDuration -= Time.deltaTime;
        if (m_CurrStateDuration <= 0)
        {
            GlobalEvents.SendRepairGameplayStart(m_RepairGameDuration);
            GlobalEvents.SendRepairReturnDuration(m_ReturnGameDuration);
            m_GameState = GameState.REPAIR;
        }
    }

    void Update_RepairLoop()
    {
        //nada?
    }

    void Update_GameOver()
    {
        //todo:
    }

    Text m_BText;
    Text m_WText;
    float m_ScaleX = 1;
    float m_ScaleY = 1;

    void ChangeScale(float scaleValue)
    {
        m_ScaleX = scaleValue;
        m_ScaleY = scaleValue;

        m_BlackText.transform.localScale = new Vector2(m_ScaleX, m_ScaleY);
        m_WhiteText.transform.localScale = new Vector2(m_ScaleX, m_ScaleY);
    }
    void UpdateUI()
    {

        if (m_GameState == GameState.ANNOUNCEMENT_HERO)
        {
            m_TextController.SetActive(true);
            m_BText.text = "Our great hero approaches!";
            m_WText.text = "Our great hero approaches!";
            ChangeScale(m_TextAppearCurve.Evaluate(m_HeroAnnouncementDuration - m_CurrStateDuration));
        }
        else if (m_GameState == GameState.ANNOUNCEMENT_REPAIR)
        {
            m_TextController.SetActive(true);
            m_BText.text = "Ready! Set! Repair!";
            m_WText.text = "Ready! Set! Repair!";
            ChangeScale(m_TextAppearCurve.Evaluate(m_RepairAnnouncementDuration - m_CurrStateDuration));
        }
        else if (m_CurrReturnAnnouncementDuration > 0)
        {
            m_TextController.SetActive(true);
            m_BText.text = "The hero is returning!";
            m_WText.text = "The hero is returning!";
            ChangeScale(m_TextAppearCurve.Evaluate(m_ReturnAnnouncementDuration - m_CurrReturnAnnouncementDuration));
        }
        else
        {
            m_TextController.SetActive(false);
            m_ScaleX = m_ScaleY = 1;
            m_BlackText.transform.localScale = new Vector2(m_ScaleX, m_ScaleY);
            m_WhiteText.transform.localScale = new Vector2(m_ScaleX, m_ScaleY);
        }
    }
}
