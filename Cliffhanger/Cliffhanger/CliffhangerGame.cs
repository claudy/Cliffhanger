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
        enum LevelStateFSM
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
        LevelStateFSM currentGameState;
        enum MenuState
        {
            TopMost,
            Help,
            Exit
        }

        /// <summary>
        /// Finite State Machine of the menu.
        /// </summary>
        MenuState currentMenuState;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ClaudyInput input;

        #region Textures
        Texture2D helpScreenTexture;
        #endregion

        public CliffhangerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            input = ClaudyInput.Instance;
        }

        protected override void Initialize()
        {
            currentGameState = LevelStateFSM.AlphaMenu;
            currentMenuState = MenuState.TopMost;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            helpScreenTexture = Content.Load<Texture2D>("helpScreenTexture");
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
                    #region Main Menu
                    switch (currentMenuState)
	                {
                        case MenuState.TopMost:
                            break;
                        case MenuState.Help:
                            break;
                        case MenuState.Exit:
                            this.Exit();
                            break;
                        default:
                            break;
	                }
                    #endregion
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (currentGameState)
            {
                case LevelStateFSM.AlphaMenu:
                    #region Main Menu
                    switch (currentMenuState)
                    {
                        case MenuState.TopMost:
                            break;
                        case MenuState.Help:
                            //TODO: Draw 1920x1080 texture which explains how to play the game.
                            spriteBatch.Draw(helpScreenTexture, GraphicsDevice.Viewport.Bounds, Color.White);
                            break;
                        case MenuState.Exit:
                            break;
                        default:
                            break;
                    }
                    #endregion
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
            // Claudy calls dibs on viewports drawing code.

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
