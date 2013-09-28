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
        public float currentPosition;
        public float velocity;


        public Rock(Game game, float startXPos, float startYPos)
            : base(game)
        {
            startPosition = new Vector2(startXPos, startYPos);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
