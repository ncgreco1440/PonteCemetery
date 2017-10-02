using System.Collections;
using UnityEngine;

namespace PonteCemetery
{
    public class GraveIndicator : MonoBehaviour
    {
        public Material m_Material;
        public static Texture[] m_Textures;
        public Texture m_DefaultTexture;

        public float m_CurrentStrength = 0.0f;
        public bool m_Indicating = false;
        public bool m_Defaulting = false;
        public Texture m_TextureToIndicate;

        public WaitForSeconds m_WaitFor60FPSTick = new WaitForSeconds(0.16f);
        public WaitForSeconds m_WaitFor30FPSTick = new WaitForSeconds(0.5f);
        [Range(0,6)]
        public int m_Minimum = 0;
        [Range(0,6)]
        public int m_Maximum = 7;

        private void Start()
        {
            if(m_Textures == null)
            {
                m_Textures = new Texture[]{
                    (Texture)Resources.Load("Textures/Lost/Lost"),
                    (Texture)Resources.Load("Textures/Lost/Leave"),
                    (Texture)Resources.Load("Textures/Lost/Shouldnt_Be_Here"),
                    (Texture)Resources.Load("Textures/Lost/GetOut"),
                    (Texture)Resources.Load("Textures/Lost/LeaveNow"),
                    (Texture)Resources.Load("Textures/Lost/Only_Sorrow_Awaits"),
                    (Texture)Resources.Load("Textures/Lost/Linger_Not_Flee")
                };
            }

            m_DefaultTexture = m_Material.GetTexture("_NameNormal");
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "LostTrigger")
            {
                m_TextureToIndicate = m_Textures[Random.Range(m_Minimum, m_Maximum)];
                StartCoroutine(DisplayIndicator());
            }
        }

        private void OnTriggerExit(Collider collider)
        { 
            if (collider.gameObject.tag == "LostTrigger")
            {
                m_TextureToIndicate = m_DefaultTexture;
                StartCoroutine(DisplayIndicator());
            }
        }

        private void OnDestroy()
        {
            m_Material.SetTexture("_NameNormal", m_DefaultTexture);
            m_Material.SetFloat("_NameNormalStrength", 3f);
        }

        private IEnumerator DisplayIndicator()
        {
            yield return Reset();
            m_Indicating = true;
            yield return Dissolve();
            m_Material.SetTexture("_NameNormal", m_TextureToIndicate);
            yield return Resolve();
            m_Indicating = false;
        }

        private IEnumerator Dissolve()
        {
            m_CurrentStrength = m_Material.GetFloat("_NameNormalStrength");
            while (m_Indicating && m_Material.GetFloat("_NameNormalStrength") > 0f)
            {
                m_CurrentStrength -= 0.35f;
                m_Material.SetFloat("_NameNormalStrength", m_CurrentStrength);
                yield return m_WaitFor60FPSTick;
            }
        }

        private IEnumerator Resolve()
        {
            m_CurrentStrength = m_Material.GetFloat("_NameNormalStrength");
            while (m_Indicating && m_Material.GetFloat("_NameNormalStrength") < 3f)
            {
                m_CurrentStrength += 0.35f;
                m_Material.SetFloat("_NameNormalStrength", m_CurrentStrength);
                yield return m_WaitFor60FPSTick;
            }
        }

        private IEnumerator Reset()
        {
            m_Indicating = false;
            yield return m_WaitFor30FPSTick;
        }
    }
}