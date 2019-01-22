using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Galaga
{
    class Enemy
    {
        public Rectangle pos;
        public Rectangle spritePos;
        public int health;
        public int value;
        public bool alive;
        public int move;

        public Enemy(Rectangle p, Rectangle sp)
        {
            pos = p;
            spritePos = sp;
            health = 1;
            move = 3;
            alive = true;
            if (spritePos.Y == 174)
                value = 100;
            if (spritePos.Y == 152)
                value = 50;
            if (spritePos.Y == 200)
                value = 100; ;
        }
    }
}
