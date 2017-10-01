using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class GhostSheet : MonoBehaviour
    {
        private static GhostSheet Instance;
        public static int Stage = 0;
        public Animator m_Animator;
        private BoxCollider m_TriggerCollider;
        public AudioSource m_Audio;
        public AudioSource m_Audio2;
        public Transform m_Ghost;

        private bool m_Triggered = false;
        private bool m_Viewed = false;
        private bool m_PlayerNear = false;
        private float m_Angle = 0.0f;

        private void Awake()
        {
            Instance = this;
            m_Animator.SetBool("Viewed", m_Viewed);
            m_Animator.SetBool("Triggered", m_Triggered);
            m_TriggerCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider player)
        {
            if (player.gameObject.tag == "Player")
            {
                m_PlayerNear = true;
                if (Stage == 1)
                    Instance.m_Audio2.Play();
            }
        }

        private void OnTriggerExit(Collider player)
        {
            if (player.gameObject.tag == "Player")
            {
                m_PlayerNear = false;
                Instance.m_Audio2.Stop();
            }
        }

        private void FixedUpdate()
        {
            if(m_PlayerNear && !m_Triggered)
            {
                m_Angle = Vector3.Angle(Player.Transform().forward, m_Ghost.forward);
                if(m_Angle >= 150f && m_Angle <= 165f && Stage == 1)
                {
                    JumpScare();
                }
            }
        }

        private void JumpScare()
        {
            Stage = 2;
            m_Triggered = true;
            m_Animator.SetBool("Triggered", true);
            Instance.m_Audio2.Stop();
            m_Audio.Play();
        }

        public static void PlayCreak()
        {
            Instance.m_Audio2.Play();
        }
    }
}