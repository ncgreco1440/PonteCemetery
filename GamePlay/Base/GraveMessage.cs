using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class GraveMessage : MonoBehaviour
    {
        [System.Serializable]
        public class FadingSpeed
        {
            [SerializeField]
            private Speed m_Speed;

            public enum Speed
            {
                Slow = -1,
                Normal = 1,
                Fast = 2,
                VeryFast = 3
            }

            public float FadeSpeed()
            {
                if (m_Speed == Speed.Slow)
                    return (0.35f * ((float)m_Speed - 0.5f));
                return (0.35f * (float)m_Speed);
            }

            public void SetSpeed(Speed speed)
            {
                m_Speed = speed;
            }
        }

        private GraveMessage m_Instance;
        [SerializeField]
        private bool m_Fading = false;
        private Texture m_CurrentTexture = null;
        private WaitForSeconds m_WaitForSeconds;

        [SerializeField]
        private Material m_Material;
        [SerializeField]
        private Texture m_DefaultTexture;
        [SerializeField]
        private FadingSpeed m_FadeSpeed;

        [SerializeField]
        private float m_Strength = 3.0f;

        private void Awake()
        {
            m_Instance = this;
            SetDefaultTexture();
        }

        private void Start()
        {
            m_WaitForSeconds = new WaitForSeconds(0.16f);
        }

        private void OnDestroy()
        {
            m_Material.SetTexture("_NameNormal", null);
            m_Material.SetFloat("_NameNormalStrength", 3.0f);
        }

        public GraveMessage Instance
        {
            get { return m_Instance; }
        }

        private IEnumerator FadeMessageOut()
        {
            m_Fading = true;
            while(m_Fading)
            {
                if (m_Strength - m_FadeSpeed.FadeSpeed() < 0.0f)
                {
                    m_Strength = 0.0f;
                    m_Fading = false;
                }
                else
                    m_Strength -= m_FadeSpeed.FadeSpeed();
                m_Material.SetFloat("_NameNormalStrength", m_Strength);
                yield return m_WaitForSeconds;
            }
        }

        /// <summary>
        /// Invokes the internal FadeMessageOut Coroutine
        /// </summary>
        public void FadeOut()
        {
            if(!IsFading())
                StartCoroutine(FadeMessageOut());
        }

        private IEnumerator FadeMessageIn()
        {
            m_Fading = true;
            while(m_Fading)
            {
                if (m_Strength + m_FadeSpeed.FadeSpeed() > 3.0f)
                {
                    m_Strength = 3.0f;
                    m_Fading = false;
                }  
                else
                    m_Strength += m_FadeSpeed.FadeSpeed();
                m_Material.SetFloat("_NameNormalStrength", m_Strength);
                yield return m_WaitForSeconds;
            }
        }

        /// <summary>
        /// Invokes the internal FadeMessageIn Coroutine
        /// </summary>
        public void FadeIn()
        {
            if(!IsFading())
                StartCoroutine(FadeMessageIn());
        }

        /// <summary>
        /// Invokes the internal reset coroutine from a public method
        /// </summary>
        public void ResetToDefaultMessage()
        {
            StartCoroutine(AutoFadeOutAndIn());
        }

        /// <summary>
        /// Auto triggers FadeOut, a Texture swap, and then a FadeIn
        /// </summary>
        /// <param name="texture"></param>
        public void AutoTextureFade(Texture texture)
        {
            StartCoroutine(AutoFadeOutAndIn(texture));
        }

        /// <summary>
        /// Repeatedly fades in and out between the default texture and the specified texture
        /// </summary>
        /// <param name="texture"></param>
        public void AutoTextureFadeRepeating(Texture texture)
        {
            StartCoroutine(AutoFadeOutAndInRepeating(texture));
        }

        private IEnumerator AutoFadeOutAndInRepeating(Texture texture)
        {
            while(true)
            {
                yield return StartCoroutine(FadeMessageOut());
                if (m_CurrentTexture == texture) SetDefaultTexture(); else SetTexture(texture);
                yield return StartCoroutine(FadeMessageIn());
            }
        }

        private IEnumerator AutoFadeOutAndIn()
        {
            yield return StartCoroutine(FadeMessageOut());
            SetDefaultTexture();
            yield return StartCoroutine(FadeMessageIn());
        }

        private IEnumerator AutoFadeOutAndIn(Texture texture)
        {
            yield return StartCoroutine(FadeMessageOut());
            SetTexture(texture);
            StartCoroutine(FadeMessageIn());
        }

        /// <summary>
        /// Sets the current texture of the material
        /// </summary>
        /// <param name="tex"></param>
        public void SetTexture(Texture tex)
        {
            m_CurrentTexture = tex;
            m_Material.SetTexture("_NameNormal", m_CurrentTexture);
        }

        /// <summary>
        /// Sets the current texture equal to the default texture
        /// </summary>
        public void SetDefaultTexture(Texture texture = null)
        {
            if(texture != null)
                m_DefaultTexture = texture;
            SetTexture(m_DefaultTexture);
        }

        public void PrepDefaultTexture(Texture texture)
        {
            m_DefaultTexture = texture;
        }

        /// <summary>
        /// Returns the current texture
        /// </summary>
        /// <returns></returns>
        public Texture GetTexture()
        {
            return m_CurrentTexture;
        }

        /// <summary>
        /// Sets the _NameNormalStrength of the GraveMessage
        /// </summary>
        /// <param name="strength"></param>
        public void SetStrength(float strength)
        {
            m_Strength = strength;
            m_Material.SetFloat("_NameNormalStrength", m_Strength);
        }

        /// <summary>
        /// Returns true if the Grave is fade out or in a message
        /// </summary>
        public bool IsFading()
        {
            return m_Fading;
        }

        public void SetFadingSpeed(FadingSpeed.Speed speed)
        {
            m_FadeSpeed.SetSpeed(speed);
        }
    }
}