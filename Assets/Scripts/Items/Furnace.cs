using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class Furnace : MonoBehaviour
{
    public RepairMachineStats m_Stats = null;
    public AudioClip m_PotDoneClip = null;
    public Transform m_SpawnPoint;

    private Transform m_Pot = null;
    private AudioSource m_Asource = null;

    private void Start() {
        m_Asource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col) {
        Pot pot = col.gameObject.GetComponent<Pot>();
        if(!pot) return;
        IFireable ifire = (IFireable)pot.GetComponent(typeof(IFireable));
        if(ifire && ifire.CheckIfFireable()) {
            StartCoroutine(FiringPot(pot));
        }
    }

    private IEnumerator FiringPot(Pot pot) {
        yield return new WaitForSeconds(m_Stats.m_FurnaceRepairTime);
        pot.CompleteFiring();
        PotDoneFeedback(pot); 
    }

    private void PotDoneFeedback(Pot pot) {
        // throw out pot todo:
        Sequence seq = DOTween.Sequence();
        seq.Append(pot.transform.DOPunchPosition(transform.position + Vector3.up, 0.2f, 0, 0));
        seq.Append(pot.transform.DOMove(m_SpawnPoint.position, 0.6f));
        m_Asource.PlayOneShot(m_PotDoneClip);
    }
}
