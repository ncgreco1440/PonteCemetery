using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    /// <summary>
    /// Each answer corresponds to the valid sentence based on index.
    /// </summary>
    public class GraveTextAssembly : MonoBehaviour
    {
        private static GraveTextAssembly m_Instance;

        public List<GraveText> m_SubmissionGraves;
        public List<GraveMessage> m_AnsweringGraves;
        public List<GraveMessage> m_SubmissionGraves1;

        public List<string> m_ValidSentences;
        public List<string> m_Answers;
        public List<Texture> m_TextureAnswers;

        [SerializeField]
        private List<string> m_SubmittedWords = new List<string>();
        [SerializeField]
        private int solved = -1;
        [SerializeField]
        private bool m_CompletedSubmission = false;
        [SerializeField]
        private AudioSource m_AudioSource;
        [SerializeField]
        private AudioClip m_CorrectAnswerSound;
        [SerializeField]
        private AudioClip m_IncorrectAnswerSound;
        [SerializeField]
        private FlickeringLights m_FlickeringLights;
        [SerializeField]
        private int m_Index = 0;

        private void Awake()
        {
            m_Instance = this;
        }

        public static void SubmitWord(string str, Texture tex)
        {
            if (!m_Instance.m_SubmittedWords.Contains(str) && m_Instance.m_SubmittedWords.Count < 3)
            {
                m_Instance.m_SubmittedWords.Add(str);
                m_Instance.m_AnsweringGraves[m_Instance.m_Index].SetTexture(tex);
                m_Instance.m_AnsweringGraves[m_Instance.m_Index].SetStrength(0.0f);
                m_Instance.m_AnsweringGraves[m_Instance.m_Index].FadeIn();
                m_Instance.m_Index++;
            }

            if (m_Instance.m_SubmittedWords.Count == 3)
                m_Instance.m_CompletedSubmission = true;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.CompareTag("Player") && m_CompletedSubmission)
            {
                if (ValidateSubmission())
                    Continue();
                else
                    Back();
                m_Index = 0;
                m_SubmittedWords.Clear();
                m_Instance.m_CompletedSubmission = false;
            }
        }

        private bool ValidateSubmission()
        {
            string str = string.Join(" ", m_SubmittedWords.ToArray());
            return m_ValidSentences.Contains(str);
        }

        private void Continue()
        {
            GameEventRun.Instance.IncrementStage();
            m_AnsweringGraves[0].FadeOut();
            m_AnsweringGraves[2].FadeOut();
            m_AnsweringGraves[1].AutoTextureFade(m_TextureAnswers[++solved]);
            if (GameEventRun.Instance.CurrentStage() < 4)
                m_SubmissionGraves.ForEach((g) => { g.MakeNextWordCurrent(); });
            PlayCorrectAnswerSound();
        }

        private void Back()
        {
            PlayIncorrectAnswerSound();
            m_SubmissionGraves.ForEach((g) => { g.Reset(); });
            m_AnsweringGraves.ForEach((g) => { g.FadeOut(); });
        }

        private void PlayCorrectAnswerSound()
        {
            m_AudioSource.clip = m_CorrectAnswerSound;
            m_AudioSource.Play();
        }

        private void PlayIncorrectAnswerSound()
        {
            m_AudioSource.clip = m_IncorrectAnswerSound;
            m_AudioSource.Play();
        }

        /// <summary>
        /// Begins the Terror Portion of the Ouiji Sequence
        /// </summary>
        public void BeginTerror()
        {
            StartCoroutine(Terror());
        }

        private IEnumerator Terror()
        {
            yield return new WaitForSeconds(3.0f);
            m_AnsweringGraves[1].AutoTextureFade(m_TextureAnswers[3]);           
        }

        public void Terrorize()
        {
            m_AnsweringGraves.ForEach((g) =>
            {
                g.PrepDefaultTexture(m_TextureAnswers[2]);
                g.SetFadingSpeed(GraveMessage.FadingSpeed.Speed.VeryFast);
            });
            m_SubmissionGraves1.ForEach((g) =>
            {
                g.PrepDefaultTexture(m_TextureAnswers[2]);
                g.SetFadingSpeed(GraveMessage.FadingSpeed.Speed.VeryFast);
            });
            m_AnsweringGraves[0].AutoTextureFadeRepeating(m_TextureAnswers[3]);
            m_AnsweringGraves[1].AutoTextureFadeRepeating(m_TextureAnswers[3]);
            m_AnsweringGraves[2].AutoTextureFadeRepeating(m_TextureAnswers[3]);
            m_SubmissionGraves1[0].AutoTextureFadeRepeating(m_TextureAnswers[3]);
            m_SubmissionGraves1[1].AutoTextureFadeRepeating(m_TextureAnswers[3]);
            m_SubmissionGraves1[2].AutoTextureFadeRepeating(m_TextureAnswers[3]);
        }
    }
}