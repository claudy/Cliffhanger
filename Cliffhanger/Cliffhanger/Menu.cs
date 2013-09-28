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
    public class Menu : Microsoft.Xna.Framework.GameComponent
    {
        public readonly Color colorSelectYES = Color.Red, colorSelectNO = Color.SlateGray;
        Vector2 playMenuItemPos, helpMenuItempPos, exitMenuItemPos;
        private ClaudyInput input;

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

        Texture2D helpScreenTexture;
        Texture2D menuScreenTexture;

        SpriteFont calibri, consolas;

        public Menu(CliffhangerGame game)
            : base(game)
        {
            currentMenuState = MenuState.TopMost;
            currentlySelectedMenuChoice = MenuChoice.Help;

            playMenuItemPos = new Vector2(Game.GraphicsDevice.Viewport.Width / 4.0f,
                Game.GraphicsDevice.Viewport.Height / 2.0f - Game.GraphicsDevice.Viewport.Height * 0.1f);
            helpMenuItempPos = new Vector2(Game.GraphicsDevice.Viewport.Width / 4.0f,
                Game.GraphicsDevice.Viewport.Height / 2.0f * 1.0f);
            exitMenuItemPos = new Vector2(Game.GraphicsDevice.Viewport.Width / 4.0f, 
                Game.GraphicsDevice.Viewport.Height / 2.0f + Game.GraphicsDevice.Viewport.Height * 0.1f);

            helpScreenTexture = Game.Content.Load<Texture2D>("helpScreenTexture");
            menuScreenTexture = Game.Content.Load<Texture2D>("menuScreenTexture");
            calibri = Game.Content.Load<SpriteFont>("calibri");
            consolas = Game.Content.Load<SpriteFont>("consolas");

            input = game.input;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, CliffhangerGame game)
        {
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
                            if (input.GamepadByID[pi].DPad.Down == ButtonState.Pressed && input.PreviousGamepadByID[pi].DPad.Down == ButtonState.Released ||
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
                                    game.currentGameState = CliffhangerGame.LevelStateFSM.Level1;
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
                                game.currentGameState = CliffhangerGame.LevelStateFSM.AlphaMenu;
                                currentMenuState = MenuState.TopMost;
                                currentlySelectedMenuChoice = MenuChoice.Play;
                            }
                        }
                    }
                    break;
                case MenuState.Exit:
                    game.Exit();
                    break;
                default:
                    break;
            }
            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch passed MUST have its .Begin() call first.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Assumes SpriteBatch has begun already.
            switch (currentMenuState)
            {
                case MenuState.TopMost:
                    spriteBatch.Draw(menuScreenTexture, Game.GraphicsDevice.Viewport.Bounds, Color.White);
                    switch (currentlySelectedMenuChoice)
                    {
                        case MenuChoice.Play:
                            spriteBatch.DrawString(calibri, "Play", playMenuItemPos, colorSelectYES);
                            spriteBatch.DrawString(calibri, "Help", helpMenuItempPos, colorSelectNO);
                            spriteBatch.DrawString(calibri, "Exit", exitMenuItemPos, colorSelectNO);
                            break;
                        case MenuChoice.Help:
                            spriteBatch.DrawString(calibri, "Play", playMenuItemPos, colorSelectNO);
                            spriteBatch.DrawString(calibri, "Help", helpMenuItempPos, colorSelectYES);
                            spriteBatch.DrawString(calibri, "Exit", exitMenuItemPos, colorSelectNO);
                            break;
                        case MenuChoice.Exit:
                            spriteBatch.DrawString(calibri, "Play", playMenuItemPos, colorSelectNO);
                            spriteBatch.DrawString(calibri, "Help", helpMenuItempPos, colorSelectNO);
                            spriteBatch.DrawString(calibri, "Exit", exitMenuItemPos, colorSelectYES);
                            break;
                        default:
                            spriteBatch.DrawString(calibri, "Play", playMenuItemPos, colorSelectNO);
                            spriteBatch.DrawString(calibri, "Help", helpMenuItempPos, colorSelectNO);
                            spriteBatch.DrawString(calibri, "Exit", exitMenuItemPos, colorSelectNO);
                            break;
                    }
                    break;
                case MenuState.Help:
                    //TODO: Draw 1920x1080 texture which explains how to play the game.
                    spriteBatch.Draw(helpScreenTexture, Game.GraphicsDevice.Viewport.Bounds, Color.White);
                    break;
                case MenuState.Exit:
                    break;
                default:
                    break;
            }

        }
    }
}
