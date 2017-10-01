using Overtop.Scripts.Interactables;
using UnityEngine;

namespace PonteCemetery.GamePlay.Interactables
{
    public class Key : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private long m_Code = 0L;

        /// <summary>
        /// Adds key to player's inventory, remove key from the scene
        /// </summary>
        public virtual void Interact()
        {
            Player.AddToKeyChain(m_Code);
            Destroy(gameObject);
        }

        public virtual bool TryAction()
        {
            return true;
        }
    }
}