using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemFinder : MonoBehaviour
{
    public Transform m_ItemInFront = null;
    public Vector3 m_ItemInFronPos = Vector3.zero;


    private void Update() {
        RaycastHit hit;
        LayerMask layer = ~(1 << gameObject.layer);
        if(Physics.SphereCast(transform.position - transform.forward * 0.5f, 0.5f, transform.forward, out hit, 2, layer)) {
            m_ItemInFront = hit.transform;
            m_ItemInFronPos = hit.point;
        } else {
            m_ItemInFront = null;
            m_ItemInFronPos = Vector3.zero;
        }
     }
}
