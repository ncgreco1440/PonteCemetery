using UnityEngine;

namespace PonteCemetery.Utility
{
    [ExecuteInEditMode]
    public class BorderFence : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Fence;
        [SerializeField]
        private GameObject m_Column;

        private GameObject m_seg;
        private float m_prevLoc;

        [SerializeField]
        private float m_Rotation = 0.0f;

        private void Start()
        {
            int i = 0;
            for (; i < 168; i++)
            {
                CreateFence(i);
                CreateColumn(i);
            }
            CreateColumn(i);
        }

        private void CreateColumn(int i)
        {
            m_seg = Instantiate(m_Column);
            m_seg.transform.SetParent(gameObject.transform);
            Vector3 vec3 = Vector3.zero;
            m_seg.transform.localPosition = vec3;

            if (m_Rotation != 0)
                m_seg.transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (i != 0)
                vec3.x += (2.87f * i);
            m_seg.transform.localPosition = vec3;
        }

        private void CreateFence(int i)
        {
            m_seg = Instantiate(m_Fence);
            m_seg.transform.SetParent(gameObject.transform);
            Vector3 vec3 = Vector3.zero;
            m_seg.transform.localPosition = vec3;
            if(m_Rotation != 0)
                m_seg.transform.localRotation = Quaternion.Euler(-m_Rotation, 0, 0);

            if (i != 0)
                vec3.x += (2.87f * i) + 1.433f;
            else
                vec3.x += 1.433f;
            m_seg.transform.localPosition = vec3;
        }
    }
}