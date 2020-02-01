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
        if (!craftMat || craftMat.currItemState != ItemStates.CraftMat) return;

        // TODO: Recipes
        if (craftMat.currCraftMatType == CraftMatType.Clay)
        {
            StartCoroutine(CraftingCoroutine());
            Destroy(craftMat.gameObject);
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
        float scale = unfiredPot.transform.localScale.x;
        unfiredPot.transform.localScale = Vector3.zero;
        // throw out pot todo:
        Sequence seq = DOTween.Sequence();
        seq.Append(unfiredPot.transform.DOMove(transform.position + Vector3.up + transform.forward, 0.4f));
        seq.Join(unfiredPot.transform.DOScale(Vector3.one * scale, 0.4f));
        seq.Append(unfiredPot.transform.DOMove(m_SpawnPoint.position, 0.6f));
        m_Asource.PlayOneShot(m_PotDoneClip);
    }
}
