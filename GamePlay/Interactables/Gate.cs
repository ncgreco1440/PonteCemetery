using System;
using Overtop.Scripts.Interactables;
using UnityEngine;

namespace PonteCemetery.GamePlay.Interactables
{
    public class Gate : IInteractable
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
        public AudioSource m_AudioSource2;
        public AudioSource m_AudioSource3;
        public AudioSource m_AudioSource4;
        public AudioSource m_AudioSource5;

        public virtual void Awake()
        {
            try
            {
                m_Rigidbody = GetComponent<Rigidbody>();
                m_HingeJoint = GetComponent<HingeJoint>();
                m_AudioSource = GetComponent<AudioSource>();
            }
            catch(System.Exception e) {}
        }

        public virtual void Start()
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
        public override void Interact()
        {
            OpenGate();
        }

        public override bool TryAction()
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
            }else if(!m_AudioSource2.isPlaying)
            {
                m_AudioSource2.clip = clip;
                m_AudioSource2.Play();
            }else if(!m_AudioSource3.isPlaying)
            {
                m_AudioSource3.clip = clip;
                m_AudioSource3.Play();
            }else if (!m_AudioSource4.isPlaying)
            {
                m_AudioSource4.clip = clip;
                m_AudioSource4.Play();
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