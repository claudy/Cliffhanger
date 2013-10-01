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
using Claudy.Input;


namespace Cliffhanger
{
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        SoundEffect jumpingeffect;
        Texture2D celsheet;
        public Rectangle hitbox;
        public float radius;
        int rundrawmodifier = 0;
        int timeinrunning = 0;
        int timeinrunningmax = 30;
        const int MAX_SPEED = 5;
        public Vector2 /*pos,*/ vel, accel, fric, gravity, jumpvel;
        public Vector2 center, position, jumpstartposition;
        Point frameSize;
        Point sheetSize;
        public bool canjump = true;
        public bool canClimb = false;
        const int defaultMillisecondsPerFrame = 16;
        enum PlayerDirection { left, right };
        enum PlayerAction { standing, running, jumping, climbing };
        PlayerAction playerAction = PlayerAction.standing;
        PlayerDirection facingDirection = PlayerDirection.right;

        //Input
        ClaudyInput input;
        int playerNumber;
        public int Num { get { return playerNumber; } protected set { } }

        public Player(Game game, int playerNumber)
            : base(game)
        {
            this.playerNumber = playerNumber;
        }

        public override void Initialize()
        {
            celsheet = Game.Content.Load<Texture2D>("spritesheet_half");
            //jumpingeffect = Game.Content.Load<SoundEffect>("Jumping");
            jumpstartposition = new Vector2(0);
            position = new Vector2(50, 10);
            frameSize = new Point(32, 67);
            sheetSize = new Point(16, 2);
            center = new Vector2(frameSize.X / 2, frameSize.Y / 2);
            hitbox = new Rectangle(0, 0, frameSize.X - 2, frameSize.Y - 2);
            radius = frameSize.X / 2;
            vel = new Vector2(0, 0);
            accel = new Vector2(.015F, 0);
            gravity = new Vector2(0f, .05F);
            fric = new Vector2(.1F, 0);

            //Input
            input = ClaudyInput.Instance;

            base.Initialize();
        }

        public void Update(GameTime gameTime, Rectangle titleSafe)
        {
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            //based on keyboard state, switch what is displayed
            timeinrunning += gameTime.ElapsedGameTime.Milliseconds;
            #region keystatelogic

            if (input.isPressed(Keys.Right) || input.GetAs8DirectionLeftThumbStick(playerNumber).X > 0)
            {
                facingDirection = PlayerDirection.right;
                playerAction = PlayerAction.running;
                vel += accel * gameTime.ElapsedGameTime.Milliseconds;
                if (vel.X > MAX_SPEED)
                {
                    vel.X = MAX_SPEED;
                }
            }
            else if (!(input.isPressed(Keys.Left)||input.GetAs8DirectionLeftThumbStick(playerNumber).X < 0) && vel.X > 0)
            {
                vel -= fric * gameTime.ElapsedGameTime.Milliseconds;
                if (vel.X < 0 && vel.X > -1.5)
                {
                    vel.X = 0;
                }
            }
            else if (input.isPressed(Keys.Left) || input.GetAs8DirectionLeftThumbStick(playerNumber).X < 0)
            {
                facingDirection = PlayerDirection.left;
                playerAction = PlayerAction.running;
                vel -= accel * gameTime.ElapsedGameTime.Milliseconds;
                if (vel.X < -MAX_SPEED)
                {
                    vel.X = -MAX_SPEED;
                }
            }
            else if (!(input.isPressed(Keys.Right) || input.GetAs8DirectionLeftThumbStick(playerNumber).X > 0) && vel.X < 0)
            {
                vel += fric * gameTime.ElapsedGameTime.Milliseconds;
                if (vel.X > 0 && vel.X < 1.5)
                {
                    vel.X = 0;
                }
            }
            else
            {
                rundrawmodifier = 0;
                playerAction = PlayerAction.standing;
            }

            if ((input.isFirstPress(Keys.Up) || input.isFirstPress(Buttons.A, playerNumber)))
            {
                rundrawmodifier = 0;
                Jump(gameTime);
                //add logic to stop from doing other running functions
            }
            
            if (canjump)
            {

                vel += gravity * gameTime.ElapsedGameTime.Milliseconds * gameTime.ElapsedGameTime.Milliseconds / 100;
                if (vel.Y > 5)
                {
                    vel.Y = 5;
                }
            }
            else
            {
                playerAction = PlayerAction.jumping;
                vel.X = vel.X / 1.1F;
                if (input.isFirstPress(Keys.Up))
                {
                    if (vel.X > 0)
                    {
                        vel.X *= -.5F;
                        jumpvel.X = vel.X;
                    }
                }
                if (input.isFirstPress(Keys.Right))
                {
                    if (vel.X < 0)
                    {
                        vel.X *= -.5F;
                        jumpvel.X = vel.X;
                    }
                }
                if (input.isPressed(Keys.Right))
                {
                    vel += accel * gameTime.ElapsedGameTime.Milliseconds;
                }
                if (input.isPressed(Keys.Left))
                {
                    vel -= accel * gameTime.ElapsedGameTime.Milliseconds;
                }

                vel += gravity * gameTime.ElapsedGameTime.Milliseconds * gameTime.ElapsedGameTime.Milliseconds / 100;
            }

            if (vel.Y > 5)
            {
                vel.Y = 5;
            }

            if (vel.Y > 0 && vel.Y < .3f && canjump)
            {
                vel.Y = 0;
            }
            position += vel * gameTime.ElapsedGameTime.Milliseconds / 10;

            if (position.X < titleSafe.Left)
            {
                position.X = 0;
            }
            if (position.X + hitbox.Width > titleSafe.Right)
            {
                position.X = titleSafe.Right - hitbox.Width;
            }


            #endregion
            base.Update(gameTime);

        }

        private void Jump(GameTime gameTime)
        {
            if (canjump)
            {
                //jumpvel = vel;
                vel.Y -= .25f * gameTime.ElapsedGameTime.Milliseconds;
                //jumpingeffect.Play();
                canjump = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            //based on some arbitrary information, display a particular subset of the celsheet
            #region left
            if (facingDirection == PlayerDirection.left)
            {
                if (playerAction == PlayerAction.standing)
                {
                    rundrawmodifier = 0;
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle((int)0 * (int)frameSize.X, (playerNumber-1) * (int)frameSize.Y, (int)frameSize.X, (int)frameSize.Y), Color.Wheat);
                }
                //running needs to have a sort of update
                if (playerAction == PlayerAction.running)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle((int)rundrawmodifier * (int)frameSize.X, (playerNumber - 1) * (int)frameSize.Y, (int)frameSize.X, (int)frameSize.Y), Color.Wheat);
                    if (rundrawmodifier < sheetSize.X-1) 
                    { 
                        if (timeinrunning > timeinrunningmax) 
                        { 
                            rundrawmodifier++; 
                            timeinrunning = 0; 
                        } 
                    }
                    else { 
                        if (timeinrunning > timeinrunningmax) 
                        { 
                            rundrawmodifier = 0; 
                            timeinrunning = 0; 
                        } 
                    }
                }
                if (playerAction == PlayerAction.jumping)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(0 * frameSize.X, (playerNumber - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
                }
            }
            #endregion
            #region right
            if (facingDirection == PlayerDirection.right)
            {
                //static
                if (playerAction == PlayerAction.standing)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(0 * frameSize.X, (playerNumber - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
                }
                //running needs to have a sort of update
                if (playerAction == PlayerAction.running)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(rundrawmodifier * frameSize.X, (playerNumber - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
                    if (rundrawmodifier < sheetSize.X-1) 
                    { 
                        if (timeinrunning > timeinrunningmax) 
                        { 
                            rundrawmodifier++; 
                            timeinrunning = 0; 
                        } 
                    }
                    else 
                    { 
                        if (timeinrunning > timeinrunningmax) 
                        { 
                            rundrawmodifier = 0; 
                            timeinrunning = 0; 
                        } 
                    }
                }
                //static
                if (playerAction == PlayerAction.jumping)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(0 * frameSize.X, (playerNumber - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
                }
            }
            #endregion
        }

        /// <summary>
        /// Call this function only if the index of the controller controlling this avatar has changed.
        /// </summary>
        /// <param name="newPlayerIndex">Should only be a 1 or a 2 for the Cliffhanger game.</param>
        public void ChangePlayerIndex(int newPlayerIndex)
        {
            //if (!(newPlayerIndex == 1 || newPlayerIndex == 2 || newPlayerIndex == 3 || newPlayerIndex == 4))
            if (!(newPlayerIndex == 1 || newPlayerIndex == 2))
            {
                throw new ArgumentOutOfRangeException();
            }
            
            playerNumber = newPlayerIndex;
        }
    }
}
