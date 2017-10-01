using System.Collections.Generic;

namespace PonteCemetery
{
    public enum TerrainGround
    {
        None = -1,
        Grass,
        Pavement,
        Dirt,
        Gravel,
        Wood
    }

    /// <summary>
    /// TerrainGrounds will correspond to the textures used to paint the terrain 
    /// within the Unity editor.
    /// </summary>
    public class TerrainGroundModel
    {
        private static List<int> m_GrassIndices = new List<int> { 0, 1, 2, 3, 5, 7, 9, 10 };
        private static List<int> m_PavementIndices = new List<int> { 6 };
        private static List<int> m_GravelIndices = new List<int> { 4, 8 };
        private static List<int> m_WoodIndices = new List<int> { 11 };

        public static List<int> Grasses
        {
            get
            {
                return m_GrassIndices;
            }
        }

        public static bool isGrass(int value)
        {
            return m_GrassIndices.Contains(value);
        }

        public static bool isPavement(int value)
        {
            return m_PavementIndices.Contains(value);
        }

        public static bool isWood(int value)
        {
            return m_WoodIndices.Contains(value);
        }

        public static TerrainGround FindTextureType(int index)
        {
            if (isGrass(index))
                return TerrainGround.Grass;
            else if (isPavement(index))
                return TerrainGround.Pavement;
            else if (isWood(index))
                return TerrainGround.Wood;
            else
                return TerrainGround.Dirt;
        }
    }
}