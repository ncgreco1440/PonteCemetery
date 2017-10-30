using PonteCemetery.GamePlay.Interactables;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class MetalGate : Gate
    {
        [SerializeField]
        private bool m_Unlocked = false;
        [SerializeField]
        private bool m_Opened = false;

        private BoxCollider m_InteractableCollider;
        public AudioClip m_LockedSound;
        public AudioClip m_OpeningSound;
        public AudioClip m_ClosingSound;
        public AudioClip m_ClosedSound;

        public Rigidbody m_LeftGate;
        public Rigidbody m_RightGate;

        public Vector3 m_Torque = new Vector3(0.0f, -1.25f, 0.0f);
        [SerializeField]
        private Vector3 m_OrigTorque;

        private float m_TimePassed = 0.0f;
        private Quaternion m_ClosedRightGateRotation;
        private Quaternion m_ClosedLeftGateRotation;

        private JointLimits m_ClosedLimits;
        private JointLimits m_ClosedLimits2;

        [SerializeField]
        private bool m_Opening = false;

        public override void Awake()
        {
            m_InteractableCollider = GetComponent<BoxCollider>();
            m_AudioSource = GetComponent<AudioSource>();
            m_ClosedRightGateRotation = m_RightGate.GetComponent<Transform>().localRotation;
            m_ClosedLeftGateRotation = m_LeftGate.GetComponent<Transform>().localRotation;
        }

        public override void Start()
        {
            base.Start();
            m_ClosedLimits.max = 85;
            m_ClosedLimits.min = 0;
            m_ClosedLimits2.max = 0;
            m_ClosedLimits2.min = -85;
            m_OrigTorque = m_Torque;
        }

        public override void Interact()
        {
            if (base.TryAction())
            {
                base.PlaySound(ref m_OpeningSound);
                base.Interact();
                m_InteractableCollider.enabled = false;
                m_RightGate.gameObject.layer = LayerMask.NameToLayer("Action");
                m_LeftGate.gameObject.layer = LayerMask.NameToLayer("Action");
                m_RightGate.isKinematic = false;
                m_LeftGate.isKinematic = false;
                if (m_Torque != m_OrigTorque) m_Torque = m_OrigTorque;
                m_Opening = true;
                m_Unlocked = true;
                m_TimePassed = 0.0f;
                m_Opened = false;
            }
            else
            {
                base.PlaySound(ref m_LockedSound);
                InvokeInteractiveFeedbackEvent(m_ReasonForFailedInteraction);
            }
        }

        private void FixedUpdate()
        {
            if(m_Unlocked && !m_Opened)
            {
                m_TimePassed += Time.fixedDeltaTime;
                if (m_Opening)
                {
                    m_RightGate.AddTorque(m_Torque, ForceMode.Force);
                    m_LeftGate.AddTorque(-m_Torque, ForceMode.Force);
                }
                else
                {
                    m_RightGate.AddTorque(-m_Torque, ForceMode.Force);
                    m_LeftGate.AddTorque(m_Torque, ForceMode.Force);
                }
                m_Opened = m_TimePassed >= 6.5f;
                m_RightGate.isKinematic = m_Opened;
                m_LeftGate.isKinematic = m_Opened;

                SetLayer();
            }
        }

        /// <summary>
        /// Set the layer of the gate
        /// </summary>
        private void SetLayer()
        {
            if (m_Opened && !m_Opening)
            {
                gameObject.layer = LayerMask.NameToLayer("Interactions");
            }

            if (m_Opened && m_Opening)
            {
                gameObject.layer = LayerMask.NameToLayer("Fence");
                m_RightGate.gameObject.layer = LayerMask.NameToLayer("Fence");
                m_LeftGate.gameObject.layer = LayerMask.NameToLayer("Fence");
            }
        }

        public override void ForceOpen()
        {
            base.PlaySound(ref m_OpeningSound);
            base.Interact();
            m_RightGate.gameObject.layer = LayerMask.NameToLayer("Action");
            m_LeftGate.gameObject.layer = LayerMask.NameToLayer("Action");
            m_InteractableCollider.enabled = false;
            m_Unlocked = true;
        }

        public override void Reset()
        {
            m_Locked = m_InteractableCollider.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Interactions");
            m_RightGate.gameObject.layer = LayerMask.NameToLayer("Fence");
            m_LeftGate.gameObject.layer = LayerMask.NameToLayer("Fence");
            m_Unlocked = m_Opened = m_RightGate.isKinematic = m_LeftGate.isKinematic = false;
            m_TimePassed = 0.0f;
            m_RightGate.transform.localRotation = m_ClosedRightGateRotation;
            m_LeftGate.transform.localRotation = m_ClosedLeftGateRotation;
            m_AudioSource.clip = m_ClosingSound;
            m_AudioSource.Play();
        }

        public void Unlock()
        {
            m_Locked = false;
        }

        public void Lock()
        {
            m_Locked = true;
        }

        public void Lock(long key)
        {
            m_Locked = true;
            m_KeyCode = key;
        }

        public void SwapKey(long newKeyCode)
        {
            m_KeyCode = newKeyCode;
        }

        public override void ForceClose()
        {
            Close();
        }

        public override void ForceClose(bool locked)
        {
            m_Locked = locked;
            Close();
        }

        private void Close()
        {
            m_InteractableCollider.enabled = true;
            m_Torque *= 3;
            m_TimePassed = 0.0f;
            if (m_OrigTorque.y > 0.0f)
            {
                m_HingeJoint.limits = m_ClosedLimits;
                m_HingeJoint2.limits = m_ClosedLimits2;
            }
            else
            {
                m_HingeJoint.limits = m_ClosedLimits2;
                m_HingeJoint2.limits = m_ClosedLimits;
            }
            base.PlaySound(ref m_ClosingSound);
            StartCoroutine(PlayClosedSound(2f));
            m_RightGate.isKinematic = false;
            m_LeftGate.isKinematic = false;
            m_Opening = false;
            m_Unlocked = true;
            m_Opened = false;
        }

        private IEnumerator PlayClosedSound(float time)
        {
            yield return new WaitForSeconds(time);
            base.PlaySound(ref m_ClosedSound);
        }
    }
}