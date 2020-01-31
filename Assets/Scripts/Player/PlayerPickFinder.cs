using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickFinder : MonoBehaviour
{
    private HashSet<Transform> m_PickbleItemSet = new HashSet<Transform>();

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.layer == LayerMask.NameToLayer("Pickable"))
            m_PickbleItemSet.Add(col.transform);
    }

    private void OnTriggerExit(Collider col) {
        if(col.gameObject.layer == LayerMask.NameToLayer("Pickable"))
            m_PickbleItemSet.Remove(col.transform);
    }

    public Transform GetItem() {
        Transform returnItem = null;
        float smallestAngle = 360f;
        foreach(Transform item in m_PickbleItemSet)
        {
            float angle = Vector3.Angle(transform.forward, item.position - transform.forward);
            if(angle < smallestAngle)
            {
                angle = smallestAngle;
                returnItem = item;
            }
        }
        return returnItem;
    }

    public Transform[] GetItems() {
        Transform[] returnArray = new Transform[m_PickbleItemSet.Count];
        m_PickbleItemSet.CopyTo(returnArray);
        return returnArray;
    }

    public void RemoveItemFromPicker(Transform item) {
        if(m_PickbleItemSet.Contains(item))
            m_PickbleItemSet.Remove(item);
    }
}
