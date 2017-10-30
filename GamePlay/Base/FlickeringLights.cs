using Overtop.Lights;
using System.Collections;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    /// <summary>
    /// Accepts an array of lights that can be set to flicker on and off repeatedly 
    /// and then fade back on. 
    /// </summary>
    public class FlickeringLights : MonoBehaviour
    {
        [SerializeField]
        private AudioSource m_AudioSource;
        [SerializeField]
        private bool m_Flickering = false;
        [SerializeField]
        private OvertopLight[] m_Lights;

        /// <summary>
        /// Begins flickering if the internal coroutine isn't already running
        /// </summary>
        /// <returns></returns>
        public bool BeginFlickering(AudioClip sound = null)
        {
            if (!m_Flickering)
            {
                if (sound != null)
                    PlaySound(sound);
                m_Flickering = true;
                StartCoroutine(Flicker());
                return true;
            }     
            else
                return false;
        }

        /// <summary>
        /// Ends flickering if the internal coroutine was running
        /// </summary>
        /// <returns></returns>
        public bool EndFlickering(AudioClip sound = null)
        {
            if(m_Flickering)
            {
                if (sound != null)
                    PlaySound(sound);
                m_Flickering = false;
                return true;
            }
            return false;
        }

        private IEnumerator Flicker()
        {
            while (m_Flickering)
            {
                for (int i = 0; i < m_Lights.Length; i++)
                {
                    m_Lights[i].TurnOn();
                }

                yield return new WaitForSeconds(0.25f);

                for (int i = 0; i < m_Lights.Length; i++)
                {
                    m_Lights[i].TurnOff();
                }

                yield return new WaitForSeconds(0.25f);
            }

            StartCoroutine(FadeLights());
        }

        private IEnumerator FadeLights()
        {
            for (int i = 0; i < m_Lights.Length; i++)
            {
                m_Lights[i].SetIntensity(0.0f);
                m_Lights[i].TurnOn();
            }

            while (m_Lights[0].GetIntensity() < 2.25f)
            {
                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < m_Lights.Length; i++)
                    m_Lights[i].SetIntensity(m_Lights[i].GetIntensity() + 0.12f);
            }
        }

        public void TurnOffAllLights()
        {
            for(int i = 0; i < m_Lights.Length; i++)
            {
                m_Lights[i].TurnOff();
            }
        }

        public void TurnOffAllLights(AudioClip sound = null)
        {
            if (sound != null)
                PlaySound(sound);
            for (int i = 0; i < m_Lights.Length; i++)
            {
                m_Lights[i].TurnOff();
            }
        }

        public void TurnOnAllLights(AudioClip sound = null)
        {
            if (sound != null)
                PlaySound(sound);
            for (int i = 0; i < m_Lights.Length; i++)
            {
                m_Lights[i].TurnOn();
            }
        }

        public void PlaySound(AudioClip sound)
        {
            m_AudioSource.clip = sound;
            m_AudioSource.Play();
        }
    }
}