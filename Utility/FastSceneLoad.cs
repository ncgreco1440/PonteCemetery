using Overtop.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PonteCemetery.Utils
{
    public class FastSceneLoad : MonoBehaviour
    {
        AsyncOperation m_CemeteryEntranceAsyncLoader = new AsyncOperation();
        AsyncOperation m_CommonGraveyardAsyncLoader = new AsyncOperation();
        AsyncOperation m_ChurchAsyncLoader = new AsyncOperation();
        AsyncOperation m_RoyalGraveyardAsyncLoader = new AsyncOperation();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            m_CemeteryEntranceAsyncLoader = SceneManager.LoadSceneAsync("Cemetery_Entrance", LoadSceneMode.Additive);
            //m_CommonGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Common_Graveyard", LoadSceneMode.Additive);
            //m_ChurchAsyncLoader = SceneManager.LoadSceneAsync("Church", LoadSceneMode.Additive);
            //m_RoyalGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Royal_Graveyard", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "New Scene")
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("New Scene"));
                SceneManager.UnloadSceneAsync("StartUp");
                //m_CommonGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Common_Graveyard", LoadSceneMode.Additive);
                //m_CommonGraveyardAsyncLoader.allowSceneActivation = false;
                //m_ChurchAsyncLoader = SceneManager.LoadSceneAsync("Church", LoadSceneMode.Additive);
                //m_ChurchAsyncLoader.allowSceneActivation = false;
                //m_RoyalGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Royal_Graveyard", LoadSceneMode.Additive);
                //m_RoyalGraveyardAsyncLoader.allowSceneActivation = false;
            }
        }

        //private void LateUpdate()
        //{
        //    if(GameManager.GameStarted)
        //    {
        //        m_ChurchAsyncLoader.allowSceneActivation = true;
        //        m_CommonGraveyardAsyncLoader.allowSceneActivation = true;
        //        m_RoyalGraveyardAsyncLoader.allowSceneActivation = true;
        //    }
        //}
    }
}