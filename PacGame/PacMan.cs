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
    public  class PacMan
    {
        //Direccion
        public int x;
        public int y;
       
        public bool IsStopped = false;
        public enum Position
        {
            Left, Right, Up, Down
        }
        public Direction direction;
        public PictureBox picture;

        public PacMan()
        {
            direction = Direction.Left;
            this.x = 353;
            this.y = 152;
        }

        //Se mueve dependiendo la direccion
        public void Move()
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
        
        //Si PacMan choca con un muro, rebota y se detiene
        public void HitWall(PictureBox wall)
        {
            if (wall.Bounds.IntersectsWith(this.picture.Bounds))
            {
                switch (this.direction)
                {
                    case Direction.Left:
                        this.x += 1;
                        break;
                    case Direction.Right:
                        this.x -= 1;
                        break;
                    case Direction.Up:
                        this.y += 1;
                        break;
                    case Direction.Down:
                        this.y -= 1;
                        break;
                    default:
                        break;
                }
                IsStopped = true;           
            }
        }
        
        
    }
}
