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
    [SerializeField] private AudioClip m_CannotDropClip = null;
    [SerializeField] private AudioClip m_PickupClip = null;
    [SerializeField] private AudioClip m_DropdownClip = null;
    public Transform m_PickedItem = null;
    private AudioSource m_Asource = null;

    [Header("Settings")]
    [Space(10)]
    [SerializeField] private float m_PlaceDistance = 1f;
    
    private void Start() {
        m_PickedItem = null;
        m_Asource = GetComponent<AudioSource>();
    }

    public bool PickItem() {
        Transform item = m_ItemFinder.m_ItemInFront;
        if(!item || Vector3.Distance(item.position, transform.position) > 1.5f)  return false;
        Entity entity = item.GetComponentInParent<Entity>();

        // sound
        m_Asource.PlayOneShot(m_PickupClip);

        // animation
        m_Anim.SetTrigger("Pickup");
        m_Anim.SetLayerWeight(1, 1);
        entity.SetMeshColliders(false);
        entity.transform.parent = m_PickedItemParent;
        entity.transform.DOLocalMove(Vector3.zero, 0.3f);
        entity.transform.DOLocalRotate(Vector3.zero, 0.3f);
        m_PickedItem = entity.transform;
        m_ItemFinder.m_ItemInFront = null;
        return true;
    }

    public bool DropItem() {
        if(m_PickedItem == null) return false;
        if(!CheckCanDrop()) return false;


        //sounds 
        m_Asource.PlayOneShot(m_DropdownClip);

        // animations
        Entity entity = m_PickedItem.GetComponentInParent<Entity>();
        float height = entity.GetPlaceHeight();
        m_Anim.SetTrigger("Dropdown");
        m_PickedItem.parent = null;
        Vector3 landPos = transform.position + transform.forward * m_Stats.m_PlaceDistance + GetComponent<Rigidbody>().velocity * 0.2f;
        landPos.y = height;
        m_PickedItem.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Append(m_PickedItem.DOMove(landPos, 0.5f));
        seq.AppendCallback(()=>{entity.SetMeshColliders(true);});
        seq.AppendCallback(()=>{m_PickedItem = null;});
        float weight = 0;
        seq.AppendCallback(()=>{DOTween.To(()=> weight, x=> {
            weight = x;
            m_Anim.SetLayerWeight(1, weight);
        }, 0, 0.01f);});
        
        return true;
    }

    private bool CheckCanDrop() {
        Transform item = m_ItemFinder.m_ItemInFront;
        if(item == null)  {
            return true;
        } else if (
            item.GetComponentInParent<Furnace>() 
            && m_PickedItem.GetComponent<Pot>() != null 
            && m_PickedItem.GetComponent<Pot>().currItemState == ItemStates.Unfired)
        {
            return true;
        } else if (
           item.GetComponentInParent<PotteryWheel>()
           && m_PickedItem.GetComponent<CraftMat>() != null
           && m_PickedItem.GetComponent<CraftMat>().currItemState == ItemStates.CraftMat)
        {
            return true;
        } else {
            // todo: play cannot drop sound
            m_Asource.PlayOneShot(m_CannotDropClip);
            Debug.Log(">>>> cannot drop");
            return false;
        }
    }
}
