using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsSnake
{
    public partial class Form1 : Form
    {
        List<Circle> Snake = new List<Circle>();
        List<Circle> Walls = new List<Circle>();
        Circle food;

        public Form1()
        {
            InitializeComponent();

            new Settings();
            LoadLevel();
            Console.WriteLine(Settings.nbTilesX + " " + Settings.nbTilesY);

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }

        void LoadLevel()
        {
            using (TextReader reader = File.OpenText("../../Levels/Map.map"))
            {
                string text = reader.ReadLine();
                string[] dimensionsStrings = text.Split(' ');

                Settings.nbTilesX = int.Parse(dimensionsStrings[0]);
                Settings.nbTilesY = int.Parse(dimensionsStrings[1]);

                for(int i = 0; i < Settings.nbTilesY; i++)
                {
                    string line = reader.ReadLine();
                    for(int j = 0; j < line.Length; j++)
                    {
                        if(line[j] == 'W')
                        {
                            Circle wall = new Circle();
                            wall.X = j;
                            wall.Y = i;
                            Walls.Add(wall);
                        }
                    }
                }
            }
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Settings();
            LoadLevel();

            Settings.TileWidth = Canvas.Width / Settings.nbTilesX;
            Settings.TileHeight = Canvas.Height / Settings.nbTilesY;

            Snake.Clear();
            Circle head = new Circle();
            head.X = 2;
            head.Y = 2;
            Snake.Add(head);

            txtScoreValue.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            Random random = new Random();

            food = new Circle();
            food.X = random.Next(1, Settings.nbTilesX - 1);
            food.Y = random.Next(1, Settings.nbTilesY - 1);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter)) StartGame();
            }

            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.LEFT) Settings.direction = Direction.RIGHT;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.RIGHT) Settings.direction = Direction.LEFT;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.DOWN) Settings.direction = Direction.UP;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.UP) Settings.direction = Direction.DOWN;

                MovePlayer();
            }

            Canvas.Invalidate();
        }

        private void MovePlayer()
        {
            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                if(i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.RIGHT:
                            Snake[i].X++;
                            break;
                        case Direction.LEFT:
                            Snake[i].X--;
                            break;
                        case Direction.UP:
                            Snake[i].Y--;
                            break;
                        case Direction.DOWN:
                            Snake[i].Y++;
                            break;
                    }

                    //Borders collision
                    if (Snake[0].X < 0 || Snake[0].Y < 0 || Snake[0].X >= Settings.nbTilesX || Snake[0].Y >= Settings.nbTilesY) Die();

                    //Body collision
                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[0].X == Snake[j].X && Snake[0].Y == Snake[j].Y) Die();
                    }

                    //Food collision
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y) Eat();

                    //Obstacles collision
                    for(int k = 0; k < Walls.Count; k++)
                    {
                        if (Snake[0].X == Walls[k].X && Snake[0].Y == Walls[k].Y) Die();
                    }
                        
                }

                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Eat()
        {
            Circle newBody = new Circle();
            newBody.X = Snake[Snake.Count - 1].X;
            newBody.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(newBody);

            Settings.Score += Settings.Points;
            txtScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void txtScore_Click(object sender, EventArgs e)
        {

        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                Brush snakeColour;

                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0) snakeColour = Brushes.Black;
                    else snakeColour = Brushes.Green;

                    canvas.FillEllipse(snakeColour, new Rectangle(Snake[i].X * Settings.TileWidth, Snake[i].Y * Settings.TileHeight, Settings.TileWidth, Settings.TileHeight));

                }

                canvas.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.TileWidth, food.Y * Settings.TileHeight, Settings.TileWidth, Settings.TileHeight));

                for(int j = 0; j < Walls.Count; j++)
                {
                    canvas.FillEllipse(Brushes.Blue, new Rectangle(Walls[j].X * Settings.TileWidth, Walls[j].Y * Settings.TileHeight, Settings.TileWidth, Settings.TileHeight));
                }
            }

            else
            {
                string gameOverMessage = "Game Over \n Your final score is : " + Settings.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOverMessage;
                lblGameOver.Visible = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
