using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationEvent : MonoBehaviour
{
    private AudioSource m_Asource = null;
    public AudioClip m_FootstepClip1 = null;
    public AudioClip m_FootstepClip2 = null;
    public AudioClip m_Battlecry = null;
    public AudioClip m_Flying = null;
    public AudioClip m_Attack = null;
    public AudioClip m_Slam = null;

    private void Start() {
        m_Asource = GetComponent<AudioSource>();
        if(!m_Asource)
            m_Asource = this.gameObject.AddComponent<AudioSource>();
    }

    private void PlayFootstep1() {
        m_Asource.PlayOneShot(m_FootstepClip1);
    }

    private void PlayFootstep2() {
        m_Asource.PlayOneShot(m_FootstepClip2);
    }

    private void PlayChakra()
    {
        m_Asource.PlayOneShot(m_Battlecry);
    }

    private void PlayFlying()
    {
        m_Asource.PlayOneShot(m_Flying);
    }

    private void PlayAttack()
    {
        m_Asource.PlayOneShot(m_Attack);
    }

    private void PlaySlam()
    {
        m_Asource.PlayOneShot(m_Slam);
    }
}
