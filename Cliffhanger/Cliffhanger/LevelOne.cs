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

        //Platform
        List<Platform> platforms;
        Platform ground;

        GraphicsDevice GraphicsDevice;
        

        public LevelOne(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }


        public void Initialize(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
            topScreen = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2);
            bottomScreen = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2);

            offsetTop = Vector2.Zero;
            offsetBottom = new Vector2(0, -topScreen.Height);

            //Player
            player1 = new Player(Game);
            player1.Initialize();

            //Platform
            platforms = new List<Platform>();
            ground = new Platform(Game, 10, GraphicsDevice.Viewport.Height - 100, 800, 100);
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
            ground.Update(gameTime);

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

            base.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw stuff in the top renderTarget
            GraphicsDevice.SetRenderTarget(topScreen);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();

            #region Top Viewport
            {
                spriteBatch.Draw(cliffTex, new Rectangle(cliffRect.X, cliffRect.Y + (int)offsetTop.Y, cliffRect.Width, cliffRect.Height), Color.White);
                player1.Draw(spriteBatch, offsetTop);
                ground.Draw(spriteBatch, offsetTop);
            }
            #endregion //Top Viewport

            spriteBatch.End();

            //Draw stuff in the bottom renderTarget; Use an offset
            GraphicsDevice.SetRenderTarget(bottomScreen);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();

            bottomOffset = GraphicsDevice.Viewport.Height;

            #region Bottom Viewport
            {
                spriteBatch.Draw(cliffTex, new Rectangle(cliffRect.X, cliffRect.Y + (int)offsetBottom.Y, cliffRect.Width, cliffRect.Height), Color.White);
                player1.Draw(spriteBatch, offsetBottom);
                ground.Draw(spriteBatch, offsetBottom);
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
