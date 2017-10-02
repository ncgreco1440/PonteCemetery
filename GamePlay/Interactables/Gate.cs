using System;
using Overtop.Scripts.Interactables;
using UnityEngine;

namespace PonteCemetery.GamePlay.Interactables
{
    public class Gate : MonoBehaviour, IInteractable
    {
        [SerializeField]
        protected bool m_Locked = false;

        protected Rigidbody m_Rigidbody;
        public HingeJoint m_HingeJoint;
        public HingeJoint m_HingeJoint2;

        protected JointLimits m_LockedJointLimits;
        protected JointLimits m_UnlockedJointLimits;
        protected JointMotor m_JointMotor;
        [SerializeField]
        protected long m_KeyCode;
        public AudioSource m_AudioSource;

        private void Awake()
        {
            try
            {
                m_Rigidbody = GetComponent<Rigidbody>();
                m_HingeJoint = GetComponent<HingeJoint>();
                m_AudioSource = GetComponent<AudioSource>();
            }
            catch(System.Exception e) {}
        }

        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactions");
            m_UnlockedJointLimits = m_HingeJoint.limits;
            m_LockedJointLimits.min = 0f;
            m_LockedJointLimits.max = 0f;
            m_LockedJointLimits.bounciness = 1f;
            m_LockedJointLimits.bounceMinVelocity = 0.2f;
            m_LockedJointLimits.contactDistance = 0f;

            if (m_Locked)
            {
                m_HingeJoint.limits = m_LockedJointLimits;
                try
                {
                    m_HingeJoint2.limits = m_LockedJointLimits;
                } catch (System.Exception e) { }
            }
            else
            {
                m_HingeJoint.limits = m_UnlockedJointLimits;
                try
                {
                    m_HingeJoint2.limits = m_UnlockedJointLimits;
                } catch (System.Exception e) { }
            }
        }

        /// <summary>
        /// 1. Unlocks the gate if player has Key with corresponding KeyCode. 
        ///   a. Opens the gate. 
        /// 2. Opens the gate if already unlocked.
        /// 3. Informs the player the gate is locked.
        /// </summary>
        public virtual void Interact()
        {
            OpenGate();
        }

        public virtual bool TryAction()
        {
            if (m_Locked)
                return Player.HasKey(m_KeyCode);
            else
                return true;
        }

        public virtual void PlaySound(ref AudioClip clip)
        {
            if(!m_AudioSource.isPlaying)
            {
                m_AudioSource.clip = clip;
                m_AudioSource.Play();
            }
        }

        private void OpenGate()
        {
            m_Locked = false;
            m_HingeJoint.limits = m_UnlockedJointLimits;
            try
            {
                m_HingeJoint2.limits = m_UnlockedJointLimits;
            } catch (System.Exception e) { }
        }

        public virtual void ForceOpen() { }

        public virtual void ForceClose() { }

        public virtual void Reset() { }
    }
}