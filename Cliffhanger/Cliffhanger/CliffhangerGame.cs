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
        enum MenuState 
        {
            TopMost,
            Help,
            InGame,
            Exit
        }

        /// <summary>
        /// Finite State Machine of the menu.
        /// </summary>
        MenuState currentMenuState;

        enum MenuChoice // THIS ENUMERATION MUST BE IN ORDER.
        {
            Play, 
            Help,
            Exit // THIS MUST BE LAST.
        }

        MenuChoice currentlySelectedMenuChoice;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ClaudyInput input;

        

        #region Textures
        Texture2D helpScreenTexture;
        #endregion

        SpriteFont calibri, consolas;
        Vector2 playMenuItemPos, helpMenuItempPos, exitMenuItemPos;
        readonly Color colorSelectYES = Color.Red, colorSelectNO = Color.White;

        LevelOne levelOne;
        public CliffhangerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            input = ClaudyInput.Instance;
        }

        protected override void Initialize()
        {
            levelOne = new LevelOne(this);
            levelOne.Initialize(GraphicsDevice);
            
            currentGameState = LevelStateFSM.AlphaMenu;
            currentMenuState = MenuState.TopMost;
            currentlySelectedMenuChoice = MenuChoice.Help;            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            levelOne.LoadContent();

            calibri = Content.Load<SpriteFont>("calibri");
            consolas = Content.Load<SpriteFont>("consolas");

            helpScreenTexture = Content.Load<Texture2D>("helpScreenTexture");

            playMenuItemPos = new Vector2(GraphicsDevice.Viewport.Width / 4.0f, GraphicsDevice.Viewport.Height / 2.0f - GraphicsDevice.Viewport.Height * 0.1f);
            helpMenuItempPos = new Vector2(GraphicsDevice.Viewport.Width / 4.0f, GraphicsDevice.Viewport.Height / 2.0f * 1.0f);
            exitMenuItemPos =  new Vector2(GraphicsDevice.Viewport.Width / 4.0f, GraphicsDevice.Viewport.Height / 2.0f + GraphicsDevice.Viewport.Height * 0.1f);
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
                            for (int pi = 1; pi <= 2; pi++)
                            {
                                if (input.GamepadByID[pi].IsConnected)
                                {
                                    if ((input.GamepadByID[pi].DPad.Up == ButtonState.Pressed && input.PreviousGamepadByID[pi].DPad.Up == ButtonState.Released) ||
                                        (input.GamepadByID[pi].ThumbSticks.Left.Y > 0.5f && input.PreviousGamepadByID[pi].ThumbSticks.Left.Y <= 0.5f) ||
                                        (input.GamepadByID[pi].ThumbSticks.Right.Y > 0.5f && input.PreviousGamepadByID[pi].ThumbSticks.Right.Y <= 0.5f))
                                    {
                                        if (currentlySelectedMenuChoice != MenuChoice.Play)
                                            currentlySelectedMenuChoice--;
                                    }
                                }
                                if (input.GamepadByID[pi].IsConnected)
                                {
                                    if (input.GamepadByID[pi].DPad.Down == ButtonState.Pressed && input.PreviousGamepadByID[pi].DPad.Down == ButtonState.Released||
                                        (input.GamepadByID[pi].ThumbSticks.Left.Y < -0.5f && input.PreviousGamepadByID[pi].ThumbSticks.Left.Y >= -0.5f) ||
                                        (input.GamepadByID[pi].ThumbSticks.Right.Y < -0.5f && input.PreviousGamepadByID[pi].ThumbSticks.Right.Y >= -0.5f))
                                    {
                                        if (currentlySelectedMenuChoice != MenuChoice.Exit)
                                        currentlySelectedMenuChoice++;
                                    }
                                }
                                if ((input.isFirstPress(Buttons.A, PlayerIndex.One) || input.isFirstPress(Buttons.A, PlayerIndex.Two)))
                                {
                                    switch (currentlySelectedMenuChoice)
                                    {
                                        case MenuChoice.Play:
                                            currentMenuState = MenuState.InGame;
                                            currentGameState = LevelStateFSM.Level1;
                                            break;
                                        case MenuChoice.Help:
                                            currentMenuState = MenuState.Help;
                                            break;
                                        case MenuChoice.Exit:
                                            currentMenuState = MenuState.Exit;
                                            break;
                                        default:
                                            break;
                                    }
                                    
                                }
                            }
                            break;
                        case MenuState.Help:
                            for (int pi = 1; pi <= 4; pi++)
                            {
                                if (input.GamepadByID[pi].IsConnected)
                                {
                                    if (input.isFirstPress(Buttons.B))
                                    {
                                        currentGameState = LevelStateFSM.AlphaMenu;
                                        currentMenuState = MenuState.TopMost;
                                        currentlySelectedMenuChoice = MenuChoice.Play;
                                    }
                                }
                            }
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

            levelOne.Update(gameTime, input);
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin();

            switch (currentGameState)
            {
                case LevelStateFSM.AlphaMenu:
                    #region Main Menu
                    switch (currentMenuState)
                    {
                        case MenuState.TopMost:
                            //TODO: BACKGROUND TEXTURE spriteBatch.Draw(menuScreenTexture, GraphicsDevice.Viewport.Bounds, Color.White);
                            switch (currentlySelectedMenuChoice)
	                        {
		                    case MenuChoice.Play:
                                spriteBatch.DrawString(consolas, "Play", playMenuItemPos, colorSelectYES);
                                spriteBatch.DrawString(consolas, "Help", helpMenuItempPos, colorSelectNO);
                                spriteBatch.DrawString(consolas, "Exit", exitMenuItemPos, colorSelectNO);
                                break;
                            case MenuChoice.Help:
                                spriteBatch.DrawString(consolas, "Play", playMenuItemPos, colorSelectNO);
                                spriteBatch.DrawString(consolas, "Help", helpMenuItempPos, colorSelectYES);
                                spriteBatch.DrawString(consolas, "Exit", exitMenuItemPos, colorSelectNO);
                                break;
                            case MenuChoice.Exit:
                                spriteBatch.DrawString(consolas, "Play", playMenuItemPos, colorSelectNO);
                                spriteBatch.DrawString(consolas, "Help", helpMenuItempPos, colorSelectNO);
                                spriteBatch.DrawString(consolas, "Exit", exitMenuItemPos, colorSelectYES);
                                break;
                            default:
                                spriteBatch.DrawString(consolas, "Play", playMenuItemPos, colorSelectNO);
                                spriteBatch.DrawString(consolas, "Help", helpMenuItempPos, colorSelectNO);
                                spriteBatch.DrawString(consolas, "Exit", exitMenuItemPos, colorSelectNO);
                                break;
                            }
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
			spriteBatch.End();



            //levelOne.Draw(spriteBatch, graphics);
            base.Draw(gameTime);
        }
    }
}
