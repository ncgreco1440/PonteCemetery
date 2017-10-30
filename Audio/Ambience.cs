using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Overtop.Managers;

namespace PonteCemetery.Audio
{
    public class Ambience : MonoBehaviour
    {
        private static Ambience m_Instance;
        public AudioSource m_AudioSource1;
        public AudioSource m_AudioSource2;
        private AudioSource m_CurrentAudioSource;
        public List<AudioClip> m_AmbientSounds;

        private bool m_Swapping = false;
        [SerializeField]
        private bool m_UsingFirstSource = true;
        private float m_TimePassed = 0.0f;
        private float m_TimeInterval = 0.2f;

        private int m_CurrentSound;
        private bool m_Done;

        [SerializeField]
        private float m_CurrentPlaybackTime = 0.0f;

        private void Awake()
        {
            SoundProperties.OnBeforeAudioConfigurationChanged += SaveCurrentAudioSettings;
            AudioSettings.OnAudioConfigurationChanged += Reload;
            m_Instance = this;
            m_CurrentAudioSource = m_AudioSource1;
        }

        private void SaveCurrentAudioSettings()
        {
            m_CurrentPlaybackTime = m_CurrentAudioSource.time;
        }

        private void Reload(bool deviceWasChanged)
        {
            m_CurrentAudioSource.clip = m_AmbientSounds[3];
            m_CurrentAudioSource.time = m_CurrentPlaybackTime;
            m_CurrentAudioSource.Play();
        }

        public static Ambience Instance
        {
            get
            {
                return m_Instance;
            }
        }

        /// <summary>
        /// Inside the mind of a modern day John Carmack!
        /// </summary>
        private void Update()
        {
            if(m_Swapping)
            {
                m_TimePassed += Time.unscaledDeltaTime;
                if(m_TimePassed >= m_TimeInterval)
                {
                    if (m_UsingFirstSource)
                    {
                        m_AudioSource1.volume = m_AudioSource1.volume + 0.05f;
                        m_AudioSource2.volume = m_AudioSource2.volume - 0.05f;
                        if (m_AudioSource2.volume == 0)
                        {
                            m_Swapping = false;
                            m_AudioSource2.volume = 0f;
                            m_AudioSource2.Stop();
                        }
                    }
                    else
                    {
                        m_AudioSource2.volume = m_AudioSource2.volume + 0.05f;
                        m_AudioSource1.volume = m_AudioSource1.volume - 0.05f;
                        if (m_AudioSource1.volume == 0)
                        {
                            m_Swapping = false;
                            m_AudioSource1.volume = 0f;
                            m_AudioSource1.Stop();
                        }
                    }
                    m_TimePassed = 0.0f;
                }
            }
        }

        public static void SwapAmbience(int index, float increment)
        {
            m_Instance.m_Swapping = false; // Stop any current swapping
            m_Instance.m_TimePassed = 0.0f;
            m_Instance.m_UsingFirstSource = !m_Instance.m_UsingFirstSource;
            if (m_Instance.m_UsingFirstSource)
            {
                m_Instance.m_AudioSource1.clip = m_Instance.m_AmbientSounds[index];
                m_Instance.m_AudioSource1.Play();
            } 
            else
            {
                m_Instance.m_AudioSource2.clip = m_Instance.m_AmbientSounds[index];
                m_Instance.m_AudioSource2.Play();
            }
            m_Instance.m_Swapping = true;
        }

        /// <summary>
        /// Begin Ambience at a set incremental speed
        /// </summary>
        /// <param name="index"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static IEnumerator BeginAmbience(int index, float increment)
        {
            m_Instance.m_CurrentSound = index;
            yield return m_Instance.StartCoroutine(EndAmbience(increment));
            m_Instance.m_CurrentAudioSource.clip = m_Instance.m_AmbientSounds[index];
            m_Instance.m_CurrentAudioSource.Play();
            while (m_Instance.m_CurrentAudioSource.volume + increment <= 1)
            {
                m_Instance.m_Done = (m_Instance.m_CurrentAudioSource.volume += increment) >= 1.0f;
                yield return new WaitForSecondsRealtime(0.33f);
            }
        }

        public static IEnumerator EndAmbience(float increment)
        {
            while (m_Instance.m_CurrentAudioSource.volume - increment >= 0)
            {
                m_Instance.m_CurrentAudioSource.volume -= increment;
                yield return new WaitForSecondsRealtime(0.33f);
            }
        }

        public static void StopAmbience()
        {
            m_Instance.m_AudioSource1.Stop();
            m_Instance.m_AudioSource2.Stop();
        }

        public static void PlayAmbience()
        {
            m_Instance.m_CurrentAudioSource.Play();
        }
    }
}