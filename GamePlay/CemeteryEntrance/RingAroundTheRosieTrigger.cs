using System.Collections;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class RingAroundTheRosieTrigger : MonoBehaviour
    {
        public RingAroundTheRosieTrigger m_Adjacent;
        public Material m_Material;
        public Material[] m_StartingMaterials;
        public Texture m_DefaultMaterial;
        public Texture[] m_Textures;
        public bool m_Next = false;
        public bool m_Start = false;
        public int m_Iter = -1;
        public MetalGate m_MetalGate;
        public AudioSource m_ChildrenTalking;
        public MetalGate m_MetalGateToTheWalk;

        [SerializeField]
        private bool m_EnteredZone = false;

        private void Start()
        {
            m_DefaultMaterial = m_Material.GetTexture("_NameNormal");
            m_Material.SetFloat("_NameNormalStrength", 3f);
        }

        /// <summary>
        /// The player will trigger the ability for Analyze approaching the grave
        /// </summary>
        /// <param name="collider"></param>
        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.CompareTag("Player"))
            {
                m_EnteredZone = true;
            }
        }

        /// <summary>
        /// The player will trigger inability to Analyze when leaving the grave
        /// </summary>
        /// <param name="collider"></param>
        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                m_EnteredZone = false;
            }
        }

        private void FixedUpdate()
        {
            if (!RingAroundTheRosie.Instance.Resetting() && 
                m_EnteredZone && 
                !RingAroundTheRosie.Instance.Completed() &&
                RingAroundTheRosie.Instance.Active)
                Analyze();
        }

        private void OnDestroy()
        {
            m_Material.SetTexture("_NameNormal", m_DefaultMaterial);
            m_Material.SetFloat("_NameNormalStrength", 3f);
        }

        /// <summary>
        /// Analyzes to see if the trigger was the Next trigger in the Ring Around the Rosy 
        /// sequence. Should only run once per OnTriggerEnter.
        /// </summary>
        private void Analyze()
        {
            if (m_Start)
            {
                StartCoroutine(StartPlaying());
                m_StartingMaterials[1].SetTexture("_NameNormal", m_Textures[1]);
                m_Start = false;
                m_Iter += 2;
            }

            if (m_Next)
            {
                m_Next = false;
                m_Adjacent.m_Next = true;
                RingAroundTheRosie.Instance.IncrementStage();

                if (RingAroundTheRosie.Instance.CurrentStage() == 17)
                {
                    m_MetalGate.ForceOpen();
                    m_ChildrenTalking.loop = false;
                    m_MetalGateToTheWalk.Unlock();
                    Audio.Ambience.SwapAmbience(0, 0.15f);
                }

                if (m_Iter + 1 <= m_Textures.Length - 1)
                    m_Material.SetTexture("_NameNormal", m_Textures[++m_Iter]);
                else
                    m_Material.SetTexture("_NameNormal", null);
            }
            else
            {
                if (RingAroundTheRosie.Instance.CurrentStage() > 0)
                {
                    RingAroundTheRosie.Instance.Active = false;
                    RingAroundTheRosie.Instance.SoftResetStages();
                }
            }
            m_EnteredZone = false;
        }

        public IEnumerator StartPlaying()
        {
            float strength = 3;

            while (strength >= 0)
            {
                yield return new WaitForSeconds(0.16f);
                strength -= 0.35f;
                m_StartingMaterials[0].SetFloat("_NameNormalStrength", strength);
            }
            m_StartingMaterials[0].SetTexture("_NameNormal", m_Textures[0]);
            while(strength <= 3)
            {
                yield return new WaitForSeconds(0.16f);
                strength += 0.35f;
                m_StartingMaterials[0].SetFloat("_NameNormalStrength", strength);
            }
        }

        public IEnumerator FadeOut()
        {
            float strength = 3;

            while (strength >= 0)
            {
                yield return new WaitForSeconds(0.16f);
                strength -= 0.35f;
                m_Material.SetFloat("_NameNormalStrength", strength);
            }
            m_Material.SetTexture("_NameNormal", m_DefaultMaterial);
            while (strength <= 3)
            {
                yield return new WaitForSeconds(0.16f);
                strength += 0.35f;
                m_Material.SetFloat("_NameNormalStrength", strength);
            }
            m_Iter = -1;
        }

        public void ResetTrigger()
        {
            m_Material.SetTexture("_NameNormal", m_DefaultMaterial);
            m_Iter = -1;
        }

        public void EnteredZone(bool b)
        {
            m_EnteredZone = b;
        }
    }
}