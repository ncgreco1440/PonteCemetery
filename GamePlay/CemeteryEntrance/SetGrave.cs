using Overtop.Scripts.Interactables;
using System.Collections;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class SetGrave : MonoBehaviour
    {
        public static SetGrave Instance;
        public static int Stage = 0;

        public AudioClip[] m_AudioClips;
        public Animator m_Animator;
        public Material m_StartMaterial;
        public Material m_EndMaterial;
        public BoxCollider m_TriggerCollider;
        public AudioSource m_AudioSource;
        public Texture[] m_Textures;

        private Transform m_Transform;
        [SerializeField]
        private bool m_Triggered = false;
        [SerializeField]
        private bool m_PlayerNear = false;
        [SerializeField]
        private bool m_Completed = false;
        private float m_Angle;
        private float m_Angle2;
        private bool m_Breathing = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_Transform = GetComponent<Transform>();
            m_Animator.SetBool("Triggered", m_Triggered);
            m_Animator.SetBool("Completed", false);
        }

        public void OnTriggerEnter()
        {
            switch(Stage)
            {
                case 2:
                    {
                        StageTwo();
                        break;
                    }
                default:
                    {
                        StageZero();
                        break;
                    }
            }
        }

        public void OnTriggerExit()
        {
            m_PlayerNear = false;
        }

        private void FixedUpdate()
        {
            if (m_PlayerNear && !m_Completed)
            {
                m_Angle = Vector3.Angle(Player.Transform().forward, m_Transform.forward);

                if (m_Angle >= 130f)
                {
                    m_EndMaterial.SetTexture("_NameNormal", m_Textures[1]);

                    m_Completed = true;
                    Stage = 1;
                    SetGravePt3.BeginVoices();
                    PlaySound(1);
                }
            }
        }

        private void StageZero()
        {
            if (!m_Triggered)
            {
                ResizeTrigger();
                PlaySound(2);
                m_Triggered = true;
                m_Animator.SetBool("Triggered", m_Triggered);
                m_EndMaterial.SetTexture("_NameNormal", m_Textures[0]);
            }
            else
            {
                m_PlayerNear = true;
                m_AudioSource.spatialBlend = 1f;
            }
        }

        private void StageTwo()
        {
            SetGravePt3.EndVoices();
            Stage = 3;
        }

        private IEnumerator Breathe()
        {
            m_Breathing = true;
            Player.SetSoundFromBehind(m_AudioClips[0]);
            while(!m_Completed)
            {
                if (!m_Completed)
                    Player.PlaySoundFromBehind();
                yield return new WaitForSeconds(3.65f);
            }
        }

        private void ResizeTrigger()
        {
            m_TriggerCollider.size = new Vector3(0.05f, 0.05f, 0.05f);
        }

        private void PlaySound(int index)
        {
            m_AudioSource.clip = m_AudioClips[index];
            m_AudioSource.Play();
        }

        public static bool Completed
        {
            get
            {
                return Instance.m_Completed;
            }
        }
    }
}