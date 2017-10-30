using Overtop.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace PonteCemetery.Utils
{
    public class FastSceneLoad : MonoBehaviour
    {
        public static FastSceneLoad Instance = null;
        public Canvas m_LoadingScreen;
        public Camera m_LoadingCam;
        [SerializeField]
        private string m_CurrentScene = "";
        [SerializeField]
        private string m_SceneToLoad = "";
        [SerializeField]
        private bool m_BaseSceneLoaded = false;

        AsyncOperation m_CemeteryEntranceAsyncLoader = new AsyncOperation();
        AsyncOperation m_CommonGraveyardAsyncLoader = new AsyncOperation();
        AsyncOperation m_ChurchAsyncLoader = new AsyncOperation();
        AsyncOperation m_RoyalGraveyardAsyncLoader = new AsyncOperation();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            //m_CemeteryEntranceAsyncLoader = SceneManager.LoadSceneAsync("Cemetery_Entrance", LoadSceneMode.Additive);
            //m_CommonGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Common_Graveyard", LoadSceneMode.Additive);
            //m_ChurchAsyncLoader = SceneManager.LoadSceneAsync("Church", LoadSceneMode.Additive);
            //m_ChurchAsyncLoader.allowSceneActivation = false;
            //m_RoyalGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Royal_Graveyard", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Base_Scene")
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Base_Scene"));
                m_BaseSceneLoaded = true;
                EndLoad();
                //m_CommonGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Common_Graveyard", LoadSceneMode.Additive);
                //m_CommonGraveyardAsyncLoader.allowSceneActivation = false;
                //m_ChurchAsyncLoader.allowSceneActivation = true;
                //m_ChurchAsyncLoader.allowSceneActivation = false;
                //m_RoyalGraveyardAsyncLoader = SceneManager.LoadSceneAsync("Royal_Graveyard", LoadSceneMode.Additive);
                //m_RoyalGraveyardAsyncLoader.allowSceneActivation = false;
            }

            if(scene.name == "Cemetery_Entrance")
            {
                SceneManager.UnloadSceneAsync("Title");
                GameManager.SetCurrentGameState(new GameState());
                GameManager.GameStarted = true;
                m_CurrentScene = "Cemetery_Entrance";
                EndLoad();
            }

            if(scene.name == "Title")
            {
                if(!InputManager.ControllerIsConnected)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }

                m_CurrentScene = "Title";
                GameManager.PlayerEyesOpen();
                if (m_BaseSceneLoaded)
                    EndLoad();
            }
        }

        public void SceneUnloaded(Scene scene)
        {
            if (scene.name == "Cemetery_Entrance" && m_SceneToLoad == "Cemetery_Entrance")
            {
                SceneManager.LoadSceneAsync("Cemetery_Entrance", LoadSceneMode.Additive);
            }
            else
            {
                if(!GameManager.GameStarted)
                    SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
            }
        }

        public void BeginLoad()
        {
            GameManager.DestroyPlayer();
            m_LoadingCam.gameObject.SetActive(true);
            m_LoadingScreen.gameObject.SetActive(true);
            m_SceneToLoad = "Cemetery_Entrance";
            SceneManager.LoadSceneAsync("Cemetery_Entrance", LoadSceneMode.Additive);
        }

        public void EndLoad()
        {
            m_LoadingCam.gameObject.SetActive(false);
            m_LoadingScreen.gameObject.SetActive(false);
        }

        public static void GoToScene(string scene)
        {
            Instance.m_SceneToLoad = scene;
            Instance.m_LoadingCam.gameObject.SetActive(true);
            Instance.m_LoadingScreen.gameObject.SetActive(true);
            SceneManager.UnloadSceneAsync("Cemetery_Entrance");            
        }

        public static string CurrentScene()
        {
            return Instance.m_CurrentScene;
        }
    }
}