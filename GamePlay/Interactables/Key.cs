using System;
using Overtop.Scripts.Interactables;
using UnityEngine;

namespace PonteCemetery.GamePlay.Interactables
{
    public class Key : IInteractable
    {
        [SerializeField]
        private long m_Code = 0L;

        /// <summary>
        /// Adds key to player's inventory, hides the key in the scene
        /// </summary>
        public override void Interact()
        {
            Player.AddToKeyChain(m_Code);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            //Destroy(gameObject);
            InvokeInteractiveFeedbackEvent(m_ReasonForSuccessInteraction);
        }

        public override string InteractableName()
        {
            return m_InteractName;
        }

        public override bool TryAction()
        {
            return true;
        }

        public override string WhyInteractFailed()
        {
            return m_ReasonForFailedInteraction;
        }
    }
}