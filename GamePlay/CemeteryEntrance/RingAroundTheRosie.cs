namespace PonteCemetery.GamePlay
{
    public class RingAroundTheRosie : GameEvent
    {
        public static RingAroundTheRosie Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_Stage = 0;
        }
    }
}