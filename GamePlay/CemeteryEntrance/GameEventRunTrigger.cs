using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class GameEventRunTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.tag == "Player")
            {
                GameEventRun.Instance.IncrementStage();
            }
        }
    }
}