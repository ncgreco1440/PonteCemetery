using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class OuijiEnd : MonoBehaviour
    {
        [SerializeField]
        private int m_FinalStage = 5;

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.CompareTag("Player") && GameEventRun.Instance.CurrentStage() == m_FinalStage)
            {
                GameEventRun.Instance.IncrementStage();
                Audio.Ambience.StopAmbience();
            }
        }
    }
}