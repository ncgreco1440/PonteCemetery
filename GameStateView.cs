using Overtop.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PonteCemetery
{
    public class GameStateView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Text m_Filename;
        [SerializeField]
        private Text m_Timestamp;
        [SerializeField]
        private Text m_Progress;
        [SerializeField]
        private GameState m_GameState;
        
        public void Set(GameState gameState)
        {
            if(gameState != null)
            {
                m_Filename.text = gameState.FilenameNoExt.Replace("_", " ");
                m_Timestamp.text = gameState.DateTimeStamp;
                m_Progress.text = gameState.Progress;
                m_GameState = gameState;
            }
            else
            {
                DefaultView();
            }
        }

        private void DefaultView()
        {
            m_Filename.text = "Empty";
            m_Timestamp.text = "1/1/2017 12:00:00 AM";
            m_Progress.text = "0%";
            m_GameState = null;
        }

        private void LoadGameState()
        {
            GameManager.SetCurrentGameState(m_GameState);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_GameState != null)
                LoadGameState();
        }
    }
}