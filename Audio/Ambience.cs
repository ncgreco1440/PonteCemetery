using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace PonteCemetery.Audio
{
    public class Ambience : MonoBehaviour
    {
        private static Ambience m_Instance;
        public AudioSource m_AudioSource;
        public List<AudioClip> m_AmbientSounds;

        private void Awake()
        {
            m_Instance = this;
        }

        public static Ambience Instance
        {
            get
            {
                return m_Instance;
            }
        }

        public static IEnumerator BeginAmbience(int index)
        {
            yield return m_Instance.StartCoroutine(EndAmbience());
            m_Instance.m_AudioSource.clip = m_Instance.m_AmbientSounds[index];
            m_Instance.m_AudioSource.Play();
            while (m_Instance.m_AudioSource.volume + 0.05f <= 1)
            {
                m_Instance.m_AudioSource.volume += 0.05f;
                yield return new WaitForSecondsRealtime(0.33f);
            }
        }

        public static IEnumerator EndAmbience()
        {
            while (m_Instance.m_AudioSource.volume - 0.05f >= 0)
            {
                m_Instance.m_AudioSource.volume -= 0.05f;
                yield return new WaitForSecondsRealtime(0.33f);
            }
        }
    }
}