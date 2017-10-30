using PonteCemetery.GamePlay.Interactables;
using System.Collections;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class TheWalk : GameEvent
    {
        private static TheWalk m_Instance;
        public Rigidbody m_GateRigidbody;
        public WoodenDoor m_Gate;
        public MetalGate m_MetalGate;
        //public BoxCollider m_BeginWalkTrigger;
        //public BoxCollider m_EndWalkTrigger;
        //public BoxCollider m_EntrapTrigger;

        public FlickeringLights m_FlickerLights;
        public AudioSource[] m_Laughs;

        public AudioClip[] m_Sounds;

        private void Awake()
        {
            m_Instance = this;
        }

        private void Start()
        {
           
        }

        public static TheWalk Instance()
        {
            return m_Instance;
        }

        public void BeginWalk()
        {
            if(CurrentStage() == 0)
            {
                Audio.Ambience.SwapAmbience(2, 0.25f);
                IncrementStage();
                m_MetalGate.ForceClose();
                m_MetalGate.Lock();
                m_GateRigidbody.drag = 1;
                m_Laughs[2].Play();
                StartCoroutine(RepeatedGateSlamming());
            }
        }

        public override void IncrementStage()
        {
            base.IncrementStage();
            if(CurrentStage() == 1)
            {
                m_FlickerLights.BeginFlickering(m_Sounds[0]);
            }
            if(CurrentStage() == 2)
            {
                m_FlickerLights.EndFlickering(m_Sounds[0]);
            }
        }

        public void Entrap()
        {
            if(CurrentStage() == 4)
            {
                IncrementStage();
                m_Gate.SlamShut();
            }   
        }

        public void Laugh(int i)
        {
            m_Laughs[i].Play();
        }

        private IEnumerator RepeatedGateSlamming()
        {
            yield return new WaitForSeconds(5.0f);
            while(CurrentStage() == 1)
            {
                m_Gate.ForceOpen();
                yield return new WaitForSeconds(0.5f);
                m_Gate.SlamShut();
                yield return new WaitForSeconds(1.0f);
            }
            m_Gate.ForceOpen();
        }

        public override void ResetStages()
        {
            base.ResetStages();
        }
    }
}