using PonteCemetery.GamePlay.Interactables;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class SetGravePt3 : MonoBehaviour
    {
        public static SetGravePt3 Instance;
        public AudioSource m_AudioSource;
        public WoodenDoor m_GravekeeperCabinDoor;

        private void Awake()
        {
            Instance = this;
        }

        public void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.tag == "Player")
            {
                if (SetGrave.Instance.CurrentStage() == 2)
                {
                    SetGrave.Instance.PlaySound(3);
                    SetGrave.Instance.IncrementStage();
                    //EndVoices();
                }

                if (SetGrave.Instance.CurrentStage() == 5)
                {
                    EndVoices();
                    SetGrave.Instance.IncrementStage();
                    m_GravekeeperCabinDoor.ForceOpen();
                }
            }
        }

        public static void BeginVoices()
        {
            Instance.m_AudioSource.Play();
        }

        public static void EndVoices()
        {
            Instance.m_AudioSource.loop = false;
            //Instance.m_AudioSource.Stop();
        }
    }
}