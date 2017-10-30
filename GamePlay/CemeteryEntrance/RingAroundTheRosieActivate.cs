using Overtop.Scripts.Interactables;
using PonteCemetery.GamePlay;

public class RingAroundTheRosieActivate : IInteractable
{ 
    public override void Interact()
    {
        gameObject.SetActive(false);
        if(!RingAroundTheRosie.Instance.Active)
            RingAroundTheRosie.Instance.Active = true;
    }
}