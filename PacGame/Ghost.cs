using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PacGame
{
    public class Ghost
    {
        public Direction direction;
        public int dir;
        public int x;
        public int y;
        public bool HasIntercepted=false;
        public PictureBox picture;
        private Random random;
        
        public bool IsStopped = false;
        

        public Ghost(int number)
        {
            this.dir = number;
            this.x = 225;
            this.y = 124;
            this.random = new Random();
            
        }
     
        public virtual void SetDirection(PacMan pac)
        {
            
                if (this.IsStopped)
                {
                    //Si va en direccion derecha o izquierda, cambia su direccion hacia arriba o abajo
                    if (this.dir == 1 || this.dir == 2)
                    {
                        int num = random.Next(1, 100);
                        if (num <= 50)
                        {
                            this.dir = 3;
                        }
                        else { this.dir = 4; }
                    }   
                    else
                    {
                    //Si va en direccion hacia arriba o abajo, cambia su direccion hacia la derecha o izquierda
                    int num = random.Next(1, 100);
                        if (num <= 50)
                        {
                            this.dir = 2;
                        }
                        else { this.dir = 1; }
                    }
                }

                //Se establece la direccion
                switch (this.dir)
                {
                    case 1:
                        this.direction = Direction.Left;                  
                        break;
                    case 2:
                        this.direction = Direction.Right;
                        break;
                    case 3:
                        this.direction = Direction.Up;
                        break;
                    case 4:
                        this.direction = Direction.Down;
                        break;
                    default:
                        break;
                }           
        }

        public void Move()
        {
            if (!this.IsStopped)
            {
                if (direction == Direction.Right)
                {
                    this.x++;
                }
                else if (direction == Direction.Left)
                {
                    this.x--;
                }
                else if (direction == Direction.Up)
                {
                    this.y--;
                }
                else if (direction == Direction.Down)
                {
                    this.y++;
                }
            }
            
        }

       

        public void HitWall(PictureBox wall)
        {
            //Rebota si choca con un muro
            if (wall.Bounds.IntersectsWith(this.picture.Bounds))
            {
                
                    switch (this.direction)
                    {
                        case Direction.Left:
                            this.x += 2;
                            break;
                        case Direction.Right:
                            this.x -= 2;
                            break;
                        case Direction.Up:
                            this.y += 2;
                            break;
                        case Direction.Down:
                            this.y -= 2;
                            break;
                        default:
                            break;
                    }    
                    this.HasIntercepted = true;
            }
        }

    }
}
