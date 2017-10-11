using PonteCemtery.GamePlay.Interactables;
using System.Collections;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class RingAroundTheRosie : GameEvent
    {
        public Material m_LetsPlay;
        public Texture m_LetsPlayTexture;
        public static RingAroundTheRosie Instance;
        public RingAroundTheRosieTrigger m_StartingTrigger;
        public bool m_CanBegin = false;
        public SOSLight m_light;
        public bool m_Resetting = false;

        public RingAroundTheRosieTrigger[] m_Triggers;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_Stage = 0;
        }

        public void BeginEvent()
        {
            if(m_CanBegin)
            {
                m_light.TurnOn();
                m_StartingTrigger.m_Start = true;
                m_StartingTrigger.m_Next = true;
                m_CanBegin = false;
                m_LetsPlay.SetTexture("_NameNormal", m_LetsPlayTexture);
            }
        }

        /// <summary>
        /// Reset the puzzle if Player fails the order
        /// </summary>
        public void SoftResetStages()
        {
            m_Resetting = true;
            base.ResetStages();
            StartCoroutine(ResetAllGraves());
            //m_StartingTrigger.m_Start = true;
            //m_StartingTrigger.m_Next = true;
        }

        private IEnumerator ResetAllGraves()
        {
            StartCoroutine(m_Triggers[0].FadeOut());
            StartCoroutine(m_Triggers[1].FadeOut());
            StartCoroutine(m_Triggers[2].FadeOut());
            StartCoroutine(m_Triggers[3].FadeOut());
            yield return new WaitForSeconds(5.0f);
            m_Resetting = false;
            m_LetsPlay.SetTexture("_NameNormal", m_LetsPlayTexture);
            m_StartingTrigger.m_Start = true;
            m_StartingTrigger.m_Next = true;
        }

        public bool Resetting()
        {
            return m_Resetting;
        }

        public override void ResetStages()
        {
            base.ResetStages();
            m_light.Reset();
            m_Triggers[0].ResetTrigger();
            m_Triggers[1].ResetTrigger();
            m_Triggers[2].ResetTrigger();
            m_Triggers[3].ResetTrigger();
            m_StartingTrigger.m_Start = false;
            m_StartingTrigger.m_Next = false;
            m_CanBegin = false;
        }
    }
}