using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class Furnace : MonoBehaviour
{
    public RepairMachineStats m_Stats = null;
    public AudioClip m_PotDoneClip = null;
    public AudioClip m_BurningClip = null;
    public Transform m_SpawnPoint;
    public ParticleSystem m_BurnParticles = null;

    private float m_BurnTimer = 0f;
    private Transform m_Pot = null;
    private AudioSource m_Asource = null;

    private void Start() {
        m_Asource = GetComponent<AudioSource>();
    }

    private void Update() {
        m_BurnTimer -= Time.deltaTime;
        if(m_BurnParticles.isPlaying && m_BurnTimer <= 0) {
            m_BurnParticles.Stop();
            m_Asource.Stop();
        }
        else if(!m_BurnParticles.isPlaying && m_BurnTimer > 0) {
            m_BurnParticles.Play();
            m_Asource.clip = m_BurningClip;
            m_Asource.Play();
        }
    }

    private void OnTriggerEnter(Collider col) {
        Pot pot = col.gameObject.GetComponentInParent<Pot>();
        if(!pot) return;
        IFireable ifire = (IFireable)pot.GetComponent(typeof(IFireable));
        if(ifire != null && ifire.CheckIfFireable()) {
            StartCoroutine(FiringPot(pot));
        }
    }

    private IEnumerator FiringPot(Pot pot) {
        m_BurnTimer = m_Stats.m_FurnaceRepairTime;
        pot.transform.DOScale(Vector3.zero, 0.4f);
        yield return new WaitForSeconds(m_Stats.m_FurnaceRepairTime);
        pot.CompleteFiring();
        PotDoneFeedback(pot); 
    }

    private void PotDoneFeedback(Pot pot) {
        // throw out pot todo:
        Sequence seq = DOTween.Sequence();
        seq.Append(pot.transform.DOMove(transform.position + Vector3.up + transform.forward, 0.4f));
        seq.Join(pot.transform.DOScale(Vector3.one, 0.4f));
        seq.Append(pot.transform.DOMove(m_SpawnPoint.position, 0.6f));
        m_Asource.PlayOneShot(m_PotDoneClip);
    }
}
