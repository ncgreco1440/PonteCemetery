using PonteCemetery.GamePlay.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class MetalGate : Gate
    {
        private bool m_Unlocked = false;
        private bool m_Opened = false;

        private BoxCollider m_InteractableCollider;
        public AudioClip m_LockedSound;
        public AudioClip m_OpeningSound;
        public AudioClip m_ClosingSound;

        public Rigidbody m_LeftGate;
        public Rigidbody m_RightGate;

        public Vector3 m_Torque = new Vector3(0.0f, -1.25f, 0.0f);
        private float m_TimePassed = 0.0f;
        private Quaternion m_ClosedRightGateRotation;
        private Quaternion m_ClosedLeftGateRotation;

        private void Awake()
        {
            m_InteractableCollider = GetComponent<BoxCollider>();
            m_AudioSource = GetComponent<AudioSource>();
            m_ClosedRightGateRotation = m_RightGate.GetComponent<Transform>().localRotation;
            m_ClosedLeftGateRotation = m_LeftGate.GetComponent<Transform>().localRotation;
        }

        public override void Interact()
        {
            if (base.TryAction())
            {
                base.PlaySound(ref m_OpeningSound);
                base.Interact();
                //Destroy(m_InteractableCollider);
                m_InteractableCollider.enabled = false;
                m_Unlocked = true;
            }
            else
            {
                base.PlaySound(ref m_LockedSound);
            }
        }

        private void FixedUpdate()
        {
            if(m_Unlocked && !m_Opened)
            {
                m_TimePassed += Time.fixedDeltaTime;
                m_RightGate.AddTorque(m_Torque, ForceMode.Force);
                m_LeftGate.AddTorque(-m_Torque, ForceMode.Force);
                m_Opened = m_TimePassed >= 6.5f;
                m_RightGate.isKinematic = m_Opened;
                m_LeftGate.isKinematic = m_Opened;
                if (m_Opened)
                {
                    gameObject.layer = LayerMask.NameToLayer("Fence");
                }
            }
        }

        public override void ForceOpen()
        {
            base.PlaySound(ref m_OpeningSound);
            base.Interact();
            //Destroy(m_InteractableCollider);
            m_InteractableCollider.enabled = false;
            m_Unlocked = true;
        }

        public override void Reset()
        {
            m_Locked = m_InteractableCollider.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Interactions");
            m_Unlocked = m_Opened = m_RightGate.isKinematic = m_LeftGate.isKinematic = false;
            m_TimePassed = 0.0f;
            m_RightGate.transform.localRotation = m_ClosedRightGateRotation;
            m_LeftGate.transform.localRotation = m_ClosedLeftGateRotation;
            m_AudioSource.clip = m_ClosingSound;
            m_AudioSource.Play();
        }

        public void SwapKey(long newKeyCode)
        {
            m_KeyCode = newKeyCode;
        }
    }
}