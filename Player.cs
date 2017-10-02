using PonteCemetery.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace PonteCemetery
{
    public class Player : MonoBehaviour
    {
        private static Player m_Instance = null;

        private Transform m_Transform;
        public Transform m_CameraTransform;
        public List<long> m_KeyChain = new List<long>();
        public AudioSource m_AudioSource;
        public SphereCollider m_LostIndictator;

        private void Awake()
        {
            m_LostIndictator.enabled = false;
            if (m_Instance == null)
                m_Instance = this;
            else
            {
                Debug.LogError("You have multiple instantiations of 'Player'. This forbidden. Please remove all but one instance of 'Player'.");
                MenuAction.Quit();
            }
        }

        public static SphereCollider LostIndication()
        {
            return m_Instance.m_LostIndictator;
        }

        public static bool WasLost()
        {
            return m_Instance.m_LostIndictator.enabled;
        }

        public static void IsLost()
        {
            m_Instance.m_LostIndictator.enabled = true;
        }

        public static void IsFound()
        {
            m_Instance.m_LostIndictator.enabled = false;
        }

        private void Start()
        {
            m_Instance.m_Transform = GetComponent<Transform>();
        }

        public static void GetPlayerPositionAndLook(ref Vector3 pPos, ref Quaternion pRot, ref Quaternion cRot)
        {
            cRot = Camera.main.GetComponent<Transform>().localRotation;
            pPos = m_Instance.gameObject.GetComponent<Transform>().position;
            pRot = m_Instance.gameObject.GetComponent<Transform>().rotation;
        }

        public static Transform Transform()
        {
            return m_Instance.m_Transform;
        }

        public static Transform Look()
        {
            return m_Instance.m_CameraTransform;
        } 

        public static void AddToKeyChain(long key)
        {
            m_Instance.m_KeyChain.Add(key);
        }

        public static bool HasKey(long key)
        {
            return m_Instance.m_KeyChain.Contains(key);
        }

        public static void SetSoundFromBehind(AudioClip m_AudioClip)
        {
            m_Instance.m_AudioSource.clip = m_AudioClip;
        }

        public static void PlaySoundFromBehind()
        {
            m_Instance.m_AudioSource.Play();
        }
    }
}