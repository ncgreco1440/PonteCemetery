using UnityEngine;
using Newtonsoft.Json;
using System;
using Overtop.Managers;
using System.Text;
using System.Collections.Generic;

namespace PonteCemetery
{
    [System.Serializable]
    public class GameState : ISerializationCallbackReceiver
    {
        [SerializeField]
        private string m_Filepath;
        [SerializeField]
        private string m_Filename;
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float m_PercentageComplete;
        [SerializeField]
        private Vector3 m_PlayerPosition;
        [SerializeField]
        private Quaternion m_PlayerRotation;
        [SerializeField]
        private Vector3 m_PlayerRotationVec3;
        [SerializeField]
        private Quaternion m_CameraRotation;
        [SerializeField]
        private Vector3 m_CameraRotationVec3;
        [SerializeField]
        private DateTime m_DateTimeStamp;
        [SerializeField]
        private string m_TimeStamp;
        [SerializeField]
        private List<GameEvent> m_GameEvents;
        
        private static GameState m_LastGameState;

        public GameState()
        {
            m_PercentageComplete = 0.0f;
            m_PlayerPosition = new Vector3(13.0f, 1.0f, 2.5f);
            m_PlayerRotation = new Quaternion(0, 0, 0, 1);
            m_CameraRotation = new Quaternion(0, 0, 0, 1);
            m_DateTimeStamp = DateTime.MinValue;
            m_Filepath = "";
            m_Filename = "";
        }

        public static GameState LoadGameState(string filename)
        {
            try
            {
                GameState gameState = new GameState();
                string saveData = System.IO.File.ReadAllText(filename);
                gameState = JsonConvert.DeserializeObject<GameState>(saveData);
                gameState.Filename = filename;
                return gameState;
            }
            catch(System.Exception e)
            {
                throw new System.Exception("Invalid file");
            }
        }

        /// <summary>
        /// Tracks the player, must be called to properly save player's current position and look rotation
        /// </summary>
        /// <param name="player"></param>
        public void Track()
        {
            Player.GetPlayerPositionAndLook(ref m_PlayerPosition, ref m_PlayerRotation, ref m_CameraRotation);
        }

        public string Filename
        {
            get { return m_Filename; }
            set
            {
                m_Filepath = value;
                List<string> strList = new List<string>(value.Split('/'));
                strList.Reverse();
                m_Filename = strList[0];
            }
        }

        public string FilenameNoExt
        {
            get { return m_Filename.Substring(0, m_Filename.IndexOf('.')); }
        }

        public int Sequence
        {
            get { return int.Parse(m_Filename.Split('_')[1]); }
        }

        public float Percentage
        {
            get { return m_PercentageComplete; }
            set { m_PercentageComplete = value; }
        }

        public string Progress
        {
            get { return m_PercentageComplete.ToString()+"%"; }
        } 

        public Vector3 PlayerPosition
        {
            get { return m_PlayerPosition; }
            set { m_PlayerPosition = value; }
        }

        public Quaternion PlayerRotation
        {
            get { return m_PlayerRotation; }
            set { m_PlayerRotation = value; m_PlayerRotationVec3 = value.eulerAngles; }
        }

        public Quaternion CameraRotation
        {
            get { return m_CameraRotation; }
            set { m_CameraRotation = value; m_CameraRotationVec3 = value.eulerAngles; }
        }

        public string DateTimeStamp
        {
            get { return m_DateTimeStamp.ToString(); }
            set
            {
                m_DateTimeStamp = DateTime.Parse(value);
                m_TimeStamp = m_DateTimeStamp.ToString();
            }
        }

        public DateTime Date
        {
            get { return m_DateTimeStamp; }
        }

        public void Save()
        {
            StringBuilder saveData = new StringBuilder();
            BeginSaveData(ref saveData);
            SaveDateTimeStamp(ref saveData);
            SavePlayerData(ref saveData);
            //TODO Save Player's in game event data...
            EndSaveData(ref saveData);

            if(Filename == "")
                Filename = GameStateListings.GenerateGameStateFileName();
            System.IO.File.WriteAllText(m_Filepath, saveData.ToString());
            GameStateListings.UpdateGameStateListings(this);
        }

        private void BeginSaveData(ref StringBuilder strBuilder)
        {
            strBuilder.Append("{");
        }

        private void EndSaveData(ref StringBuilder strBuilder)
        {
            strBuilder.Append("}");
        }

        private void SaveDateTimeStamp(ref StringBuilder strBuilder)
        {
            m_DateTimeStamp = DateTime.Now;
            strBuilder.Append("DateTimeStamp: \"" + m_DateTimeStamp.ToString() + "\",");
        }

        private void SavePlayerData(ref StringBuilder strBuilder)
        {
            Track();
            strBuilder.Append(
                    "PlayerPosition:{" +
                        "x: " + m_PlayerPosition.x + "," +
                        "y: " + m_PlayerPosition.y + "," +
                        "z: " + m_PlayerPosition.z + "}," +
                    "PlayerRotation: { " +
                        "x: " + 0 + "," +
                        "y: " + m_PlayerRotation.y + "," +
                        "z: " + 0 + "," +
                        "w: " + m_PlayerRotation.w + "}," +
                    "CameraRotation: { " +
                        "x: " + m_CameraRotation.x + "," +
                        "y: " + 0 + "," +
                        "z: " + 0 + "," +
                        "w: " + m_CameraRotation.w + "}");
        }
        
        private void SaveGameEventData(ref StringBuilder strBuilder)
        {
            strBuilder.Append(",GameEvents:{");
            for(int i = 0; i < m_GameEvents.Count; i++)
            {
                m_GameEvents[i].SaveEventProgress(ref strBuilder);
                if(i < m_GameEvents.Count - 1)
                    strBuilder.Append(",");
            }
            strBuilder.Append("}");
        }

        public void OnBeforeSerialize()
        {
            if (m_DateTimeStamp != null)
                m_TimeStamp = m_DateTimeStamp.ToString();
            else
                m_TimeStamp = "";
        }

        public void OnAfterDeserialize() {}

        public static bool operator ==(GameState lhs, GameState rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            else
                return false;
        }

        public static bool operator !=(GameState lhs, GameState rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            else
                return false;
        }
    }
}