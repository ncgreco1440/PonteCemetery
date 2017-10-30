using Overtop.Scripts.Interactables;
using System.Collections;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class SetGrave : GameEvent
    {
        public static SetGrave Instance;
        //public static int Stage = 0;

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
        private float m_Angle;
        private float m_Angle2;
        private bool m_Breathing = false;
        private Vector3 m_OriginalSize;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_Transform = GetComponent<Transform>();
            m_Animator.SetBool("Triggered", m_Triggered);
            m_Animator.SetBool("Completed", false);
            m_OriginalSize = m_TriggerCollider.size;
        }

        public void OnTriggerEnter()
        {
            if(m_Stage > 0)
            {
                m_PlayerNear = true;
                m_AudioSource.spatialBlend = 1f;
            }

            //if (m_Stage == 3) // Do not run this...
            //    StageThree(); // There is no Stage 3 on this grave...
            if (m_Stage == 0)
                StageZero();
        }

        public void OnTriggerExit()
        {
            m_PlayerNear = false;
        }

        private void FixedUpdate()
        {
            if (m_PlayerNear && m_Stage == 1)
            {
                m_Angle = Vector3.Angle(Player.Transform().forward, m_Transform.forward);
                if (m_Angle >= 130f)
                {
                    m_EndMaterial.SetTexture("_NameNormal", m_Textures[1]);
                    IncrementStage(); // Goto stage 2
                    PlaySound(1);
                }
            }

            if (m_PlayerNear && m_Stage == 2)
            {
                m_Angle = Vector3.Angle(Player.Transform().forward, m_Transform.forward);
                if (m_Angle <= 55f)
                {
                    IncrementStage(); // Goto stage 3
                    SetGravePt3.BeginVoices();
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
                IncrementStage(); // Goto stage 1
            }
        }

        private void StageThree()
        {
            SetGravePt3.BeginVoices();
            IncrementStage(); //Goto stage 4
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

        private void ResizeTrigger(bool orig = false)
        {
            if (!orig)
                m_TriggerCollider.size = new Vector3(0.05f, 0.05f, 0.05f);
            else
                m_TriggerCollider.size = m_OriginalSize;
        }

        public void PlaySound(int index)
        {
            m_AudioSource.clip = m_AudioClips[index];
            m_AudioSource.Play();
        }

        public override void ResetStages()
        {
            base.ResetStages();
            m_Triggered = false;
            m_Animator.SetBool("Triggered", false);
            m_Animator.Update(0f);
            m_Animator.Rebind();
            m_PlayerNear = false;
            m_EndMaterial.SetTexture("_NameNormal", m_Textures[1]);
            ResizeTrigger(true);
        }
    }
}