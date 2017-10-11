using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class RingAroundTheRosieStart : MonoBehaviour
    {
        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                RingAroundTheRosie.Instance.BeginEvent();
            }
        }
    }
}