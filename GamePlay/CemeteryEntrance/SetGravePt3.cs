using Overtop.Scripts.Interactables;
using PonteCemetery.GamePlay.Interactables;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class SetGravePt3 : IInteractable
    {
        public static SetGravePt3 Instance;
        public AudioSource m_AudioSource;
        public WoodenDoor m_GravekeeperCabinDoor;

        private void Awake()
        {
            Instance = this;
            m_Collider = GetComponent<BoxCollider>();
        }

        public override void Interact()
        {
            if(SetGrave.Instance.CurrentStage() == 3)
            {
                m_GravekeeperCabinDoor.ForceOpen();
                Disable();
                EndVoices();
                InvokeInteractiveFeedbackEvent(m_ReasonForSuccessInteraction);
            }
            else
            {
                InvokeInteractiveFeedbackEvent(m_ReasonForFailedInteraction);
            }
        }

        public static void BeginVoices()
        {
            Instance.m_AudioSource.Play();
        }

        public static void EndVoices()
        {
            Instance.m_AudioSource.loop = false;
        }
    }
}