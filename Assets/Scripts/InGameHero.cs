using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHero : MonoBehaviour
{
    public GameObject m_SpawnPoint;
    public GameObject m_DoorDestroyPoint;
    public GameObject m_InHousePoint;
    public GameObject m_LeavePoint;
    public Animator m_Anim;

    public float m_AccelMultiplier = 5.0f;
    public float m_MaxSpeed = 7f;
    public float m_SlowdownDistanceThreshold = 1f;

    private int m_InGameHeroState = 0;

    private Renderer[] m_Renderers;

    [SerializeField] private AudioClip m_KickingClip    = null;
    [SerializeField] private AudioClip m_ChakraClip     = null;
    [SerializeField] private AudioClip m_SlamClip       = null;
    [SerializeField] private AudioClip m_CollectionClip = null;

    private AudioSource m_HeroAS;

    // Start is called before the first frame update

    void OnEnable()
    {
        //subscribe to the event which tells the hero to moves in.
        GlobalEvents.OnPlayerApproachHouse += HandlePlayerApproachHouse;
    }

    void OnDisable()
    {
        //unsubscribe to the event which tells the hero to moves in.
        GlobalEvents.OnPlayerApproachHouse += HandlePlayerApproachHouse;
    }

    void HandlePlayerApproachHouse()
    {
        if (m_InGameHeroState == 0)
        {
            foreach (Renderer r in m_Renderers)
            {
                r.enabled = true;
            }

            m_InGameHeroState = 1;
        }
    }

    void Start()
    {
        //init the hero and teleport him to the spawn point.
        gameObject.transform.position = m_SpawnPoint.transform.position;
        Vector3 forward = m_DoorDestroyPoint.transform.position - m_SpawnPoint.transform.position;
        forward.y = 0;
        forward = forward.normalized;

        gameObject.transform.rotation = Quaternion.LookRotation(forward);
        //disable the visual
        m_Renderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer r in m_Renderers)
        {
            r.enabled = false;
        }
        m_HeroAS = GetComponent<AudioSource>();
        m_InGameHeroState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_InGameHeroState)
        {
            case 0:
                Update_NotActive();
                break;
            case 1:
                Update_WalkToDoor();
                break;
            case 2:
                Update_DestroyDoor();
                break;
            case 3:
                Update_WalkIntoRoom();
                break;
            case 4:
                Update_JudgeRoom();
                break;
            case 5:
                Update_DestroyRoom();
                break;
            case 6:
                Update_Leave();
                break;
        }
    }

    void Update_NotActive()
    {
        //for now instantly enter the house - COMMENT oUT LATER
        /*foreach(Renderer r in m_Renderers)
        {
            r.enabled = true;
        }
        
        m_InGameHeroState = 1;*/
    }

    float m_StateDuration = 0;
    void Update_WalkToDoor()
    {
        bool reached = MoveTowards(m_DoorDestroyPoint.transform.position);
        if(m_Anim.GetBool("Walking") == false)
            m_Anim.SetBool("Walking", true);
        if (reached)
        {
            GlobalEvents.SendPlayerMoveAllowChange(false);
            GlobalEvents.SendPlayerStartDestroyDoor();
            m_StateDuration = 2.5f; //this should be animation time
            m_InGameHeroState = 2;
        }
    }

    float m_MovementSpeed = 0;
    bool MoveTowards(Vector3 position)
    {
        float dist = (gameObject.transform.position - position).magnitude;

        if (dist < m_SlowdownDistanceThreshold)
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, 0.1f, Time.deltaTime * m_AccelMultiplier);
        }
        else
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, m_MaxSpeed, Time.deltaTime * m_AccelMultiplier);
        }
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, position, Time.deltaTime * m_MovementSpeed);
        Vector3 forward = position - gameObject.transform.position;
        if (forward.magnitude > 0.1f)
        {
            forward.y = 0;
            forward = forward.normalized;
            RotateToward(forward);
            //gameObject.transform.rotation = Quaternion.LookRotation(forward);
        }
        return dist < (Time.deltaTime * 2);
    }

    float knockCD;
    void Update_DestroyDoor()
    {
        m_StateDuration -= Time.deltaTime;
        knockCD -= Time.deltaTime;
        RotateToward(Vector3.back);
        if (m_StateDuration > 0 && knockCD < 0)
        {
            int multiplier = Mathf.CeilToInt(m_StateDuration);
            CameraShake.Instance.Shake(0.1f * multiplier, 0.25f / multiplier);
            knockCD = 1;
            if(m_Anim.GetBool("Walking") == true)
                m_Anim.SetBool("Walking", false);
            m_Anim.SetTrigger("Kick");
            m_HeroAS.clip = m_KickingClip;
            m_HeroAS.Play();

        }

        if (m_StateDuration <= 0)
        {
            if(m_Anim.GetBool("Walking") == false)
                m_Anim.SetBool("Walking", true);
            GlobalEvents.SendPlayerDestroyedDoor();
            CameraShake.Instance.Shake(0.3f);
            m_InGameHeroState = 3;
        }
    }

    void Update_WalkIntoRoom()
    {
        bool reached = MoveTowards(m_InHousePoint.transform.position);
        if(m_Anim.GetBool("Walking") == false)
            m_Anim.SetBool("Walking", true);
        if (reached)
        {
            if(m_Anim.GetBool("Walking") == true)
                m_Anim.SetBool("Walking", false);
            if(!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Inspect"))
                m_Anim.SetTrigger("Inspect");
            m_StateDuration = 2.5f; //this should be animation time
            m_InGameHeroState = 4;
        }
    }

    void Update_JudgeRoom()
    {
        m_StateDuration -= Time.deltaTime;

        //look around

        if (m_StateDuration <= 0)
        {
            if(!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Skill"))
                m_Anim.SetTrigger("Skill");
            m_HeroAS.PlayOneShot(m_ChakraClip);
            GlobalEvents.SendPlayerStartDestroyAll(ProgressBarController.Instance.progressionTier); //value from gamestatemanager
            m_StateDuration = 12.5f;
            m_InGameHeroState = 5;
            StartCoroutine(DelayDestroyObjects(7.1f));
            StartCoroutine(DelayCollectCoin(8f));
        }
    }

    IEnumerator DelayDestroyObjects(float delay) {
        yield return new WaitForSeconds(delay);
        GlobalEvents.SendPlayerDestroyedAll(1);
    }

    IEnumerator DelayCollectCoin(float delay) {
        yield return new WaitForSeconds(delay);
        GlobalEvents.SendCollectCoins();
    }

    void Update_DestroyRoom()
    {
        m_StateDuration -= Time.deltaTime;
        //look back
        RotateToward(Vector3.back);

        if (m_StateDuration <= 0)
        {
            // GlobalEvents.SendPlayerDestroyedAll(1);
            m_StateDuration = 3.0f;
            m_InGameHeroState = 6;
        }
    }
    float m_RotateSpeed = 5;

    void RotateToward(Vector3 dir)
    {
        Vector3 newForward = Vector3.RotateTowards(gameObject.transform.forward, dir, Time.deltaTime * m_RotateSpeed, Time.deltaTime * m_RotateSpeed);
        newForward.y = 0;
        gameObject.transform.rotation = Quaternion.LookRotation(newForward);
    }

    void Update_Leave()
    {
        bool reached = MoveTowards(m_LeavePoint.transform.position);
        if(m_Anim.GetBool("Walking") == false)
            m_Anim.SetBool("Walking", true);
        if (reached)
        {
            foreach (Renderer r in m_Renderers)
            {
                r.enabled = false;
            }
            gameObject.transform.position = m_SpawnPoint.transform.position;

            if(m_Anim.GetBool("Walking") == true)
                m_Anim.SetBool("Walking", false);
            GlobalEvents.SendPlayerLeaveComplete(1);
            StartCoroutine(DelayEnablePlayerMove(2.8f));
            m_StateDuration = 3.0f; //this should be animation time
            m_InGameHeroState = 0;
        }
    }

    IEnumerator DelayEnablePlayerMove(float delay) {
        yield return new WaitForSeconds(delay);
        GlobalEvents.SendPlayerMoveAllowChange(true);
    }

    Camera m_MainCam;
    void OnGUI()
    {
        /*if (m_MainCam == null)
        {
            m_MainCam = Camera.main;
        }
        Vector3 screenPos = m_MainCam.WorldToScreenPoint(gameObject.transform.position);
        Rect displayRect = new Rect(screenPos.x, screenPos.y, 100, 1000);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        switch (m_InGameHeroState)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                GUI.Label(displayRect, "BREAKING DOOR: " + m_StateDuration, style);
                break;
            case 3:
                break;
            case 4:
                GUI.Label(displayRect, "JUDGING ROOM: " + m_StateDuration, style);
                break;
            case 5:
                GUI.Label(displayRect, "DESTROY ROOM: " + m_StateDuration, style);
                break;
            case 6:
                break;
        }*/
    }
}
