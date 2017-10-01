using System.Text;
using UnityEngine;

namespace PonteCemetery
{
    [System.Serializable]
    public class GameEvent : MonoBehaviour
    {
        public string m_EventName = "GameEvent";
        [SerializeField]
        protected bool m_Completed = false;
        protected int m_Stage = 0;
        public int m_NumStages = 1;

        public GameEvent() { }

        /// <summary>
        /// Returns the Stages associated with this GameEvent
        /// </summary>
        public virtual void Stages() { }

        /// <summary>
        /// Returns true if the GameEvent is completed
        /// </summary>
        /// <returns></returns>
        public virtual bool Completed()
        {
            return m_Completed;
        }

        /// <summary>
        /// Returns the current stage of the GameEvent
        /// </summary>
        public virtual int CurrentStage()
        {
            return m_Stage;
        }

        /// <summary>
        /// Increments the stage of the GameEvent if it is not yet completed. A GameEvent's stage will never 
        /// be greater than or equal to the number of stages.
        /// </summary>
        public virtual void IncrementStage()
        {
            if(!m_Completed)
            {
                m_Stage++;
                m_Completed = m_Stage == m_NumStages - 1;
            }
        }

        /// <summary>
        /// Decrements the stage of the GameEvent if it is not yet completed. A GameEvent's stage can never be less than 0
        /// </summary>
        public virtual void DecrementStage()
        {
            if(!m_Completed && m_Stage - 1 > -1)
                m_Stage--;
        }

        /// <summary>
        /// Resets the stages and completed status of the GameEvent
        /// </summary>
        public virtual void ResetStages()
        {
            m_Completed = false;
            m_Stage = 0;
        }

        /// <summary>
        /// Load in stages from a previous play session
        /// </summary>
        public virtual void LoadEventProgress(int stage = 0, bool completed = false)
        {
            m_Stage = stage;
            m_Completed = completed;
        }

        /// <summary>
        /// Saves stages from the current play session to a passed in StringBuilder
        /// </summary>
        public virtual StringBuilder SaveEventProgress(ref StringBuilder strBuilder)
        {
            return strBuilder.AppendFormat("{0}: { stage: {1}, completed: {2} }", m_EventName, m_Stage, m_Completed);
        }
    }
}