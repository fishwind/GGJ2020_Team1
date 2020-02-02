using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float m_rotateSpeed = 10f;
    private void Start() {
        HeroCoinCollector.m_Instance.m_Coins.Add(this.transform);
    }

    private void Update() {
        transform.rotation *= Quaternion.Euler(0, Time.deltaTime * m_rotateSpeed, 0);
    }
}
