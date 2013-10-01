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
        }

        public override void Initialize()
        {
            platform = Game.Content.Load<Texture2D>("blankTex");
            platformRect = new Rectangle((int)position.X, (int)position.Y, width, height);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(platform, new Rectangle(platformRect.X, platformRect.Y + (int)offset.Y, platformRect.Width, platformRect.Height), Color.DarkGreen);
        }
    }
}
