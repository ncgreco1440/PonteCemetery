using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class AudioExperience : MonoBehaviour
    {
        private static AudioExperience m_Instance;
        private AudioSource m_AudioSource;

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_Instance = this;
        }

        public static AudioSource AudioSource()
        {
            return m_Instance.m_AudioSource;
        }

        public static void PlayAudioClip(AudioClip clip)
        {
            m_Instance.m_AudioSource.clip = clip;
            m_Instance.m_AudioSource.Play();
        }
    }
}