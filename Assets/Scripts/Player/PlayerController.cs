using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats m_Stats = null;
    [SerializeField] private PlayerPicker m_Picker = null;
    [SerializeField] private PlayerItemFinder m_ItemFinder = null;
    [SerializeField] private Animator m_Anim = null;
    [SerializeField] private AudioClip m_HoldActiveDoneClip = null;
    [SerializeField] private AudioClip m_HittingClip = null;
    [SerializeField] private AudioClip m_SweepingClip = null;

    public bool m_CanMove = true;
    private Camera m_Cam = null;
    private Rigidbody m_Rbody = null;
    private AudioSource m_Asource = null;
    private Coroutine m_HoldActiveCoroutine = null;

    // for holding spacebar actions
    private bool m_HoldingActiveAction = false;
    private IRepairable m_RepairToBeStopped = null;

    [Header("Settings")]
    [Space(10)]

    // Player private
    private bool m_HasPicked = false;

    private void OnEnabled() {
        GlobalEvents.OnMenuOpened += EnablePlayerMovement;
    }

    private void OnDisable() {
        GlobalEvents.OnMenuOpened -= EnablePlayerMovement;                 
    }

    private void Start() {
        m_Cam = Camera.main;
        m_Rbody = GetComponent<Rigidbody>();
        m_Asource = GetComponent<AudioSource>();
    }

    private void Update() {
        // when let go space while sweep/hitting halfway
        if(Input.GetKeyUp(KeyCode.Space) && m_HoldingActiveAction) {
            m_Anim.SetBool("Sweep", false);
            m_Anim.SetBool("Hit", false);
            m_CanMove = true;
            if(m_HoldActiveCoroutine != null) {
                StopCoroutine(m_HoldActiveCoroutine);
            }
            StopActiveHoldingAction(m_RepairToBeStopped);
        }
        if(!m_CanMove) return;
        UpdateMovement();
        UpdateAction();

        if (Input.GetKeyUp(KeyCode.J))
        {
            GlobalEvents.SendPlayerStartDestroyAll(1);
        }
    }

    private void FixedUpdate() {
        LimitMaxSpeed(m_Stats.m_MaxSpeed);
    }

    private void UpdateMovement() {
        // determine move direction
        Vector3 dir = Vector3.zero;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            dir += m_Cam.transform.up;
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            dir -= m_Cam.transform.up;
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            dir -= m_Cam.transform.right;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            dir += m_Cam.transform.right;
        dir.y = 0;
        dir = dir.normalized;

        // animation
        Vector3 animationVelocity = m_Rbody.velocity;
        animationVelocity.y = 0;
        float currSpeed = m_Anim.GetFloat("Speed");
        float animSpeed = Mathf.Lerp(currSpeed, animationVelocity.sqrMagnitude, 5f * Time.deltaTime);
        m_Anim.SetFloat("Speed", animSpeed);


        if(dir == Vector3.zero) return;

        // Add moving force
        m_Rbody.AddForce(dir * m_Stats.m_MovePower * Time.deltaTime);

        // rotate player
        UpdateRotation(dir);
    }

    private void LimitMaxSpeed(float maxSpeed) {
        float finalMaxSpeed = Input.GetKey(KeyCode.Z)? maxSpeed * 3f : maxSpeed;

        if(m_Rbody.velocity.sqrMagnitude > finalMaxSpeed * finalMaxSpeed)
            m_Rbody.velocity = m_Rbody.velocity.normalized * finalMaxSpeed;
    }

    private void UpdateRotation(Vector3 dir) {
        if(dir == Vector3.zero || dir == Vector3.up)   return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), m_Stats.m_RotateSpeed * Time.deltaTime);
    }

    private void UpdateAction() {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(m_Picker.m_PickedItem)
            {
                this.PickDropAction();
                return;
            }
            Transform item = m_ItemFinder.m_ItemInFront;
            if(!item) return;

            Entity entity = item.GetComponentInParent<Entity>();
            if(!entity) return;

            ItemActionState actionState = entity.currItemActionState;
            switch(actionState) {
                case ItemActionState.Pickup: this.PickDropAction(); break;
                case ItemActionState.RepairHit:
                    IRepairable repairableHit = (IRepairable)entity.GetComponent(typeof(IRepairable));
                    if(repairableHit != null) {
                        repairableHit.StartRepairing();
                        m_CanMove = false;
                        m_Anim.SetBool("Hit", true);
                        m_HoldActiveCoroutine = StartCoroutine(HoldingActive(repairableHit, actionState));
                        m_RepairToBeStopped = repairableHit;
                        m_HoldingActiveAction = true;
                    }
                    break;
                case ItemActionState.RepairSweep:
                    IRepairable repairableSweep = (IRepairable)entity.GetComponent(typeof(IRepairable));
                    if(repairableSweep != null) {
                        repairableSweep.StartRepairing();
                        m_CanMove = false;
                        m_Anim.SetBool("Sweep", true);
                        m_HoldActiveCoroutine = StartCoroutine(HoldingActive(repairableSweep, actionState));
                        m_RepairToBeStopped = repairableSweep;
                        m_HoldingActiveAction = true;
                    }
                    break;
                default: break;
            }
        }
    }

    private void EnablePlayerMovement(bool canMove)
    {
        m_CanMove = canMove;
    }

    private void PickDropAction() {
        bool successful = false;
        if(!m_HasPicked)
            successful = m_Picker.PickItem();
        else 
            successful = m_Picker.DropItem();

        m_HasPicked = (successful)? !m_HasPicked : m_HasPicked;
    }

    private void CompleteActiveHoldingAction(IRepairable repairable) {
        m_Anim.SetBool("Sweep", false);
        m_Anim.SetBool("Hit", false);
        m_CanMove = true;
        m_Asource.PlayOneShot(m_HoldActiveDoneClip);
        repairable.CompleteRepairing();
        m_Asource.Stop();
        m_HoldingActiveAction = false;
        Debug.Log(">>>>>>>>> completed holding action");
    }

    private void StopActiveHoldingAction(IRepairable repairable) {
        if(repairable == null) return;
        m_Anim.SetBool("Sweep", false);
        m_Anim.SetBool("Hit", false);
        m_CanMove = true;
        repairable.StopRepairing();
        m_Asource.Stop();
        m_RepairToBeStopped = null;
        m_HoldingActiveAction = false;
        Debug.Log(">>>>>>>>> stopped holding action");
    }

    private IEnumerator HoldingActive(IRepairable repairable, ItemActionState state) {
        if(state == ItemActionState.RepairHit)  m_Asource.clip = m_HittingClip;
        if(state == ItemActionState.RepairSweep)  m_Asource.clip = m_SweepingClip;
        m_Asource.Play();
        yield return new WaitForSeconds(3f);
        CompleteActiveHoldingAction(repairable);
    }
}
