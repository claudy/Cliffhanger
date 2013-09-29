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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
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
        public Vector2 pos, vel, accel, fric, gravity, jumpvel;
        public Vector2 center, position, jumpstartposition;
        Point frameSize;
        Point sheetSize;
        public bool canjump = true;
        const int defaultMillisecondsPerFrame = 16;
        enum PlayerDirection { left, right };
        enum PlayerAction { standing, running, jumping, climbing};
        PlayerAction playerAction = PlayerAction.standing;
        PlayerDirection facingDirection = PlayerDirection.right;
        KeyboardState pkbs = new KeyboardState();
        KeyboardState nkbs = new KeyboardState();

        //Input
        ClaudyInput input;
        int playerNum;

        public Player(Game game, int playerNumber)
            : base(game)
        {
            // TODO: Construct any child components here
            playerNum = playerNumber;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            pkbs = nkbs;
            nkbs = Keyboard.GetState();
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            // TODO: Add your update code here
            //based on keyboard state, swithc what is displayed
            timeinrunning += gameTime.ElapsedGameTime.Milliseconds;
            #region keystatelogic

            if (nkbs.IsKeyDown(Keys.Right) || input.GetAs8DirectionLeftThumbStick(playerNum).X > 0)
            {
                facingDirection = PlayerDirection.right;
                playerAction = PlayerAction.running;
                vel += accel * gameTime.ElapsedGameTime.Milliseconds;
                if (vel.X > MAX_SPEED)
                {
                    vel.X = MAX_SPEED;
                }
            }
            else if (!(nkbs.IsKeyDown(Keys.Left)||input.GetAs8DirectionLeftThumbStick(playerNum).X < 0) && vel.X > 0)
            {
                vel -= fric * gameTime.ElapsedGameTime.Milliseconds;
                if (vel.X < 0 && vel.X > -1.5)
                {
                    vel.X = 0;
                }
            }
            else if (nkbs.IsKeyDown(Keys.Left) || input.GetAs8DirectionLeftThumbStick(playerNum).X < 0)
            {
                facingDirection = PlayerDirection.left;
                playerAction = PlayerAction.running;
                vel -= accel * gameTime.ElapsedGameTime.Milliseconds;
                if (vel.X < -MAX_SPEED)
                {
                    vel.X = -MAX_SPEED;
                }
            }
            else if (!(nkbs.IsKeyDown(Keys.Right)|| input.GetAs8DirectionLeftThumbStick(playerNum).X > 0) && vel.X < 0)
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

            if ((nkbs.IsKeyDown(Keys.Up) && pkbs.IsKeyUp(Keys.Up)) || input.isFirstPress(Buttons.A, playerNum))
            {
                rundrawmodifier = 0;
                Jump(gameTime);
                //add logic to stop from doing other running functions
            }

            if (canjump)
            {
                vel += gravity * gameTime.ElapsedGameTime.Milliseconds * gameTime.ElapsedGameTime.Milliseconds / 100;
                if (vel.Y > 10)
                {
                    vel.Y = 10;
                }
            }
            else
            {
                playerAction = PlayerAction.jumping;
                vel.X = vel.X / 1.1F;
                if ((nkbs.IsKeyDown(Keys.Left) && pkbs.IsKeyUp(Keys.Left)))
                {
                    if (vel.X > 0)
                    {
                        vel.X *= -.5F;
                        jumpvel.X = vel.X;
                    }
                }
                if (nkbs.IsKeyDown(Keys.Right) && pkbs.IsKeyUp(Keys.Right))
                {
                    if (vel.X < 0)
                    {
                        vel.X *= -.5F;
                        jumpvel.X = vel.X;
                    }
                }
                if (nkbs.IsKeyDown(Keys.Right))
                {
                    vel += accel * gameTime.ElapsedGameTime.Milliseconds;
                }
                if (nkbs.IsKeyDown(Keys.Left))
                {
                    vel -= accel * gameTime.ElapsedGameTime.Milliseconds;
                }

                vel += gravity * gameTime.ElapsedGameTime.Milliseconds * gameTime.ElapsedGameTime.Milliseconds / 100;
            }

            if (vel.Y > 10)
            {
                vel.Y = 10;
            }
            
            position += vel * gameTime.ElapsedGameTime.Milliseconds / 10;

            #endregion
            base.Update(gameTime);

        }

        private void Jump(GameTime gameTime)
        {
            if (canjump)
            {
                jumpvel = vel;
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
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle((int)0 * (int)frameSize.X, (playerNum-1) * (int)frameSize.Y, (int)frameSize.X, (int)frameSize.Y), Color.Wheat);
                }
                //running needs to have a sort of update
                if (playerAction == PlayerAction.running)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle((int)rundrawmodifier * (int)frameSize.X, (playerNum - 1) * (int)frameSize.Y, (int)frameSize.X, (int)frameSize.Y), Color.Wheat);
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
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(0 * frameSize.X, (playerNum - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
                }
            }
            #endregion
            #region right
            if (facingDirection == PlayerDirection.right)
            {
                //static
                if (playerAction == PlayerAction.standing)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(0 * frameSize.X, (playerNum - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
                }
                //running needs to have a sort of update
                if (playerAction == PlayerAction.running)
                {
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(rundrawmodifier * frameSize.X, (playerNum - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
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
                    spriteBatch.Draw(celsheet, position + offset, new Rectangle(0 * frameSize.X, (playerNum - 1) * frameSize.Y, frameSize.X, frameSize.Y), Color.Wheat);
                }
            }
            #endregion
        }

    }
}
