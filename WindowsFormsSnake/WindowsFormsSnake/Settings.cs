using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsSnake
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

    class Settings
    {
        public static int TileWidth { get; set; }
        public static int TileHeight { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static int nbTilesX { get; set; }
        public static int nbTilesY { get; set; }
        public static bool GameOver { get; set; }
        public static Direction direction { get; set; }

        public Settings()
        {
            nbTilesX = 10;
            nbTilesY = 10;
            TileWidth = 0;
            TileHeight = 0;
            Speed = 10;
            Score = 0;
            Points = 100;
            GameOver = false;
            direction = Direction.DOWN;
        }
    }
}
