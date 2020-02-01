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
            if(someEntity != null)
                RejectObject(someEntity.gameObject);
        } else {
            // Check if CraftMat Match
            if (craftMat.currCraftMatType == CraftMatType.Clay)
            {
                StartCoroutine(CraftingCoroutine());
                Destroy(craftMat.gameObject);
            } else {
                RejectObject(craftMat.gameObject);
            }
        }

        
    }

    private IEnumerator CraftingCoroutine()
    {
        m_BurnTimer = m_Stats.m_FurnaceRepairTime;
        yield return new WaitForSeconds(m_Stats.m_FurnaceRepairTime);
        CraftDoneFeedback();
    }

    private void CraftDoneFeedback()
    {
        // Instantiate Pot
        GameObject unfiredPot = Instantiate(prefabPotUnfired, transform.position, transform.rotation);
        // Off Collider #IMPT to avoid infinte loop
        unfiredPot.GetComponent<Entity>().SetMeshColliders(false);

        ShootOutObject(unfiredPot);

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
