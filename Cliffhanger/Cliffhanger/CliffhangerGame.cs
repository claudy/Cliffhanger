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
        #region Menu
        public enum LevelStateFSM
        {
            AlphaMenu,
            Level1,
            Level1Ending,
            Level2,
            Level2Ending,
            Level3,
            Level3Ending
        }
        public LevelStateFSM currentGameState;
        Menu mainMenu;
        #endregion //Menu

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public ClaudyInput input;

        LevelOne level1;

        public SpriteFont consolas;
        Music music;

        public CliffhangerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            input = ClaudyInput.Instance;
#if WINDOWS
            graphics.PreferredBackBufferWidth = 980;
            graphics.PreferredBackBufferHeight = 540;
            graphics.IsFullScreen = false;
#endif
        }

        protected override void Initialize()
        {
            level1 = new LevelOne(this);
            level1.Initialize(GraphicsDevice);
            
            currentGameState = LevelStateFSM.AlphaMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            consolas = Content.Load<SpriteFont>("consolas");

            level1.LoadContent();
            mainMenu = new Menu(this);
            music = new Music(this);
            music.playBackgroundMusic();
        }

        protected override void UnloadContent()
        {

        }

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
                    level1.Update(gameTime, input);
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


            // Music Testing Code.
            if (input.isPressed(Buttons.B)) music.stopBackgroundMusic();
            if (input.isPressed(Buttons.Y)) music.playBackgroundMusic();
            // Delete Music Testing Code once convinced of functionality of music class.

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (currentGameState)
            {
                case LevelStateFSM.AlphaMenu:
                    mainMenu.Draw(spriteBatch);
                    break;
                case LevelStateFSM.Level1:
                    spriteBatch.End(); //Not quite kosher.  Refactor?
                    level1.Draw(spriteBatch);
                    spriteBatch.Begin(); // Refactor?
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
