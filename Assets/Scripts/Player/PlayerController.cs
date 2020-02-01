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
    private Camera m_Cam = null;
    private Rigidbody m_Rbody = null;

    [Header("Settings")]
    [Space(10)]

    // Player private
    private bool m_HasPicked = false;

    private void Start() {
        m_Cam = Camera.main;
        m_Rbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        UpdateMovement();
        UpdateAction();
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
        m_Anim.SetFloat("Speed", animationVelocity.sqrMagnitude);


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
            Transform item = m_ItemFinder.m_ItemInFront;
            if(!item) return;

            Entity entity = item.GetComponent<Entity>();
            ItemActionState actionState = entity.ItemActionState;
            switch(actionState) {
                case 1: this.PickDropAction(); break;
                case 2: entity.Sweep(); break;
                default: return;
            }
            
        }
    }

    private void PickDropAction() {
        bool successful = false;
        if(!m_HasPicked)
            successful = m_Picker.PickItem();
        else 
            successful = m_Picker.DropItem();

        m_HasPicked = (successful)? !m_HasPicked : m_HasPicked;
    }

    private void Sweep() {

    }
}
