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


namespace Cliffhanger
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Platform : Microsoft.Xna.Framework.GameComponent
    {
        Texture2D platform;
        public Rectangle platformRect;
        public Vector2 position;
        int height, width;

        public Platform(Game game, int x, int y, int w, int h)
            : base(game)
        {
            position.X = x;
            position.Y = y;
            height = h;
            width = w;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            platform = Game.Content.Load<Texture2D>("Platform");
            platformRect = new Rectangle((int)position.X, (int)position.Y, width, height);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(platform, platformRect, Color.Wheat);
        }
    }
}
