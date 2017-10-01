using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System;
using Overtop.Managers;

namespace PonteCemetery
{
    public static class GameStateListings
    {
        /// <summary>
        /// @"Assets/Resources/Saves/GameStateSaves.json"
        /// </summary>
        private static string GAME_STATE_LOADING_ERROR = "GameState could not be read because the files are either corrupted or do not exist.";
        private static string GAME_STATE_LISTINGS_ERROR = "GameStateListing.json could not be read because the file is either corrupted or doesn\'t exist.";
        private static List<GameState> m_GameStateDB;
        private static List<string> m_GameStateListings;

        public static string ApplicationGameSavesPath
        {
            get { return GameManager.ApplicationDataPath + "/Saves/"; }
        }

        public static GameState GetGameState(int index)
        {
            return m_GameStateDB[index];
        }

        /// <summary>
        /// Attempts to load in the GameStateSaves.json file and populate an internal 
        /// List of strings containing all GameState file locations.
        /// </summary>
        public static void LookUpGameStateListings()
        {
            m_GameStateListings = new List<string>();
            m_GameStateDB = new List<GameState>();
            try
            {
                //Read in all files from the Data directory
                m_GameStateListings = new List<string>(System.IO.Directory.GetFiles(ApplicationGameSavesPath));
                //Remove all Unity generated meta files and the README.txt
                m_GameStateListings.RemoveAll((str) => { return str.Contains("meta") || str.Contains("README"); });
                m_GameStateListings.ForEach((str) => {
                    try {
                        m_GameStateDB.Add(GameState.LoadGameState(str));
                    } catch(System.Exception e) {
                        Debug.Log(e);
                        //Do not add invalid save files...
                    }
                });
            }
            catch (System.Exception e)
            {
                if (e.GetType() == typeof(System.IO.DirectoryNotFoundException))
                    System.IO.Directory.CreateDirectory(ApplicationGameSavesPath);
                CleanUp();
            }
        }

        /// <summary>
        /// Returns a list of strings indicating the filenames of all GameState listings the player has.
        /// </summary>
        /// <returns></returns>
        public static List<GameState> GetAllGameStates()
        {
            return m_GameStateDB;
        }

        public static string GetFirstGameState()
        {
            try
            {
                return m_GameStateListings[0];
            }catch(System.Exception e)
            {
                return "";
            }
        }

        public static GameState GetLastSavedGameState()
        {
            GameState lastGameState = new GameState();
            m_GameStateDB.ForEach((gameState) => {
                if (lastGameState.Date.CompareTo(gameState.Date) < 0)
                    lastGameState = gameState;
            });
            return lastGameState;
        }

        public static int NumGameSaves
        {
            get { return m_GameStateDB.Count; }
        }

        /// <summary>
        /// Writes all known GameStateListings to the hard disk in GameStateSaves.json
        /// </summary>
        public static void SaveGameStateListings()
        {
            if(m_GameStateListings.Count > 0)
            {
                try
                {
                    System.IO.File.WriteAllText(ApplicationGameSavesPath, JsonConvert.SerializeObject(m_GameStateListings));
                }
                catch(System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        /// <summary>
        /// Updates the GameState Database with a new entry if the argument does not already exist.
        /// </summary>
        /// <param name="gameState"></param>
        public static void UpdateGameStateListings(GameState gameState)
        {
            if (!m_GameStateDB.Contains(gameState))
            {
                m_GameStateDB.Add(gameState);
            }   
        }

        /// <summary>
        /// Adds a new GameState to be saved in GameStateListing.json
        /// </summary>
        /// <param name="filename"></param>
        public static void CreateNewGameStateListing(string filename)
        {
            m_GameStateListings.Add(filename);
        }

        /// <summary>
        /// Removes a GameState and will not be saved in GameStateListing.json
        /// </summary>
        /// <param name="filename"></param>
        public static void RemoveGameStateListing(string filename)
        {
            m_GameStateListings.Remove(filename);
            System.IO.File.Delete(filename);
        }

        /// <summary>
        /// Generates the next sequential filename to be saved.
        /// Follows the format GameState_X_.json where X follows
        /// a sequential integer pattern starting at 1.
        /// </summary>
        /// <returns></returns>
        public static string GenerateGameStateFileName()
        {
            StringBuilder generatedFile = new StringBuilder();
            generatedFile.Append("Save_");
            generatedFile.Append(CalculateNextSequentialGameSaveIndex());
            generatedFile.Append("_.json");

            return ApplicationGameSavesPath + generatedFile.ToString();
        }

        /// <summary>
        /// Returns a list of all filenames assigned to the GameState saves without the directory location.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetGameStateFileNames()
        {
            List<string> filenames = new List<string>();
            m_GameStateDB.ForEach((gameState) => {
                filenames.Add(gameState.Filename);
            });
            return filenames;
        }

        private static int CalculateNextSequentialGameSaveIndex()
        {
            int largest = 1;
            m_GameStateDB.ForEach((gameState) => {
                if (largest < gameState.Sequence)
                    largest = gameState.Sequence;
            });
            return ++largest;
        }

        /// <summary>
        /// Attempts to clean up the GameStateSaves.json file by deleting it because as at 
        /// this point it has either been corrupted or tampered with. 
        /// </summary>
        private static void CleanUp()
        {
            try
            {
                System.IO.File.Delete(ApplicationGameSavesPath);
            }
            catch (System.Exception ex) { }
        }
    }
}