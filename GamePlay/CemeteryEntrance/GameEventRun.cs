using Overtop.Managers;
using PonteCemetery.GamePlay.Interactables;
using System;
using System.Collections;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class GameEventRun : GameEvent
    {
        private static GameEventRun m_Instance = null;
        public Transform m_AudioSourceTransform;
        public AudioSource m_AudioSource;

        [SerializeField]
        private WoodenDoor m_Door;

        [SerializeField]
        private FlickeringLights m_FlickeringLights;

        [Tooltip("Positions that the audio will move to")]
        public Transform[] m_Positions;

        [SerializeField]
        private AudioClip[] m_Sounds;

        [SerializeField]
        private GraveTextAssembly m_OuijiSequence;

        private void Awake()
        {
            m_Instance = this;
        }

        public override void IncrementStage()
        {
            base.IncrementStage();
            switch(CurrentStage())
            {
                case 4:
                {
                    m_OuijiSequence.BeginTerror();
                    StartCoroutine(TransitionToTerror());
                    break;
                }
                case 5:
                {
                    m_FlickeringLights.BeginFlickering(m_Sounds[1]);
                    m_OuijiSequence.Terrorize();
                    Audio.Ambience.Instance.StartCoroutine(Audio.Ambience.BeginAmbience(5, 0.25f));
                    break;
                }
                case 6:
                {
                    m_Door.SlamShut();
                    m_AudioSource.clip = m_Sounds[4];
                    m_AudioSource.Play();
                    StartCoroutine(WaitForABit(GameManager.TriggerFin, 0.5f));
                    break;
                }
            }
        }

        private IEnumerator WaitForABit(Action action = null, float time = 2.5f)
        {
            yield return new WaitForSeconds(time);
            if (action != null)
                action();
        }

        private IEnumerator TransitionToTerror()
        {
            yield return StartCoroutine(WaitForABit(() => { m_FlickeringLights.TurnOffAllLights(m_Sounds[0]); }, 5.5f));
            StartCoroutine(WaitForABit(IncrementStage, 3.0f));
        }

        /// <summary>
        /// Singleton instance of the GameEvent Run
        /// </summary>
        public static GameEventRun Instance
        {
            get
            {
                return m_Instance;
            }
        }
    }
}