using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay.Interactables
{
    public class WoodenDoor : Gate
    {
        public bool m_Unlocked = false;
        public bool m_Opened = false;
        public AudioClip m_LockedSound;
        public AudioClip m_OpeningSound;
        public AudioClip m_ClosingSound;
        public Transform m_Knob1;
        public Transform m_Knob2;

        public Vector3 m_Torque = new Vector3(0.0f, -0.5f, 0.0f);
        Vector3 m_OrigTorque;
        private float m_TimePassed = 0.0f;

        private JointLimits m_ClosedLimits;
        private bool m_Opening = false;

        public override void Awake()
        {
            base.Awake();
            m_ClosedLimits.max = 0;
            m_ClosedLimits.min = -85;
        }

        public override void Start()
        {
            base.Start();
            m_OrigTorque = m_Torque;
        }

        public override void Interact()
        {
            if (base.TryAction())
            {   
                base.PlaySound(ref m_OpeningSound);
                base.Interact();
                m_Unlocked = true;
            }
            else
            { 
                base.PlaySound(ref m_LockedSound);
                InvokeInteractiveFeedbackEvent(m_ReasonForFailedInteraction);
            }
        }

        private void FixedUpdate()
        {
            if (m_Unlocked && !m_Opened)
            {
                m_TimePassed += Time.fixedDeltaTime;
                if (m_Opening)
                    m_Rigidbody.AddTorque(m_Torque);
                else
                    m_Rigidbody.AddTorque(-(m_Torque));
                m_Opened = m_Rigidbody.isKinematic = m_TimePassed >= 6.5f;
                if (m_Opened)
                {
                    gameObject.layer = LayerMask.NameToLayer("Fence");
                }
            }   
        }

        private IEnumerator TurnKnobs(float deg)
        {
            bool turning = true;
            float source = 0, rot = 0, t = 0;
            while(turning)
            {
                t = t + Time.deltaTime <= 1f ? t + Time.deltaTime : 1f;
                rot = Mathf.Lerp(source, deg, t);
                m_Knob1.localRotation = Quaternion.Euler(-90f, rot, 0f);
                m_Knob2.localRotation = Quaternion.Euler(-90f, rot, 0f);
                if (m_Knob1.localRotation == Quaternion.Euler(-90f, deg, 0f))
                {
                    t = 0f;
                    deg = 0f;
                    source = -45f;
                }
                if (m_Knob1.localRotation == Quaternion.Euler(-90f, 0f, 0f))
                    turning = false;
                yield return new WaitForFixedUpdate();
            }
        }

        public override void ForceOpen()
        {
            m_Torque = m_OrigTorque;
            m_TimePassed = 0.0f;
            base.PlaySound(ref m_OpeningSound);
            base.Interact();
            m_Rigidbody.isKinematic = false;
            m_Opening = true;
            m_Opened = false;
            m_Unlocked = true;
        }

        public override void ForceClose()
        {
            m_TimePassed = 0.0f;
            m_HingeJoint.limits = m_ClosedLimits;
            base.PlaySound(ref m_ClosingSound);
            m_Rigidbody.isKinematic = false;
            m_Opening = false;
            m_Unlocked = true;
            m_Opened = false;
        }

        public void SlamShut()
        {
            m_Torque *= 4;
            m_TimePassed = 0.0f;
            m_HingeJoint.limits = m_ClosedLimits;
            base.PlaySound(ref m_ClosingSound);
            m_Rigidbody.isKinematic = false;
            m_Opening = false;
            m_Unlocked = true;
            m_Opened = false;
        }
    }
}