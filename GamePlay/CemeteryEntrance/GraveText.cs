using PonteCemetery.GamePlay.Interactables;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class GraveText : Grave
    {
        [SerializeField]
        private GraveMessage m_SubmittingMessage;
        [SerializeField]
        private GraveMessage m_AnsweringMessage;

        [SerializeField]
        private GraveMessage[] m_AnsweringMessages;

        public List<string> m_Words;
        public List<Texture> m_Textures;

        private int m_CurrentIndex = 0;

        public void Awake()
        {
            m_Collider = GetComponent<BoxCollider>();
        }

        public override void Interact()
        {
            GraveTextAssembly.SubmitWord(m_Words[m_CurrentIndex], m_Textures[m_CurrentIndex]);
            m_SubmittingMessage.FadeOut();
            //m_AnsweringMessages[m_CurrentIndex].SetTexture(m_SubmittingMessage.GetTexture());
            //m_AnsweringMessages[m_CurrentIndex].SetStrength(0.0f);
            //m_AnsweringMessages[m_CurrentIndex].FadeIn();
            Disable();
        }

        public void MakeNextWordCurrent()
        {
            Enable();
            m_SubmittingMessage.SetTexture(m_Textures[++m_CurrentIndex]);
            m_SubmittingMessage.FadeIn();
        }

        public void Reset()
        {
            Enable();
            m_SubmittingMessage.FadeIn();
        }
    }
}