using Overtop.Managers;
using PonteCemetery.GamePlay.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class TheWalk : GameEvent
    {
        private static TheWalk m_Instance;
        public Rigidbody m_GateRigidbody;
        public WoodenDoor m_Gate;
        public MetalGate m_MetalGate;
        public BoxCollider m_BeginWalkTrigger;
        public BoxCollider m_EndWalkTrigger;
        public BoxCollider m_EntrapTrigger;

        public Material[] m_LightBulbMaterials;
        public MeshRenderer[] m_LightMeshes;
        public Light[] m_Lights;
        public AudioSource[] m_Laughs;

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
                Audio.Ambience.Instance.StartCoroutine(Audio.Ambience.BeginAmbience(2));
                IncrementStage();
                m_MetalGate.Reset();
                m_GateRigidbody.drag = 1;
                StartCoroutine(RepeatedGateSlamming());
                StartCoroutine(FlickeringLights());
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

        private IEnumerator FlickeringLights()
        {
            while(CurrentStage() == 1)
            {
                for (int i = 0; i < m_Lights.Length; i++)
                {
                    m_Lights[i].enabled = true;
                    m_LightMeshes[i].material = m_LightBulbMaterials[0];
                }
                    
                yield return new WaitForSeconds(0.25f);

                for (int i = 0; i < m_Lights.Length; i++)
                {
                    m_Lights[i].enabled = false;
                    m_LightMeshes[i].material = m_LightBulbMaterials[1];
                }

                yield return new WaitForSeconds(0.25f);
            }

            StartCoroutine(FadeLights());
        }

        private IEnumerator FadeLights()
        {
            for (int i = 0; i < m_Lights.Length; i++)
            {
                m_Lights[i].intensity = 0.0f;
                m_Lights[i].enabled = true;
                m_LightMeshes[i].material = m_LightBulbMaterials[0];
            }
                
            while (m_Lights[0].intensity < 2.25f)
            {
                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < m_Lights.Length; i++)
                    m_Lights[i].intensity += 0.12f;
            }
        }

        public override void ResetStages()
        {
            base.ResetStages();
        }
    }
}