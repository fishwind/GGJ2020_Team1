using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerStats : ScriptableObject
{
    public float m_MovePower = 1000;
    public float m_MaxSpeed = 8;
    public float m_RotateSpeed = 8;
    public float m_ThrowPower = 1000f;
}
