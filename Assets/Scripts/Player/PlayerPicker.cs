using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerPicker : MonoBehaviour
{
    [SerializeField] private PlayerStats m_Stats = null;
    [SerializeField] private PlayerPickFinder m_PickFinder = null;
    [SerializeField] private Transform m_PickedItemParent = null;
    [SerializeField] private Animator m_Anim = null;
    private Transform m_PickedItem = null;
    
    public void PickItem() {
        Transform item = m_PickFinder.GetItem();
        if(!item)  return;

        m_Anim.SetTrigger("Pickup");
        Rigidbody itemRigidBody = item.GetComponent<Rigidbody>();
        if(itemRigidBody) {
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponentInChildren<Collider>().enabled = false;
        }
        else {
            Debug.LogError(">>>> pick item does not have rigidbody!");
        }    
        item.parent = m_PickedItemParent;
        item.DOLocalMove(Vector3.zero, 0.3f);
        item.DOLocalRotate(Vector3.zero, 0.3f);
        m_PickedItem = item;
    }

    public void DropItem() {
        if(m_PickedItem == null) return;

        m_Anim.SetTrigger("Dropdown");
        Rigidbody itemRigidBody = m_PickedItem.GetComponent<Rigidbody>();
        if(itemRigidBody) {
            m_PickedItem.GetComponent<Rigidbody>().isKinematic = false; 
            m_PickedItem.GetComponent<Rigidbody>().AddForce(transform.forward * m_Stats.m_ThrowPower);
            m_PickedItem.GetComponentInChildren<Collider>().enabled = true;
            m_PickedItem.parent = null;
            m_PickFinder.RemoveItemFromPicker(m_PickedItem);
        }
        else {
            Debug.LogError(">>>> drop item does not have rigidbody!");
        }
    }
}
