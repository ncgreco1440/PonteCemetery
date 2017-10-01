using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class SetGraveSynapseWall : MonoBehaviour
    {
        public Texture[] m_Textures;
        public Material m_EndMaterial;

        public void OnTriggerEnter()
        {
            if (SetGrave.Stage == 0)
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);

            if(SetGrave.Stage == 1)
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[0]);

            if(SetGrave.Stage == 2)
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[1]);

            if (SetGrave.Stage == 3)
            {
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[2]);
                SetGrave.Stage = 4;
            }
        }
    }
}