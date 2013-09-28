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
    /// This is the main type for your game
    /// </summary>
    public class CliffhangerGame : Microsoft.Xna.Framework.Game
    {
        public enum LevelStateFSM
        {
            /// <summary>
            /// Similar to the alpha dog being the leader of the pack, the alpha menu leads the rest of the levels.
            /// </summary>
            AlphaMenu,
            Level1,
            Level1Ending,
            Level2,
            Level2Ending,
            Level3,
            Level3Ending
        }

        /// <summary>
        /// Finite State Machine of the game mode/current level.
        /// </summary>
        public LevelStateFSM currentGameState;
        Menu mainMenu;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public ClaudyInput input;

        //Viewport stuff
        RenderTarget2D topScreen;
        RenderTarget2D bottomScreen;
        int bottomOffset;

        //Test texture
        Texture2D blankTex;
        Rectangle test;

        //Cliff
        Texture2D cliffTex;
        Rectangle cliffRect;
        int cliffTop, cliffBottom;

        //Textures
        Texture2D helpScreenTexture;
        Texture2D menuScreenTexture;

        public SpriteFont calibri, consolas;

        public CliffhangerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            input = ClaudyInput.Instance;
            graphics.IsFullScreen = false; //TODO: MAKE TRUE ON SATURDAY AFTERNOON
        }

        protected override void Initialize()
        {
            topScreen = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2);
            bottomScreen = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2);
            test = new Rectangle(0, 50, 100, 100);
            cliffTop = 0;
            cliffBottom = 0;
            currentGameState = LevelStateFSM.AlphaMenu;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            blankTex = Content.Load<Texture2D>("blankTex");
            cliffTex = Content.Load<Texture2D>("cliff");
            cliffRect = new Rectangle(0, GraphicsDevice.Viewport.Height - cliffTex.Height*2, GraphicsDevice.Viewport.Width * 2, cliffTex.Height * 2);
            calibri = Content.Load<SpriteFont>("calibri");
            consolas = Content.Load<SpriteFont>("consolas");

            helpScreenTexture = Content.Load<Texture2D>("helpScreenTexture");
            menuScreenTexture = Content.Load<Texture2D>("menuScreenTexture");
            mainMenu = new Menu(this);
        }

        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update(); // ONLY MAKE THIS CALL ONCE PER FRAME!!!1
            // Allows the game to exit
            if (input.DetectBackPressedByAnyPlayer())
                this.Exit();
            //////////////////////////////////////

            switch (currentGameState)
            {
                case LevelStateFSM.AlphaMenu:
                    mainMenu.Update(gameTime, this);
                    break;
                case LevelStateFSM.Level1:
                    break;
                case LevelStateFSM.Level1Ending:
                    break;
                case LevelStateFSM.Level2:
                    break;
                case LevelStateFSM.Level2Ending:
                    break;
                case LevelStateFSM.Level3:
                    break;
                case LevelStateFSM.Level3Ending:
                    break;
                default:
                    break;
            }

            test.X += (int)(input.GetAs8DirectionLeftThumbStick().X*10);
            cliffTop += (int)(input.GetAs8DirectionLeftThumbStick().Y*10);

            if (cliffTop < 500)
                cliffBottom = cliffTop;
            else
                cliffBottom = 500;


            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Draw stuff in the top renderTarget
            graphics.GraphicsDevice.SetRenderTarget(topScreen);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            spriteBatch.Draw(cliffTex, new Rectangle(cliffRect.X, cliffRect.Y + cliffTop, cliffRect.Width, cliffRect.Height), Color.White);
            spriteBatch.Draw(blankTex, test, Color.Red);
            spriteBatch.End();
            //Draw stuff in the bottom renderTarget; Use an offset
            graphics.GraphicsDevice.SetRenderTarget(bottomScreen);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            bottomOffset = GraphicsDevice.Viewport.Height;
            spriteBatch.Draw(cliffTex, new Rectangle(cliffRect.X, cliffRect.Y - bottomOffset + cliffBottom, cliffRect.Width, cliffRect.Height), Color.White);
            spriteBatch.Draw(blankTex, new Rectangle(test.X, test.Y - bottomOffset, test.Width, test.Height), Color.Blue);

            spriteBatch.End();

            //Draw the renderTargets
            graphics.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(topScreen, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bottomScreen, new Vector2(0, GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            switch (currentGameState)
            {
                case LevelStateFSM.AlphaMenu:
                    mainMenu.Draw(spriteBatch,
                        consolas,
                        menuScreenTexture,
                        helpScreenTexture);
                    break;
                case LevelStateFSM.Level1:
                    break;
                case LevelStateFSM.Level1Ending:
                    break;
                case LevelStateFSM.Level2:
                    break;
                case LevelStateFSM.Level2Ending:
                    break;
                case LevelStateFSM.Level3:
                    break;
                case LevelStateFSM.Level3Ending:
                    break;
                default:
                    break;
            }
			spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
