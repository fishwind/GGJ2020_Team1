using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerPicker : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerStats m_Stats = null;
    [SerializeField] private PlayerItemFinder m_ItemFinder = null;
    [SerializeField] private Transform m_PickedItemParent = null;
    [SerializeField] private Animator m_Anim = null;
    public Transform m_PickedItem = null;

    [Header("Settings")]
    [Space(10)]
    [SerializeField] private float m_PlaceDistance = 1f;
    
    private void Start() {
        m_PickedItem = null;
    }

    public bool PickItem() {
        Transform item = m_ItemFinder.m_ItemInFront;
        if(!item)  return false;

        m_Anim.SetTrigger("Pickup");
        item.GetComponentInChildren<Collider>().enabled = false;
        item.parent = m_PickedItemParent;
        item.DOLocalMove(Vector3.zero, 0.3f);
        item.DOLocalRotate(Vector3.zero, 0.3f);
        m_PickedItem = item;
        m_ItemFinder.m_ItemInFront = null;
        return true;
    }

    public bool DropItem() {
        if(m_PickedItem == null) return false;
        if(!CheckCanDrop()) return false;

        m_Anim.SetTrigger("Dropdown");
        m_PickedItem.parent = null;
        Vector3 landPos = transform.position + transform.forward * m_PlaceDistance + GetComponent<Rigidbody>().velocity * 0.2f;
        landPos.y = m_PickedItem.GetComponent<Entity>().GetPlaceHeight();
        m_PickedItem.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Append(m_PickedItem.DOMove(landPos, 0.5f));
        seq.AppendCallback(()=>{m_PickedItem.GetComponentInChildren<Collider>().enabled = true;});
        seq.AppendCallback(()=>{m_PickedItem = null;});
        return true;
    }

    private bool CheckCanDrop() {
        if(m_ItemFinder.m_ItemInFront == null)  {
            return true;
        } else {
            // check if in front is placeable
            return false;
        }
    }
}
