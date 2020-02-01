using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float m_RepairGameDuration = 300f;
    private float m_CurrStateDuration = 0;
    void Awake()
    {
        if(GameStateManager.Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
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

    float m_CurrReturnAnnouncementDuration = 4f;
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
        if(m_CurrReturnAnnouncementDuration > 0)
        {
            m_CurrReturnAnnouncementDuration -= Time.deltaTime;
            if(m_CurrReturnAnnouncementDuration <= 0)
            {
                m_CurrReturnAnnouncementDuration = 0;
            }
        }
        switch(m_GameState)
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
        if(m_CurrStateDuration <= 0)
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
        if(m_CurrStateDuration <= 0)
        {
            GlobalEvents.SendRepairGameplayStart(m_RepairGameDuration);
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

    void OnGUI()
    {

    }
}
