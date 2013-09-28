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
        int cliffTop, cliffBottom;
        Vector2 offsetTop;
        Vector2 offsetBottom;

        //Player Stuff
        Player player1;

        //Platform
        List<Platform> platforms;
        Platform ground;

        //Vine
        List<Vine> vines;
        Vine vine1;

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
            cliffTop = 0;
            cliffBottom = 0;

            offsetTop = Vector2.Zero;
            offsetBottom = new Vector2(0, -topScreen.Height);

            //Player
            player1 = new Player(Game);
            player1.Initialize();
            player1.position = new Vector2(100, -100);

            //Platform
            platforms = new List<Platform>();
            ground = new Platform(Game, 10, 256, 800, 100);
            ground.Initialize();
            platforms.Add(ground);


            //Vine
            vines = new List<Vine>();
            vines.Add(new Vine(Game, -200, 10, 0)); // (Game, Position Y, Height/32, Lane)
            vines.Add(new Vine(Game, -200, 11, 1));
            vines.Add(new Vine(Game, -200, 9, 2));
            vines.Add(new Vine(Game, -200, 10, 3));
            vines.Add(new Vine(Game, -200, 11, 4));
            vines.Add(new Vine(Game, -200, 9, 5));
            vines.Add(new Vine(Game, -200, 10, 6));
            vines.Add(new Vine(Game, -200, 11, 7));
            vines.Add(new Vine(Game, -200, 11, 8));
            
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
            
        }

        public void Update(GameTime gameTime, ClaudyInput input)
        {
            //cliffTop += (int)(input.GetAs8DirectionLeftThumbStick().Y * 10);
            offsetTop.Y += (int)(input.GetAs8DirectionLeftThumbStick().Y * 5);
            offsetBottom.Y += (int)(input.GetAs8DirectionRightThumbStick().Y * 5);

            if (cliffTop < 500)
                cliffBottom = cliffTop;
            else
                cliffBottom = 500;

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
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null);

            #region Top Viewport
            {
                spriteBatch.Draw(cliffTex, new Rectangle(cliffRect.X, cliffRect.Y + (int)offsetTop.Y, cliffRect.Width, cliffRect.Height), Color.White);
                
                //Vines
                foreach (Vine vine in vines)
                {
                    vine.Draw(spriteBatch, offsetTop);
                }

                player1.Draw(spriteBatch, offsetTop);
                ground.Draw(spriteBatch, offsetTop);
            }
            #endregion //Top Viewport

            spriteBatch.End();
            //Draw stuff in the bottom renderTarget; Use an offset
            GraphicsDevice.SetRenderTarget(bottomScreen);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null);

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
                ground.Draw(spriteBatch, offsetBottom);
            }
            #endregion //Bottom Viewport

            spriteBatch.End();

            //Draw the renderTargets
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(topScreen, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bottomScreen, new Vector2(0, GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.End();

        }
    }
}
