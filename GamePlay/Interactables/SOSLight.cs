using UnityEngine;
using Overtop.Scripts.Interactables;
using System.Collections;
using PonteCemetery.GamePlay;

namespace PonteCemtery.GamePlay.Interactables
{
    [RequireComponent(typeof(Light))]
    public class SOSLight : MonoBehaviour, IInteractable
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
        public Material m_LetsPlay;
        public Texture m_LetsPlayTexture;

        private void Awake()
        {
            m_Light = GetComponent<Light>();
            m_OrigIntensity = m_Light.intensity;
            m_AudioSource.clip = m_AudioClips[0];
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
        public void Interact()
        {
            m_Light.intensity = 0;
            m_Bulb.material = m_BulbOff;
            m_Signaling = false;
            m_AudioSource.Stop();
            PonteCemetery.Audio.Ambience.Instance.StartCoroutine(PonteCemetery.Audio.Ambience.BeginAmbience(1));
            m_GravekeeperGate.Reset();
            m_GravekeeperGate.SwapKey(123L);
            m_LetsPlay.SetTexture("_NameNormal", m_LetsPlayTexture);
            m_BeginRingAroundTheRosie.m_Next = true;
            m_BeginRingAroundTheRosie.m_Start = true;
        }

        public bool TryAction()
        {
            return true;
        }
    }
}