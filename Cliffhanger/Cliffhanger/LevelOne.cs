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

        //Test texture
        Texture2D blankTex;
        Rectangle test;

        //Cliff
        Texture2D cliffTex;
        Rectangle cliffRect;
        int cliffTop, cliffBottom;

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
            test = new Rectangle(0, 50, 100, 100);
            cliffTop = 0;
            cliffBottom = 0;
            base.Initialize();
        }

        public void LoadContent()
        {
            blankTex = Game.Content.Load<Texture2D>("blankTex");
            cliffTex = Game.Content.Load<Texture2D>("cliff");
            cliffRect = new Rectangle(0, GraphicsDevice.Viewport.Height - cliffTex.Height * 2, GraphicsDevice.Viewport.Width * 2, cliffTex.Height * 2);
            
        }

        public void Update(GameTime gameTime, ClaudyInput input)
        {
            test.X += (int)(input.GetAs8DirectionLeftThumbStick().X * 10);
            cliffTop += (int)(input.GetAs8DirectionLeftThumbStick().Y * 10);

            if (cliffTop < 500)
                cliffBottom = cliffTop;
            else
                cliffBottom = 500;


            base.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
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

        }
    }
}
