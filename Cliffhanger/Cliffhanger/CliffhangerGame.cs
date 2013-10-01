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
            Splash,
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
        LevelOne level2;

        Rectangle titleSafeRect;

        Texture2D victoryScreen;
        int player1Score, player2Score;
        int playerVictorNumber;

        Texture2D splash;
        int splashLength = 250;

        public SpriteFont consolas;
        public SpriteFont tahoma;
        Music music;

        public CliffhangerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1200;
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
            
            currentGameState = LevelStateFSM.Splash;

            //titleSafe
            titleSafeRect = GraphicsDevice.Viewport.TitleSafeArea;

            player1Score = 0;
            player2Score = 0;
            playerVictorNumber = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            consolas = Content.Load<SpriteFont>("consolas");
            tahoma = Content.Load<SpriteFont>("Tahoma");
            level1.LoadContent();
            mainMenu = new Menu(this);
            music = new Music(this);
            music.playBackgroundMusic();
            victoryScreen = Content.Load<Texture2D>("CliffClimbed");
            splash = Content.Load<Texture2D>("steve");
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
            for (int pi = 1; pi <= 4; pi++)
            {
                if (input.isFirstPress(Buttons.Start, pi))
                {
                    //CHEAT CODE: Advance Level.
                    switch (currentGameState)
                    {
                        case LevelStateFSM.AlphaMenu:
                            currentGameState = LevelStateFSM.Level1;
                            break;
                        case LevelStateFSM.Level1:
                            currentGameState = LevelStateFSM.Level1Ending;
                            break;
                        case LevelStateFSM.Level1Ending:
                            currentGameState = LevelStateFSM.Level2;
                            break;
                        case LevelStateFSM.Level2:
                            currentGameState = LevelStateFSM.Level2Ending;
                            break;
                        case LevelStateFSM.Level2Ending:
                            currentGameState = LevelStateFSM.Level1;
                            break;
                        // Disabled until level 3 is implemented.
                        case LevelStateFSM.Level3:
                            break; // Disabled on level 3.
                        default:
                            break; // Default is do nothing. (e.g. when on a victory screen.)
                    }
                }
            }
            //////////////////////////////////////

            switch (currentGameState)
            {
                case LevelStateFSM.Splash:
                    splashLength--;
                    if (splashLength <= 0)
                    {
                        currentGameState = LevelStateFSM.AlphaMenu;
                    }
                    break;
                case LevelStateFSM.AlphaMenu:
                    mainMenu.Update(gameTime, this);
                    break;
                case LevelStateFSM.Level1:
                    level1.Update(gameTime, input, titleSafeRect);
                    if (level1.isCompleted)
                        currentGameState = LevelStateFSM.Level1Ending;
                    break;
                case LevelStateFSM.Level1Ending:
                    //level1.victorPlayerNum;
                    //level1.Dispose();
                    if (level1.victorPlayerNum == 1)
                    {
                        player1Score++;
                        playerVictorNumber = 1;
                        level1.victorPlayerNum = 0;
                    }
                    else if (level1.victorPlayerNum == 2)
                    {
                        player2Score++;
                        playerVictorNumber = 2;
                        level1.victorPlayerNum = 0;
                    }
                    level2 = new LevelOne(this);
                    level2.Initialize(GraphicsDevice);
                    level2.LoadContent();
                    break;
                case LevelStateFSM.Level2:
                    level2.Update(gameTime, input, titleSafeRect);
                    if (level2.isCompleted)
                    {
                        currentGameState = LevelStateFSM.Level2Ending;
                    }
                    break;
                case LevelStateFSM.Level2Ending:
                    //level2.Dispose();
                    if (level2.victorPlayerNum == 1)
                    {
                        player1Score++;
                        playerVictorNumber = 1;
                        level2.victorPlayerNum = 0;
                    }
                    else if(level2.victorPlayerNum == 2)
                    {
                        player2Score++;
                        playerVictorNumber = 2;
                        level2.victorPlayerNum = 0;
                    }
                    level1 = new LevelOne(this);
                    level1.Initialize(GraphicsDevice);
                    level1.LoadContent();
                    break;
                case LevelStateFSM.Level3:
                    break;
                case LevelStateFSM.Level3Ending:
                    break;
                default:
                    break;
            }


            // CHEAT CODE: Music Testing Code.
            if (input.isPressed(Buttons.X)) music.stopBackgroundMusic();
            if (input.isPressed(Buttons.Y)) music.playBackgroundMusic();
            // Delete Music Testing Code once convinced of functionality of music class.
            // OOOOORRRRR Just leave it in as a cheat code.

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (currentGameState)
            {
                case LevelStateFSM.Splash:
                    spriteBatch.Draw(splash, new Rectangle(titleSafeRect.X, titleSafeRect.Y, titleSafeRect.Width, titleSafeRect.Height), Color.White);
                    break;
                case LevelStateFSM.AlphaMenu:
                    mainMenu.Draw(spriteBatch);
                    break;
                case LevelStateFSM.Level1:
                    spriteBatch.End(); //Not quite kosher.  Refactor?
                    level1.Draw(spriteBatch);
                    spriteBatch.Begin(); // Refactor?
                    break;
                case LevelStateFSM.Level1Ending:
                    spriteBatch.Draw(victoryScreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.DrawString(tahoma, "Press The Start Button To Continue...", new Vector2(titleSafeRect.X, titleSafeRect.Y), Color.Black);
                    if(playerVictorNumber == 1)
                        spriteBatch.DrawString(tahoma, "Congrats Player Red", new Vector2(titleSafeRect.X, titleSafeRect.Height / 2 + 100), Color.Yellow);
                    else
                        spriteBatch.DrawString(tahoma, "Congrats Player Blue", new Vector2(titleSafeRect.X, titleSafeRect.Height / 2 + 100), Color.Yellow);
                    spriteBatch.DrawString(tahoma, "Player 1 Score: " + player1Score.ToString() + "    Player 2 Score: " + player2Score.ToString(),
                                           new Vector2(titleSafeRect.X, titleSafeRect.Height / 2 + 200), Color.Yellow);
                    break;
                case LevelStateFSM.Level2:
                    spriteBatch.End(); //Not quite kosher.  Refactor?
                    level2.Draw(spriteBatch);
                    spriteBatch.Begin();
                    break;
                case LevelStateFSM.Level2Ending:
                    spriteBatch.Draw(victoryScreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.DrawString(tahoma, "Press The Start Button To Continue...", new Vector2(titleSafeRect.X, titleSafeRect.Y), Color.Black);
                    if(playerVictorNumber == 1)
                        spriteBatch.DrawString(tahoma, "Congrats Player Red", new Vector2(titleSafeRect.X, titleSafeRect.Height / 2 + 100), Color.Yellow);
                    else
                        spriteBatch.DrawString(tahoma, "Congrats Player Blue", new Vector2(titleSafeRect.X, titleSafeRect.Height / 2 + 100), Color.Yellow);
                    spriteBatch.DrawString(tahoma, "Player 1 Score: " + player1Score.ToString() + "    Player 2 Score: " + player2Score.ToString(),
                                           new Vector2(titleSafeRect.X, titleSafeRect.Height / 2 + 200), Color.Yellow);
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
