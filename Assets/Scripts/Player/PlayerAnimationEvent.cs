using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private AudioSource m_Asource = null;
    public AudioClip m_FootstepClip1 = null;
    public AudioClip m_FootstepClip2 = null;

    private void Start() {
        m_Asource = GetComponentInParent<AudioSource>();
    }
    public void PlayFootstep1() {
        m_Asource.PlayOneShot(m_FootstepClip1);
    }
    
    public void PlayFootstep2() {
        m_Asource.PlayOneShot(m_FootstepClip2);
    }
}
