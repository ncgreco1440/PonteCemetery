using PonteCemetery.GamePlay.Interactables;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class GravekeeperKey : Key
    {
        public override void Interact()
        {
            base.Interact();
            GhostSheet.Stage = 1;
            GhostSheet.PlayCreak();
        }
    }
}