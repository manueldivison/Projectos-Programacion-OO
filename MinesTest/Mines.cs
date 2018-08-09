using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesTest
{
     partial class Mines : Form
    {

        MainMenu menu;
        MenuItem filemenu;
        Position[,] matrix;
        Random random = new Random();
        private int ButtonsEnabled = 0;
        private bool GameFinished = false;
        private Level level;
        private int Rows;
        private int Cols;
        private int Bombs;

        public Mines(Level Difficulty)
        {
            InitializeComponent();
            menu = new MainMenu();
            filemenu = new MenuItem();
            filemenu.Text = "&Options";
            menu.MenuItems.Add(filemenu);
            MenuItem print = new MenuItem();
            print.Text = "Level";
            filemenu.MenuItems.Add(print);

            MenuItem temp = new MenuItem();
            temp.Text = "Easy";
            print.MenuItems.Add(temp);

            MenuItem temp2 = new MenuItem();
            temp2.Text = "Medium";
            print.MenuItems.Add(temp2);

            MenuItem temp3 = new MenuItem();
            temp3.Text = "Hard";
            print.MenuItems.Add(temp3);

            this.Menu = menu;

            temp.Click += Temp_Click;
            temp2.Click += Temp2_Click;
            temp3.Click += Temp3_Click;

            switch (Difficulty)
            {
                case Level.easy:
                    this.Rows = 10;
                    this.Cols = 10;                  
                    break;
                case Level.medium:
                    this.Rows = 15;
                    this.Cols = 15;
                    break;
                case Level.hard:
                    this.Rows = 20;
                    this.Cols = 20;
                    break;
            }
            matrix = new Position[this.Rows, this.Cols];
            this.Bombs = (int)(matrix.Length / 6.4);

            int startTop = 28;
            int startLeft = 0;
            int count = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                
                for (int j = 0; j < this.Cols; j++)
                {
                    MineButton but = CreateButton(startTop, startLeft, i, j, false);
                    startLeft += 28;          
                    this.Controls.Add(but);
                    count++; 
                }
                startTop += 28;
                startLeft = 0;
            }
            this.pictureBox2.MouseClick += pictureBox2_MouseClick;
            PutBombs(this.Bombs);
            PutNumbers();

        }

        private void Temp_Click(object sender, EventArgs e)
        {
            this.level = Level.easy;
        }
        private void Temp2_Click(object sender, EventArgs e)
        {
            this.level = Level.medium;
        }
        private void Temp3_Click(object sender, EventArgs e)
        {
            this.level = Level.hard;
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs me)
        {
            Mines NewForm = new Mines(this.level);
            NewForm.Show();
            this.Dispose(false);
        }

        private void Btn1_Click(object sender, MouseEventArgs e)
       {

            MineButton btn = (MineButton)sender;
            Position pos = matrix[btn.X, btn.Y];
            
            if (e.Button == MouseButtons.Left && !matrix[pos.X, pos.Y].IsBomb && !GameFinished && !matrix[pos.X, pos.Y].IsFlagged)
            {
              
               EnableButton(matrix[pos.X,pos.Y]);
                
            } else if (e.Button == MouseButtons.Left && !pos.IsFlagged && !GameFinished)
            {
                this.pictureBox2.Image = global::MinesTest.Properties.Resources.sad;
                GameFinished = true;

                for (int i = 0; i < this.Rows; i++)
                {
                    for (int j = 0; j < this.Cols; j++)
                    {
                        if (matrix[i, j].IsEnabled && matrix[i,j].IsBomb)
                        {
                            matrix[i, j].Boton.Image = global::MinesTest.Properties.Resources.bomb;
                            matrix[i, j].IsEnabled = false;
                        }

                    }
                }
            }

             if (e.Button == MouseButtons.Right && matrix[btn.X, btn.Y].IsFlagged && !GameFinished)
            {
                btn.Text = "";
                btn.Image = null;
                matrix[btn.X, btn.Y].IsFlagged = false;
            }
             else if (e.Button == MouseButtons.Right && !GameFinished)
            {
                matrix[btn.X, btn.Y].IsFlagged = true;
               
                btn.Image = global::MinesTest.Properties.Resources.banderita;       
            }
        } 

        private MineButton CreateButton(int top, int left, int x, int y, bool isBomb)
        {
            MineButton bt;
            bt = new MineButton
            {
                Top = top,
                Left = left,
                Text = "",
                Width = 30,
                Height = 30,
                X = x,
                Y = y
            };
            matrix[x, y] = new Position() { X = x, Y = y, IsBomb = false, Boton = bt, IsEnabled = true };
          
            bt.MouseUp += Btn1_Click;
           
            return bt;
        }

        void PutBombs(int bombs)
        {
            int i = 0;
            while (i != bombs)
            {
                int x = random.Next(0, this.Rows-1);
                int y = random.Next(0, this.Cols-1);
                if (matrix[x,y].IsBomb != true)
                {                   
                    this.matrix[x, y].IsBomb = true;
                    i++;
                }
            }
        }

        void PutNumbers()
        {
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    if (matrix[i,j].IsBomb != true)
                    {
                        int count = 0;
                        for (int w = -1; w < 2; w++)
                        {
                            for (int k = -1; k < 2; k++)
                            {
                                if (k == 0 && w == 0) { continue; }
                                int row = i + k;
                                int col = j + w;
                                if ((row >= 0 && row < this.Rows) && (col >= 0 && col < this.Cols))
                                {
                                    if (matrix[row,col].IsBomb)
                                    {
                                        count++;
                                    }
                                }

                            }
                        }
                        matrix[i, j].BombsAround = count;
                    }
                }
            }
        }

        private void Mines_Load(object sender, EventArgs e)
        {
          this.AutoSize = true;
           this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
       this.pictureBox2.Location = new System.Drawing.Point((this.Width / 2)-30, 1);
        }
        private void EnableButton(Position pos)
        {
            if (pos.BombsAround == 0) { pos.Boton.Enabled = false; }
            if (pos.BombsAround > 0)
            {

                ShowNumbers(pos);
            }
            else 
            {
                pos.IsEnabled= false;
                ButtonsEnabled++;
                CheckIfPlayerWon();
                for (int w = -1; w < 2; w++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        if (k == 0 && w == 0) { continue; }
                        int row = pos.X + k;
                        int col = pos.Y + w;
                        if ((row >= 0 && row < this.Rows) && (col >= 0 && col < this.Cols) && matrix[row, col].IsEnabled)
                        {
                            EnableButton(matrix[row, col]);
                        }

                    }
                }


            }
        }

        private void CheckIfPlayerWon()
        {
            if (ButtonsEnabled == matrix.Length-this.Bombs)
            {
                GameFinished = true;
                MessageBox.Show("Congratulations!");
            }
        }

        private void ShowNumbers(Position pos )
        {
            switch (pos.BombsAround)
            {
                case 1:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.uno1;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;
                case 2:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.dos;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;
                case 3:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.tres;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;
                case 4:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.cuatro;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;
                case 5:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.cinco;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;
                case 6:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.seis;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;
                case 7:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.siete;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;
                case 8:
                    pos.Boton.Image = global::MinesTest.Properties.Resources.ocho;
                    pos.IsEnabled = false;
                    ButtonsEnabled++;
                    CheckIfPlayerWon();
                    break;

            }
        }
    }
}