using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class TheWalkTrigger : MonoBehaviour
    {
        public enum TriggerType
        {
            StartWalk,
            EndWalk,
            Entrap,
            Laugh1,
            Laugh2
        }

        public TriggerType m_Type;

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.tag == "Player")
            {
                Trigger();
            }
        }

        private void Trigger()
        {
            if(m_Type == TriggerType.StartWalk)
            {
                TheWalk.Instance().BeginWalk();
            }

            if (m_Type == TriggerType.EndWalk)
            {
                TheWalk.Instance().IncrementStage();
                GameEventRun.Instance.IncrementStage();
            }

            if (m_Type == TriggerType.Entrap)
            {
                TheWalk.Instance().Entrap();
            }

            if(m_Type == TriggerType.Laugh1)
            {
                TheWalk.Instance().Laugh(0);
            }

            if (m_Type == TriggerType.Laugh2)
            {
                TheWalk.Instance().Laugh(1);
            }
        }
    }
}