using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTesterSpawner : MonoBehaviour
{
    public GameObject m_TesterPrefab = null;
    
    private void Start() {
        Instantiate(m_TesterPrefab, transform.position, Quaternion.identity);
    }
}
