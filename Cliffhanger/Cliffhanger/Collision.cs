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

        public static bool PlayerPlatformCollision(Player player, Platform platform)
        {
            if ((player.position.Y + player.hitbox.Height > platform.position.Y) && player.position.Y + player.hitbox.Height - mailmanThresh < platform.position.Y)
            {
                if ((player.position.X + player.hitbox.Width > platform.position.X + platformEdge) && player.position.X < platform.position.X + platform.platformRect.Width - platformEdge)
                {
                    player.position.Y = platform.position.Y - player.hitbox.Height;
                    player.vel.Y = 0;
                    return true;
                }
                else return false;
            }
            else return false;
        }

        static int vineThreshold = 10;
        public static bool PlayerVineCollision(Player player, Vine vine, GameTime gameTime)
        {
            if ((player.position.X + player.hitbox.Width/2 < vine.lane * 100 + vine.vineRect.Width/2 + 75 + vineThreshold)
                && (player.position.X + player.hitbox.Width / 2 > vine.lane * 100 + 75 - vineThreshold))
            {
                if ((player.position.Y + player.hitbox.Height > vine.vineRect.Y) && (player.position.Y + player.hitbox.Height < vine.vineRect.Y + vine.vineRect.Height))
                {
                    //player.position.Y = platform.position.Y - player.hitbox.Height;
                    player.position.Y -= player.vel.Y * gameTime.ElapsedGameTime.Milliseconds / 10;
                    player.vel.X *= .5f;
                    player.vel.Y = 0;
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
