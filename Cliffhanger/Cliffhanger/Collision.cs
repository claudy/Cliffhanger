/*******************************************************************
 * Clayton Sandham & Pat Neville
 * COMP 441 - Project 2: Going Postal
 * Date: 2012, Nov 12
 * 
 * Details: This file contains the implementation of collides, which is a collection of collision functions
 ******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Cliffhanger
{
    public static class Collision
    {
        static int platformEdge = 17;
        static int mailmanThresh = 20;

        public static bool PlayerPlatformCollision(Player mailman, Platform platform)
        {
            if ((mailman.position.Y + mailman.hitbox.Height > platform.position.Y) && mailman.position.Y + mailman.hitbox.Height - mailmanThresh < platform.position.Y)
            {
                if ((mailman.position.X + mailman.hitbox.Width > platform.position.X + platformEdge) && mailman.position.X < platform.position.X + platform.platformRect.Width - platformEdge)
                {
                    mailman.position.Y = platform.position.Y - mailman.hitbox.Height;
                    mailman.vel.Y = 0;
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
