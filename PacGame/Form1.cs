using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacGame
{
    public partial class Form1 : Form
    {   
        //Listas de los elementos del juego
        private List<PictureBox> walls;
        private List<PictureBox> dots;
        private List<PictureBox> wallhided;
        public List<Ghost> ghosts;

        public PacMan pac;
        public Ghost redGhost = new Ghost(1);
        public Ghost redGhost2 = new Ghost(2);
        public Ghost redGhost3 = new Ghost(3);
        public Ghost redGhost4 = new Ghost(4);      

        //Variable para controlar que pacman mastique
        private bool tick= true;

        public Form1()
        {
            InitializeComponent();
            pac = new PacMan();
            
            this.walls = new List<PictureBox>();
            this.dots = new List<PictureBox>();
            this.wallhided = new List<PictureBox>();
            this.ghosts= new List<Ghost>();

            this.ghosts.Add(redGhost);
            this.ghosts.Add(redGhost2);
            this.ghosts.Add(redGhost3);
            this.ghosts.Add(redGhost4);

            //Se agrega cada elemento a su lista correspondiente
            foreach (PictureBox pic in this.Controls)
            {
                Type t = pic.GetType();
                if (t==typeof(PictureBox))
                {
                    if ((String)pic.Tag == "wall")
                    {
                        this.walls.Add(pic);
                    }
                    else if ((String)pic.Tag == "dot")
                    {
                        this.dots.Add(pic);
                    }                   
                    else if ((String)pic.Tag == "wallhided")
                    {
                        this.wallhided.Add(pic);
                    }                  
                }
            } 
        }

        //Se dibuja a PacMan y a los ghosts
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.SuspendLayout();
            pac.picture.Location = new Point(pac.x, pac.y);
            foreach (Ghost ghost in this.ghosts)
            {
                ghost.picture.Location = new Point(ghost.x, ghost.y);
            }
            this.ResumeLayout();
        }

        //Se mueve Pacman en una direccion especifica
        private void move()
        {
            pac.Move();
        }

        //Se presiona una tecla 
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            pac.IsStopped = false;
            if (e.KeyCode == Keys.Left)
            {
                pac.direction = Direction.Left;
            }
            if (e.KeyCode == Keys.Right )
            {
                pac.direction = Direction.Right;
            }
            if (e.KeyCode == Keys.Up )
            {
                pac.direction = Direction.Up;
            }
            if (e.KeyCode == Keys.Down )
            {
                pac.direction = Direction.Down;
            }
        }
   
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Si un fantasma toca a PacMan el juego se termina
            foreach (Ghost ghost in this.ghosts)
            {
                if (ghost.picture.Bounds.IntersectsWith(pac.picture.Bounds))
                {
                    GameOver();
                }
            }
            
            //Si PacMan se come todos los dots el juego se termina
            if (this.dots.Count == 0)
            {
                GameOver();
            }

            //Si PacMan no esta detenido se mueve
            if (!pac.IsStopped)
            {
              move();
            }
            
            //Recorre todos los muros para determinar si PacMan choca con alguno
             foreach (var pic in this.walls)
             {
                pac.HitWall(pic);
             }
            
                //Si PacMan se come un dot, este se elimina
                foreach (PictureBox pic in this.dots.Reverse<PictureBox>())
                {
                    if (pic.Bounds.IntersectsWith(pac.picture.Bounds))
                    {
                        this.Controls.Remove(pic);
                        this.dots.Remove(pic);
                    }
                }         

            //Aqui PacMan y los fantasmas se transportan por el pasadizo
            foreach (PictureBox pic in this.wallhided.Reverse<PictureBox>())
            {
                if (pic.Bounds.IntersectsWith(pac.picture.Bounds))
                {
                    if (pic.Name == "pictureBox52")
                    {
                        pac.x = this.pictureBox15.Bounds.X  - pac.picture.Width ;
                    }
                    else
                    {
                        pac.x = this.pictureBox52.Bounds.X + 5;
                    }
                }
                foreach (Ghost ghost in this.ghosts)
                {
                    if (pic.Bounds.IntersectsWith(ghost.picture.Bounds))
                    {
                        if (pic.Name == "pictureBox52")
                        {
                            ghost.x = this.pictureBox15.Bounds.X - ghost.picture.Width;
                        }
                        else
                        {
                            ghost.x = this.pictureBox52.Bounds.X + 5;
                        }
                    }
                }
            }
            Invalidate();        
        }

        //Se instancian las imagenes de PacMan y los Ghosts
        private void Form1_Load(object sender, EventArgs e)
        {
            pac.picture = new PictureBox
            {
                Name = "pictureBox001",
                Size = new Size(25, 25),
                Location = new Point(pac.x, pac.y),
                Image = Image.FromFile("Pacman1.png"),              
            };
            
                redGhost.picture = new PictureBox
                {
                    Name = "pictureBox120",
                    Size = new Size(25, 30),
                    Location = new Point(redGhost.x, redGhost.y),
                    Image = Image.FromFile("RedGhost.png"),
                };
                this.Controls.Add(redGhost.picture);

            redGhost2.picture = new PictureBox
            {
                Name = "pictureBox121",
                Size = new Size(25, 30),
                Location = new Point(redGhost2.x, redGhost2.y),
                Image = Image.FromFile("BlueGhost.png"),
            };
            this.Controls.Add(redGhost2.picture);

            redGhost3.picture = new PictureBox
            {
                Name = "pictureBox122",
                Size = new Size(25, 30),
                Location = new Point(redGhost3.x, redGhost3.y),
                Image = Image.FromFile("OrangeGhost.png"),
            };
            this.Controls.Add(redGhost3.picture);

            redGhost4.picture = new PictureBox
            {
                Name = "pictureBox123",
                Size = new Size(25, 30),
                Location = new Point(redGhost4.x, redGhost4.y),
                Image = Image.FromFile("PinkGhost.png"),
            };
            this.Controls.Add(redGhost4.picture);

            this.Controls.Add(pac.picture);
        }

        

        private void timer3_Tick(object sender, EventArgs e)
        {
            //Aqui se desplazan los ghosts por todo el mapa
            foreach (Ghost ghost in this.ghosts)
            {
                ghost.SetDirection(this.pac);
                ghost.Move();
                ghost.HasIntercepted = false;
                foreach (var pic in this.walls)
                {
                    ghost.HitWall(pic);
                }
                if (!ghost.HasIntercepted)
                {
                    ghost.IsStopped = false;
                }
                else { ghost.IsStopped = true; }
            }     
        }

        //Se termina el juego
        private void GameOver()
        {
            this.timer1.Enabled = false;
            this.timer3.Enabled = false;
            this.timer4.Enabled = false;
            MessageBox.Show("GameOver!");
            Application.Restart();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (this.tick)
            {
                //Se cambia la imagen de PacMan dependiendo de su direccion
                switch (pac.direction)
                {
                    case Direction.Left:
                        pac.picture.Image = Image.FromFile("Pacman1Left.png");
                        break;
                    case Direction.Right:
                        pac.picture.Image = Image.FromFile("Pacman1.png");
                        break;
                    case Direction.Up:
                        pac.picture.Image = Image.FromFile("Pacman1Up.png");
                        break;
                    case Direction.Down:
                        pac.picture.Image = Image.FromFile("Pacman1Down.png");
                        break;
                    default:
                        break;
                }
                this.tick = false;
            } else
            {
                //Pacman cierra la boca
                pac.picture.Image = Image.FromFile("MouthClosed.png");
                this.tick = true;
            }
        }   
    }
}
