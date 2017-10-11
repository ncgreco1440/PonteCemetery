using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class SetGraveSynapseWall : MonoBehaviour
    {
        public Texture[] m_Textures;
        public Material m_EndMaterial;

        public void OnTriggerEnter()
        {
            if (SetGrave.Instance.CurrentStage() < 2)
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);

            if(SetGrave.Instance.CurrentStage() == 2)
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[0]);

            if(SetGrave.Instance.CurrentStage() == 3)
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[1]);

            if (SetGrave.Instance.CurrentStage() == 4)
            {
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);
                SetGrave.Instance.IncrementStage();
            }
        }
    }
}