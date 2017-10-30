using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtop.Lights
{
    public class OvertopLight : MonoBehaviour
    {
        [SerializeField]
        private Material m_OnMaterial;
        [SerializeField]
        private Material m_OffMaterial;
        [SerializeField]
        private float m_OnIntensity = 1.0f;
        [SerializeField]
        private Light m_Light;
        [SerializeField]
        private MeshRenderer m_BulbMesh;

        public void TurnOn()
        {
            m_BulbMesh.material = m_OnMaterial;
            m_Light.intensity = m_OnIntensity;
            m_Light.enabled = true;
        }

        public void TurnOff()
        {
            m_BulbMesh.material = m_OffMaterial;
            m_Light.intensity = 0.0f;
            m_Light.enabled = false;
        }

        public void SetIntensity(float intensity)
        {
            m_OnIntensity = intensity;
            m_Light.intensity = intensity;
        }

        public float GetIntensity()
        {
            return m_OnIntensity;
        }
    }
}