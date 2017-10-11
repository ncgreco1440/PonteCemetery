using UnityEngine;
using Overtop.Scripts.Interactables;
using System.Collections;
using PonteCemetery.GamePlay;
using System;

namespace PonteCemtery.GamePlay.Interactables
{
    [RequireComponent(typeof(Light))]
    public class SOSLight : IInteractable
    {
        private Light m_Light;
        private bool m_Signaling = true;
        private bool m_OnOff = true;
        public AudioSource m_AudioSource;
        public AudioClip[] m_AudioClips;

        [SerializeField]
        private MeshRenderer m_Bulb;
        [SerializeField]
        private Material m_BulbOn;
        [SerializeField]
        private Material m_BulbOff;

        [SerializeField]
        private float m_OrigIntensity = 0.0f;

        private int m_Iteration = 1;
        [SerializeField]
        private float m_Duration = 0.5f;

        private bool m_Short = true;

        public RingAroundTheRosieTrigger m_BeginRingAroundTheRosie;
        public MetalGate m_GravekeeperGate;

        private void Awake()
        {
            m_Light = GetComponent<Light>();
            m_OrigIntensity = m_Light.intensity;
            m_AudioSource.clip = m_AudioClips[0];
            m_AudioSource.loop = false;
        }

        private void OnEnable()
        {
            StartCoroutine(SOSSignal());
            StartCoroutine(HelpMeVoice());
        }

        private void OnDisable()
        {
            StopCoroutine(SOSSignal());
            StopCoroutine(HelpMeVoice());
        }

        private IEnumerator HelpMeVoice()
        {
            while(m_Signaling)
            {
                yield return new WaitForSeconds(25.0f);
                m_AudioSource.Play();
            }
        }

        private IEnumerator SOSSignal()
        {
            while(m_Signaling)
            {
                if (m_OnOff)
                {
                    m_Light.intensity /= 2;
                    m_Bulb.material = m_BulbOff;
                    m_OnOff = false;
                }
                else
                {
                    m_Light.intensity = m_OrigIntensity;
                    m_Bulb.material = m_BulbOn;
                    m_OnOff = true;
                    m_Iteration++;
                }

                yield return new WaitForSeconds(Duration);
            }
        }

        public void Reset()
        {
            m_Signaling = true;
            m_AudioSource.loop = false;
            m_AudioSource.Play();
            m_GravekeeperGate.Reset();
            gameObject.layer = LayerMask.NameToLayer("Interactions");
            OnEnable();
        }

        private float Duration
        {
            get
            {
                if (m_Iteration > 3)
                    Alternate();
                return m_Duration;
            }
        }

        private void Alternate()
        {
            if (m_Short)
                m_Duration = 1f;
            else
                m_Duration = 0.5f;

            m_Iteration = 1;
            m_Short = !m_Short;
        }
        
        /// <summary>
        /// Cease SOS signal
        /// </summary>
        public override void Interact()
        {
            m_Light.intensity = 0;
            m_Bulb.material = m_BulbOff;
            m_Signaling = false;
            m_AudioSource.loop = false;
            m_AudioSource.Stop();
            //PonteCemetery.Audio.Ambience.Instance.StartCoroutine(PonteCemetery.Audio.Ambience.BeginAmbience(1));
            m_GravekeeperGate.Reset();
            m_GravekeeperGate.SwapKey(123L);
            m_GravekeeperGate.m_ReasonForFailedInteraction = "This gate won't open anymore.";
            gameObject.layer = LayerMask.NameToLayer("Default");
            RingAroundTheRosie.Instance.m_CanBegin = true;
        }

        public override bool TryAction()
        {
            return true;
        }

        public void TurnOn()
        {
            StartCoroutine(LightUp());
        }

        private IEnumerator LightUp()
        {
            while(m_Light.intensity < 4.0f)
            {
                m_Light.intensity += 0.25f;
                yield return new WaitForSeconds(0.33f);
            }
        }
    }
}