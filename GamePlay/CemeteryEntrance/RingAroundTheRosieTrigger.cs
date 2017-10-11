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

        private void Start()
        {
            m_DefaultMaterial = m_Material.GetTexture("_NameNormal");
            m_Material.SetFloat("_NameNormalStrength", 3f);
        }

        private void OnTriggerEnter()
        {
            if(!RingAroundTheRosie.Instance.Resetting())
            {
                if (m_Start)
                {
                    StartCoroutine(StartPlaying());
                    m_StartingMaterials[1].SetTexture("_NameNormal", m_Textures[1]);
                    m_Start = false;
                    m_Iter += 2;
                    if (!m_ChildrenTalking.isPlaying)
                        m_ChildrenTalking.Play();
                }

                if (m_Next)
                {
                    m_Next = false;
                    m_Adjacent.m_Next = true;
                    RingAroundTheRosie.Instance.IncrementStage();

                    if (RingAroundTheRosie.Instance.CurrentStage() == 21)
                    {
                        m_MetalGate.ForceOpen();
                        m_ChildrenTalking.loop = false;
                        m_MetalGateToTheWalk.Unlock();
                        //Audio.Ambience.Instance.StartCoroutine(Audio.Ambience.BeginAmbience(0));
                    }

                    if (m_Iter + 1 <= m_Textures.Length - 1)
                        m_Material.SetTexture("_NameNormal", m_Textures[++m_Iter]);
                    else
                        m_Material.SetTexture("_NameNormal", null);
                }
                else
                {
                    if (RingAroundTheRosie.Instance.CurrentStage() > 0)
                        RingAroundTheRosie.Instance.SoftResetStages();
                }
            }
        }

        private void OnDestroy()
        {
            m_Material.SetTexture("_NameNormal", m_DefaultMaterial);
            m_Material.SetFloat("_NameNormalStrength", 3f);
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
    }
}