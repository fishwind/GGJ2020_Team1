using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class PotteryWheel : MonoBehaviour
{
    public RepairMachineStats m_Stats = null;
    public AudioClip m_PotDoneClip = null;
    public AudioClip m_BurningClip = null;
    public AudioClip m_MachineRejectClip = null;
    public Transform m_SpawnPoint;
    public ParticleSystem m_BurnParticles = null;

    private float m_BurnTimer = 0f;
    private Transform m_Pot = null;
    private AudioSource m_Asource = null;

    public GameObject prefabPotUnfired;
    public GameObject prefabCrate;

    [Header("Material counter")]
    public int requiredClay = 1;
    public int clayCount = 0;
    public int requiredWood = 1;
    public int woodCount = 0;

    private void Start()
    {
        m_Asource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_BurnTimer -= Time.deltaTime;
        if (m_BurnParticles.isPlaying && m_BurnTimer <= 0)
        {
            m_BurnParticles.Stop();
            m_Asource.Stop();
        }
        else if (!m_BurnParticles.isPlaying && m_BurnTimer > 0)
        {
            m_BurnParticles.Play();
            m_Asource.clip = m_BurningClip;
            m_Asource.Play();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        CraftMat craftMat = col.gameObject.GetComponentInParent<CraftMat>();
        if (!craftMat || craftMat.currItemState != ItemStates.CraftMat)
        {
            Entity someEntity = col.gameObject.GetComponentInParent<Entity>();
            if (someEntity != null)
                RejectObject(someEntity.gameObject);
        }
        else
        {
            // Check if CraftMat Match
            if (craftMat.currCraftMatType == CraftMatType.Clay)
            {
                clayCount++;

                if (ReadyToCraft(CraftMatType.Clay))
                    StartCoroutine(CraftingCoroutine(CraftMatType.Clay));

                Feedback();
                Destroy(craftMat.gameObject);
            }
            else if (craftMat.currCraftMatType == CraftMatType.Wood)
            {
                woodCount++;

                if (ReadyToCraft(CraftMatType.Wood))
                    StartCoroutine(CraftingCoroutine(CraftMatType.Wood));

                Feedback();
                Destroy(craftMat.gameObject);
            }
            else
            {
                RejectObject(craftMat.gameObject);
            }
        }

    }


    private bool ReadyToCraft(CraftMatType type)
    {
        switch (type)
        {
            case CraftMatType.Clay:
                return clayCount >= requiredClay;

            case CraftMatType.Wood:
                return woodCount >= requiredWood;

            default:
                return false;
        }
    }

    private IEnumerator CraftingCoroutine(CraftMatType type)
    {
        m_BurnTimer = m_Stats.m_FurnaceRepairTime;

        // Reset after craft
        if (type == CraftMatType.Clay) clayCount = 0;
        if (type == CraftMatType.Wood) woodCount = 0;

        Feedback();
        yield return new WaitForSeconds(m_Stats.m_FurnaceRepairTime);
        CraftDoneFeedback(type);
    }

    private void Feedback()
    {
        transform.DOShakeRotation(m_Stats.m_FurnaceRepairTime);
    }

    private void CraftDoneFeedback(CraftMatType type)
    {
        if (type == CraftMatType.Clay)
            CreatePot();

        if (type == CraftMatType.Wood)
            CreateCrate();
    }

    private void CreatePot()
    {
        // Instantiate Pot
        GameObject unfiredPot = Instantiate(prefabPotUnfired, transform.position, transform.rotation);
        // Off Collider #IMPT to avoid infinte loop
        unfiredPot.GetComponent<Entity>().SetMeshColliders(false);

        ShootOutObject(unfiredPot);

        m_Asource.PlayOneShot(m_PotDoneClip);
    }

    private void CreateCrate()
    {
        // Instantiate Pot
        GameObject crateObj = Instantiate(prefabCrate, transform.position, transform.rotation);
        // Off Collider #IMPT to avoid infinte loop
        crateObj.GetComponent<Entity>().SetMeshColliders(false);

        ShootOutObject(crateObj);

        m_Asource.PlayOneShot(m_PotDoneClip);
    }

    private void RejectObject(GameObject rejectObject)
    {
        // Off Collider #IMPT to avoid infinte loop
        rejectObject.GetComponent<Entity>().SetMeshColliders(false);

        ShootOutObject(rejectObject);

        m_Asource.PlayOneShot(m_MachineRejectClip);
    }

    private void ShootOutObject(GameObject thrownObject)
    {
        float scale = thrownObject.transform.localScale.x;
        thrownObject.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(thrownObject.transform.DOMove(transform.position + Vector3.up + transform.forward, 0.4f));
        seq.Join(thrownObject.transform.DOScale(Vector3.one * scale, 0.4f));
        seq.Append(thrownObject.transform.DOMove(m_SpawnPoint.position, 0.6f));
        seq.AppendCallback(() => { thrownObject.GetComponent<Entity>().SetMeshColliders(true); });
    }
}
