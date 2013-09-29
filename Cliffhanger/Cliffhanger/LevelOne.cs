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
    public class LevelOne : Microsoft.Xna.Framework.GameComponent
    {
        //Viewport stuff
        RenderTarget2D topScreen;
        RenderTarget2D bottomScreen;
        int bottomOffset;

        //Cliff
        Texture2D cliffTex;
        Rectangle cliffRect;
        Vector2 offsetTop;
        Vector2 offsetBottom;

        //Test Player stuff
        Vector2 p1ScreenPos, p2ScreenPos;
        Vector2 p1ScreenVel, p2ScreenVel;
        Vector2 playerDifference;
        Texture2D test;
        Boolean screenSplit;
        Color p1, p2;
        Boolean swapped;


        //Player Stuff
        Player player1;
        Player player2;

        //Platform
        List<Platform> platforms;
        Platform ground;

        //Rock
        List<Rock> rocks;

        //Vine
        List<Vine> vines;


        GraphicsDevice GraphicsDevice;
        

        public LevelOne(Game game)
            : base(game)
        {
        }


        public void Initialize(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
            topScreen = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2);
            bottomScreen = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2);

            offsetTop = Vector2.Zero;
            offsetBottom = new Vector2(0, -topScreen.Height);

            //Player
            player1 = new Player(Game, 1);
            player1.Initialize();
            player1.position = new Vector2(100, -100);
            player2 = new Player(Game, 2);
            player2.Initialize();
            player2.position = new Vector2(400, -100);

            //Platform
            platforms = new List<Platform>();
            ground = new Platform(Game, 10, 256, 800, 100);
            ground.Initialize();
            platforms.Add(ground);


            //Test players
            p1ScreenPos = new Vector2(200, 200);
            p2ScreenPos = new Vector2(400, 200);
            p1ScreenVel = new Vector2(0, 0);
            p2ScreenVel = new Vector2(0, 0);
            playerDifference = new Vector2(0, 0);

            screenSplit = false;

            p1 = Color.Red;
            p2 = Color.Blue;
            swapped = false;


            //Rock
            rocks = new List<Rock>();
            //Testrock
            rocks.Add(new Rock(Game, 0.0f, 0.0f, new Vector2(-0.01f, 0.01f)));
            rocks[0].Initialize();
            rocks[0].velocity = new Vector2(5.2f, 0.5f);

            //Vine
            vines = new List<Vine>();
            vines.Add(new Vine(Game, -200, 10, 0)); // (Game, Position Y, Height/32, Lane)
            vines.Add(new Vine(Game, -200, 12, 1));
            vines.Add(new Vine(Game, -200, 9, 2));
            vines.Add(new Vine(Game, -200, 10, 3));
            vines.Add(new Vine(Game, -200, 12, 4));
            vines.Add(new Vine(Game, -200, 9, 5));
            vines.Add(new Vine(Game, -200, 12, 6));
            vines.Add(new Vine(Game, -200, 12, 7));
            vines.Add(new Vine(Game, -200, 12, 8));
            
            foreach(Vine vine in vines)
            {
                vine.Initialize();
            }

            base.Initialize();
        }

        public void LoadContent()
        {
            cliffTex = Game.Content.Load<Texture2D>("cliff");
            cliffRect = new Rectangle(0, GraphicsDevice.Viewport.Height - cliffTex.Height * 2, GraphicsDevice.Viewport.Width * 2, cliffTex.Height * 2);

            test = Game.Content.Load<Texture2D>("blankTex");
        }

        public void Update(GameTime gameTime, ClaudyInput input, Rectangle  titleSafeRect)
        {
            //cliffTop += (int)(input.GetAs8DirectionLeftThumbStick().Y * 10);
            //offsetTop.Y += (int)(input.GetAs8DirectionLeftThumbStick().Y * 5);
            //offsetBottom.Y += (int)(input.GetAs8DirectionRightThumbStick().Y * 5);

            
            p1ScreenVel.Y = input.GetAs8DirectionRightThumbStick().Y * 5;

            p2ScreenVel.Y = input.GetAs8DirectionLeftThumbStick().Y * 5;


            p1ScreenPos.Y -= p1ScreenVel.Y;

            p2ScreenPos.Y -= p2ScreenVel.Y;

            playerDifference.Y += p1ScreenVel.Y;

            if (p1ScreenPos.Y < titleSafeRect.Y )
            {
                p1ScreenPos.Y = titleSafeRect.Y;
                offsetTop.Y += p1ScreenVel.Y;

                if (p2ScreenPos.Y > titleSafeRect.Height - 55)
                {
                    screenSplit = true;
                }

                if (!screenSplit)
                {
                    p2ScreenPos.Y += p1ScreenVel.Y;
                    offsetBottom.Y += p1ScreenVel.Y;
                }   
            }
            if (p2ScreenPos.Y < titleSafeRect.Y)
            {
                p2ScreenPos.Y = titleSafeRect.Y;
                offsetTop.Y += p2ScreenVel.Y;

                if (p1ScreenPos.Y > titleSafeRect.Height - 55 && !screenSplit)
                {
                    screenSplit = true;
                }

                if (!screenSplit)
                {
                    p1ScreenPos.Y += p2ScreenVel.Y;
                    offsetBottom.Y += p2ScreenVel.Y;
                }
            }
            else if (p2ScreenPos.Y > titleSafeRect.Height - 50)
            {
                offsetBottom.Y += p2ScreenVel.Y;
                p2ScreenPos.Y = titleSafeRect.Height - 50;
                if (p1ScreenPos.Y < titleSafeRect.Y && !screenSplit)
                {
                    screenSplit = true;
                }

                if (!screenSplit)
                {
                    p1ScreenPos.Y += p2ScreenVel.Y;
                    offsetTop.Y += p2ScreenVel.Y;
                }
            }
            if (p1ScreenPos.Y > titleSafeRect.Height - 50)
            {
                offsetBottom.Y += p1ScreenVel.Y;
                p1ScreenPos.Y = titleSafeRect.Height - 50;
                if (p2ScreenPos.Y < titleSafeRect.Y && !screenSplit)
                {
                    screenSplit = true;
                }

                if (!screenSplit)
                {
                    p2ScreenPos.Y += p1ScreenVel.Y;
                    offsetTop.Y += p1ScreenVel.Y;
                }
            }
           

            if (screenSplit)
            {
                if (p1ScreenPos.Y > GraphicsDevice.Viewport.Height / 2 - 50 && p1ScreenPos.Y < GraphicsDevice.Viewport.Height /2 - 30)
                {
                    p1ScreenPos.Y = GraphicsDevice.Viewport.Height / 2 - 50;
                    offsetTop.Y += p1ScreenVel.Y;
                    if (offsetTop.Y <= offsetBottom.Y + GraphicsDevice.Viewport.Height / 2)
                        screenSplit = false;

                }
                if (p2ScreenPos.Y > GraphicsDevice.Viewport.Height / 2 - 50 && p2ScreenPos.Y < GraphicsDevice.Viewport.Height / 2 - 30)
                {
                    p2ScreenPos.Y = GraphicsDevice.Viewport.Height / 2 - 50;
                    offsetTop.Y += p2ScreenVel.Y;
                    if (offsetTop.Y <= offsetBottom.Y + GraphicsDevice.Viewport.Height / 2)
                        screenSplit = false;

                }
                if (p2ScreenPos.Y < GraphicsDevice.Viewport.Height / 2 && p2ScreenPos.Y > GraphicsDevice.Viewport.Height / 2 - 20)
                {
                    p2ScreenPos.Y = GraphicsDevice.Viewport.Height / 2;
                    offsetBottom.Y += p2ScreenVel.Y;
                    if (offsetTop.Y <= offsetBottom.Y + GraphicsDevice.Viewport.Height / 2)
                        screenSplit = false;

                }
                if (p1ScreenPos.Y < GraphicsDevice.Viewport.Height / 2 && p1ScreenPos.Y > GraphicsDevice.Viewport.Height / 2 - 20)
                {
                    p1ScreenPos.Y = GraphicsDevice.Viewport.Height / 2;
                    offsetBottom.Y += p1ScreenVel.Y;
                    if (offsetTop.Y <= offsetBottom.Y + GraphicsDevice.Viewport.Height / 2)
                        screenSplit = false;

                }
            }




            if (offsetTop.Y <= offsetBottom.Y + GraphicsDevice.Viewport.Height / 2)
            {
                screenSplit = false;
                offsetBottom.Y = offsetTop.Y - GraphicsDevice.Viewport.Height / 2;
            }
            

            player1.Update(gameTime);

            player2.Update(gameTime);

            ground.Update(gameTime);

            #region Platform Collision
            foreach (Platform platform in platforms)
            {
                if (player1.vel.Y >= 0)
                {
                    if (Collision.PlayerPlatformCollision(player1, platform))
                    {
                        //state = PlayerState.standing;
                        player1.canjump = true;
                        break;
                    }
                    else
                    {
                        //state = PlayerState.falling;
                        player1.canjump = false;
                    }
                }
            }
            foreach (Platform platform in platforms)
            {
                if (player2.vel.Y >= 0)
                {
                    if (Collision.PlayerPlatformCollision(player2, platform))
                    {
                        //state = PlayerState.standing;
                        player2.canjump = true;
                        break;
                    }
                    else
                    {
                        //state = PlayerState.falling;
                        player2.canjump = false;
                    }
                }
            }
            #endregion //Platform Collision

            foreach (Rock r in rocks)
            {
                r.Update(gameTime, ground);
            }

            #region Vine Collision
            foreach (Vine vine in vines)
            {
                if (player1.vel.Y >= 0)
                {
                    if (Collision.PlayerVineCollision(player1, vine, gameTime))
                    {
                        //state = PlayerState.standing;
                        player1.canjump = true;
                        break;
                    }
                    else
                    {
                        //state = PlayerState.falling;
                        //player1.canjump = false;
                    }
                }
            }
            foreach (Vine vine in vines)
            {
                if (player2.vel.Y >= 0)
                {
                    if (Collision.PlayerVineCollision(player2, vine, gameTime))
                    {
                        //state = PlayerState.standing;
                        player2.canjump = true;
                        break;
                    }
                    else
                    {
                        //state = PlayerState.falling;
                        //player2.canjump = false;
                    }
                }
            }
            #endregion //Vine Collision

            #region Throw Rocks (requires knowledge of player & of the rock list)

            //PLAYER 1
            if (input.GamepadByID[1].Triggers.Left > 0.5f &&
                input.PreviousGamepadByID[1].Triggers.Left <= 0.5f)
            {
                Rock r = new Rock(Game,
                    player1.position.X, player1.position.Y,
                    Rock.SUGGESTED_L_VELOCITY,
                    player1.Num);
                r.Initialize();
                rocks.Add(r);
            }
            if (input.GamepadByID[1].Triggers.Right > 0.5f &&
                input.PreviousGamepadByID[1].Triggers.Right <= 0.5f)
            {
                Rock r = new Rock(Game,
                    player1.position.X, player1.position.Y,
                    Rock.SUGGESTED_R_VELOCITY,
                    player1.Num);
                r.Initialize();
                rocks.Add(r);
            }
            //PLAYER 2
            if (input.GamepadByID[2].Triggers.Left > 0.5f &&
                input.PreviousGamepadByID[2].Triggers.Left <= 0.5f)
            {
                Rock r = new Rock(Game,
                    player2.position.X, player2.position.Y,
                    Rock.SUGGESTED_L_VELOCITY,
                    player2.Num);
                r.Initialize();
                rocks.Add(r);
            }
            if (input.GamepadByID[2].Triggers.Right > 0.5f &&
                input.PreviousGamepadByID[2].Triggers.Right <= 0.5f)
            {
                Rock r = new Rock(Game,
                    player2.position.X, player2.position.Y,
                    Rock.SUGGESTED_R_VELOCITY,
                    player2.Num);
                r.Initialize();
                rocks.Add(r);
            }

            #endregion



            base.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw stuff in the top renderTarget
            GraphicsDevice.SetRenderTarget(topScreen);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null); // TODO: Change to LinearWrap before submitting to Dr. Birmingham.

            #region Top Viewport
            {
                spriteBatch.Draw(cliffTex, new Rectangle(cliffRect.X, cliffRect.Y + (int)offsetTop.Y, cliffRect.Width, cliffRect.Height), Color.White);
                
                //Vines
                foreach (Vine vine in vines)
                {
                    vine.Draw(spriteBatch, offsetTop);
                }

                player1.Draw(spriteBatch, offsetTop);
                player2.Draw(spriteBatch, offsetTop);
                ground.Draw(spriteBatch, offsetTop);
                foreach (Rock r in rocks)
                {
                    r.Draw(spriteBatch, offsetTop);
                }
            }
            #endregion //Top Viewport

            spriteBatch.End();

            //Draw stuff in the bottom renderTarget; Use an offset
            GraphicsDevice.SetRenderTarget(bottomScreen);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null); // TODO: Change to LinearWrap before submitting to Dr. Birmingham.

            bottomOffset = GraphicsDevice.Viewport.Height;

            #region Bottom Viewport
            {
                spriteBatch.Draw(cliffTex, new Rectangle(cliffRect.X, cliffRect.Y + (int)offsetBottom.Y, cliffRect.Width, cliffRect.Height), Color.White);
                
                //Vines
                foreach (Vine vine in vines)
                {
                    vine.Draw(spriteBatch, offsetBottom);
                }

                player1.Draw(spriteBatch, offsetBottom);
                player2.Draw(spriteBatch, offsetBottom);
                ground.Draw(spriteBatch, offsetBottom);
                foreach (Rock r in rocks)
                {
                    r.Draw(spriteBatch, offsetBottom);
                }
            }
            #endregion //Bottom Viewport

            spriteBatch.End();

            //Draw the renderTargets
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(topScreen, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bottomScreen, new Vector2(0, GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.Draw(test, new Rectangle((int)p1ScreenPos.X, (int)p1ScreenPos.Y, 50, 50), p1);
            spriteBatch.Draw(test, new Rectangle((int)p2ScreenPos.X, (int)p2ScreenPos.Y, 50, 50), p2);

            spriteBatch.End();

        }
    }
}
