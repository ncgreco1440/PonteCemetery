using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class SetGraveSynapseWall : MonoBehaviour
    {
        public Texture[] m_Textures;
        public Material m_EndMaterial;

        private bool m_Revert = false;
        private int m_RecalledStage = 0;

        public void OnTriggerEnter()
        {
            //if (ShouldRevert())
            //{
            //    switch(m_RecalledStage)
            //    {
            //        case 3:
            //        {
            //            m_EndMaterial.SetTexture("_NameNormal", m_Textures[0]);
            //            break;
            //        }
            //        case 4:
            //        {
            //            m_EndMaterial.SetTexture("_NameNormal", m_Textures[1]);
            //            break;
            //        }
            //        default:
            //        {
            //            m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    m_RecalledStage = SetGrave.Instance.CurrentStage();
            //    Stuff();
            //}
            if (SetGrave.Instance.CurrentStage() == 2)
            {
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);
                SetGrave.Instance.IncrementStage();
            }
        }

        private bool ShouldRevert()
        {
            return SetGrave.Instance.CurrentStage() == m_RecalledStage;
        }

        private void Stuff()
        {
            if (SetGrave.Instance.CurrentStage() < 2)
            {
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);
            }

            if (SetGrave.Instance.CurrentStage() == 2)
            {
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);
            }

            if (SetGrave.Instance.CurrentStage() == 3)
            {
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[0]);
            }

            if (SetGrave.Instance.CurrentStage() == 4)
            {
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);
                SetGrave.Instance.IncrementStage();
            }
        }
    }
}