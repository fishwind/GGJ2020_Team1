using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHero : MonoBehaviour
{
    public GameObject m_SpawnPoint;
    public GameObject m_DoorDestroyPoint;
    public GameObject m_InHousePoint;
    public GameObject m_LeavePoint;

    private int m_InGameHeroState = 0;

    private Renderer[] m_Renderers;
    // Start is called before the first frame update

    void OnEnable()
    {
        //subscribe to the event which tells the hero to moves in.
    }

    void OnDisable()
    {
        //unsubscribe to the event which tells the hero to moves in.
    }

    void Start()
    {
        //init the hero and teleport him to the spawn point.
        gameObject.transform.position = m_SpawnPoint.transform.position;
        Vector3 forward = m_DoorDestroyPoint.transform.position - m_SpawnPoint.transform.position;

        //disable the visual
        m_Renderers = GetComponentsInChildren<Renderer>(true);

        foreach(Renderer r in m_Renderers)
        {
            r.enabled = false;
        }

        m_InGameHeroState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_InGameHeroState)
        {
            case 0: Update_NotActive();
            break;
            case 1: Update_WalkToDoor();
            break;
            case 2: Update_DestroyDoor();
            break;
            case 3: Update_WalkIntoRoom();
            break;
            case 4: Update_JudgeRoom();
            break;
            case 5: Update_DestroyRoom();
            break;
            case 6: Update_Leave();
            break;
        }
    }

    void Update_NotActive()
    {
        //for now instantly enter the house
        foreach(Renderer r in m_Renderers)
        {
            r.enabled = true;
        }

        m_InGameHeroState = 1;
    }

    void Update_WalkToDoor()
    {

    }

    void Update_DestroyDoor()
    {

    }

    void Update_WalkIntoRoom()
    {

    }

    void Update_JudgeRoom()
    {

    }

    void Update_DestroyRoom()
    {

    }

    void Update_Leave()
    {

    }
}
