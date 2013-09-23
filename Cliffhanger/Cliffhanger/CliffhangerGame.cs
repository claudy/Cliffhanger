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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ClaudyInput test;

        SpriteFont consolas;

        public CliffhangerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            test = ClaudyInput.Instance;
        }

        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            consolas = Content.Load<SpriteFont>("Consolas");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            test.Update();

            // Allows the game to exit
            if (test.DetectBackPressedByAnyPlayer())
                this.Exit();

            if (isPluggedIn = test.AreAnyControllersPluggedIn())
            {

                buttonsPressed = test.GamePadCurrent1.Buttons;
            }
            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        #region Testing Vars
        GamePadButtons buttonsPressed;
        bool isPluggedIn;

        readonly Color byDirect = Color.Blue;
        readonly Color byID = Color.YellowGreen;
        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(consolas,
            "test.GamePadCurrent1.IsConnected=" +
                test.GamePadCurrent1.IsConnected.ToString() + "\n" +
            "test.GamePadCurrent2.IsConnected=" +
                test.GamePadCurrent2.IsConnected.ToString() + "\n" +
            "test.GamePadCurrent3.IsConnected=" +
                test.GamePadCurrent3.IsConnected.ToString() + "\n" +
            "test.GamePadCurrent4.IsConnected=" +
                test.GamePadCurrent4.IsConnected.ToString(),
            Vector2.Zero,
            byDirect);

            spriteBatch.DrawString(consolas,
            "test.GamepadByID[1].IsConnected=" +
                test.GamepadByID[1].IsConnected.ToString() + "\n" +
            "test.GamepadByID[2].IsConnected=" +
                test.GamepadByID[2].IsConnected.ToString() + "\n" +
            "test.GamepadByID[3].IsConnected=" +
                test.GamepadByID[3].IsConnected.ToString() + "\n" +
            "test.GamepadByID[4].IsConnected=" +
                test.GamepadByID[4].IsConnected.ToString(),
            new Vector2(0f, 75f),
            byID);

            spriteBatch.DrawString(consolas,
                "AreAnyControllersPluggedIn()=" + isPluggedIn.ToString(),
                new Vector2(0f, 150f),
                byID);

            spriteBatch.DrawString(consolas,
                "test.GamePadCurrent1.Buttons = " + buttonsPressed.ToString(),
                new Vector2(0f, 170f),
                byDirect);
            spriteBatch.DrawString(consolas,
                "test.GamepadByID[1].Buttons.ToString()" + test.GamepadByID[1].Buttons.ToString(),
                new Vector2(0f, 200f),
                byID);



            spriteBatch.DrawString(consolas,
                "test.GetLeftThumbStickAs8Direction()" + test.GetAs8DirectionLeftThumbStick(),
                new Vector2(0f, 300f),
                byDirect);
            spriteBatch.DrawString(consolas,
                "test.GetRightThumbStickAs8Direction()" + test.GetAs8DirectionRightThumbStick(),
                new Vector2(0f, 380f),
                byDirect);
            spriteBatch.DrawString(consolas,
                "test.GetLeftThumbStickAs8Direction(P_)" + test.GetAs8DirectionLeftThumbStick(PlayerIndex.One),
                new Vector2(200f, 320f),
                byID);
            spriteBatch.DrawString(consolas,
                "test.GetLeftThumbStickAs8Direction(int)" + test.GetAs8DirectionLeftThumbStick(1),
                new Vector2(200f, 340f),
                byID);
            spriteBatch.DrawString(consolas,
                "test.GetRightThumbStickAs8Direction(P_)" + test.GetAs8DirectionRightThumbStick(PlayerIndex.One),
                new Vector2(200f, 400f),
                byID);
            spriteBatch.DrawString(consolas,
                "test.GetRightThumbStickAs8Direction(int)" + test.GetAs8DirectionRightThumbStick(1),
                new Vector2(200f, 420f),
                byID);

            spriteBatch.End();

            // Claudy calls dibs on viewports drawing code.

            base.Draw(gameTime);
        }
    }
}
