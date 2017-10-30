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
        [SerializeField]
        private bool m_Active = false;

        public RingAroundTheRosieTrigger[] m_Triggers;
        public RingAroundTheRosieActivate m_Activator;

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
                m_Activator.gameObject.SetActive(true);
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
        }

        private IEnumerator ResetAllGraves()
        {
            StartCoroutine(m_Triggers[0].FadeOut());
            StartCoroutine(m_Triggers[1].FadeOut());
            StartCoroutine(m_Triggers[2].FadeOut());
            StartCoroutine(m_Triggers[3].FadeOut());
            m_Triggers[0].EnteredZone(false);
            m_Triggers[1].EnteredZone(false);
            m_Triggers[2].EnteredZone(false);
            m_Triggers[3].EnteredZone(false);
            yield return StartCoroutine(BeginAfterTime());
            m_LetsPlay.SetTexture("_NameNormal", m_LetsPlayTexture);
            m_Activator.gameObject.SetActive(true);
        }

        private IEnumerator BeginAfterTime()
        {
            yield return new WaitForSeconds(5.0f);
            m_StartingTrigger.m_Start = true;
            m_StartingTrigger.m_Next = true;
            m_Resetting = false;
            m_StartingTrigger.EnteredZone(true);
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

        public bool Active
        {
            get { return m_Active; }
            set
            {
                if (!value)
                    m_Activator.Enable();
                m_Active = value;
            }
        }
    }
}