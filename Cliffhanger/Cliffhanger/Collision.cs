/*******************************************************************
 * Originally: Clayton Sandham & Pat Neville
 * Adapted from the COMP 441 - Project 2: Going Postal
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
            if ((player.position.Y + player.hitbox.Height > platform.position.Y) && 
                player.position.Y + player.hitbox.Height - mailmanThresh < platform.position.Y)
            {
                if ((player.position.X + player.hitbox.Width > platform.position.X + platformEdge) &&
                    (player.position.X < platform.position.X + platform.platformRect.Width - platformEdge))
                {
                    player.position.Y = platform.position.Y - player.hitbox.Height + 2;
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
                if ((player.position.Y + player.hitbox.Height > vine.vineRect.Y) &&
                    (player.position.Y + player.hitbox.Height < vine.vineRect.Y + vine.vineRect.Height))
                {
                   
                    return true;
                }
                
            }
            return false;
        }

        public static bool PlayerRockCollision(Player player, Rock rock)
        {
            // Don't bother testing if the rock already collided with someone.
            // Don't bother testing if the player who being checked threw the rock.
            if (!rock.hasCollidedWithAPlayer && rock.IndexOfPlayerWhoThrewMe != player.Num)
            {
                if ((player.position.X + player.hitbox.Width / 2 > rock.HitBox.X - rock.HitBox.Width / 2) &&
                   (player.position.X - player.hitbox.Width / 2 < rock.HitBox.X + rock.HitBox.Width / 2))
                {
                    if ((player.position.Y + player.hitbox.Height / 2 > rock.currentPosition.Y - rock.HitBox.Height / 2) &&
                        (player.position.Y - player.hitbox.Height / 2 < rock.currentPosition.Y + rock.HitBox.Height / 2))
                    {
                        //Affect player
                        player.vel.X += 10f * Math.Sign(rock.velocity.X); //4f is the impact constant.
                        rock.velocity.X -= rock.velocity.X / 2f;
                        
                        rock.hasCollidedWithAPlayer = true;
                        rock.shade = Color.DeepPink; //Change rock for debug purposes
                    }
                }
            }
            //else for any in this nested if block, return false.
            return false;
        }
    }
}
