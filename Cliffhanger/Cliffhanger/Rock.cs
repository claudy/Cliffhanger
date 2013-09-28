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
    public class Rock : Microsoft.Xna.Framework.GameComponent
    {
        public const float ROCKGRAVITY = -10f; // Gravity
        public Vector2 startPosition;
        public Vector2 currentPosition;
        public Vector2 velocity;
        private Rectangle rect;
        protected const int WH = 64; //Size of texture in pixel, Assumes Width == height

        public Rock(Game game, float startXPos, float startYPos, Vector2 velocity)
            : base(game)
        {
            startPosition = new Vector2(startXPos, startYPos);
            currentPosition = startPosition;
            this.velocity = velocity;
            rect = new Rectangle((int)startXPos,
                (int)startYPos,
                WH,
                WH);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Update(GameTime gameTime, Platform ground)
        {
            if (currentPosition.Y < ground.position.Y)
                this.Dispose();

            currentPosition.X += velocity.X;
            currentPosition.Y += velocity.Y;

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D rockTexture)
        {
            rect.X = (int)currentPosition.X;
            rect.Y = (int)currentPosition.Y;
            rect.Width = WH;
            rect.Height = WH;
            spriteBatch.Draw(rockTexture, rect, Color.White);
        }
    }
}
