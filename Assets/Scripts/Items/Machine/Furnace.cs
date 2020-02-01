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
    public AudioClip m_MachineRejectClip = null;
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
        if (!pot) {
            Entity someEntity = col.gameObject.GetComponentInParent<Entity>();
            if (someEntity != null)
                RejectObject(someEntity.gameObject);
        } else {
            IFireable ifire = (IFireable)pot.GetComponent(typeof(IFireable));
            if (ifire != null && ifire.CheckIfFireable())
            {
                StartCoroutine(FiringPot(pot));
            } else {
                RejectObject(pot.gameObject);
            }
        }
        
    }

    private IEnumerator FiringPot(Pot pot) {
        m_BurnTimer = m_Stats.m_FurnaceRepairTime;
        pot.transform.DOScale(Vector3.zero, 0.4f);
        transform.DOShakeRotation(m_Stats.m_FurnaceRepairTime);
        yield return new WaitForSeconds(m_Stats.m_FurnaceRepairTime);
        pot.CompleteFiring();
        PotDoneFeedback(pot); 
    }

    private void PotDoneFeedback(Pot pot) {
        // Off Collider #IMPT to avoid infinte loop
        pot.GetComponent<Entity>().SetMeshColliders(false);

        ShootOutObject(pot.gameObject, 1.0f);

        m_Asource.PlayOneShot(m_PotDoneClip);
    }

    private void RejectObject(GameObject rejectObject)
    {
        // Off Collider #IMPT to avoid infinte loop
        rejectObject.GetComponent<Entity>().SetMeshColliders(false);

        float scale = rejectObject.transform.localScale.x;
        rejectObject.transform.localScale = Vector3.zero;
        ShootOutObject(rejectObject, scale);

        m_Asource.PlayOneShot(m_MachineRejectClip);
    }

    private void ShootOutObject(GameObject thrownObject, float scale)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(thrownObject.transform.DOMove(transform.position + Vector3.up + transform.forward, 0.4f));
        seq.Join(thrownObject.transform.DOScale(Vector3.one * scale, 0.4f));
        seq.Append(thrownObject.transform.DOMove(m_SpawnPoint.position, 0.6f));
        seq.AppendCallback(() => { thrownObject.GetComponent<Entity>().SetMeshColliders(true);} );
    }
}
