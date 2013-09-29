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
        private const int TIME_TO_LIVE = 5; // In seconds.
        public static readonly Vector2 SUGGESTED_L_VELOCITY = new Vector2(-4.5f, -5.2f);
        public static readonly Vector2 SUGGESTED_R_VELOCITY = new Vector2(4.5f, -5.2f);
        public const float ROCKGRAVITY = -.01f; // Gravity

        public Vector2 startPosition;
        public Vector2 currentPosition;
        public Vector2 velocity;

        protected Rectangle hitbox; // Center is 0, 0; NOT upper left.
        public Rectangle HitBox { get { return hitbox; } protected set { } }
        
        protected const int WH = 32; //Size of texture in pixel, Assumes Width == height
        protected static Texture2D rockTex;
        public Color shade = Color.White;

        public bool hasCollidedWithAPlayer = false;
        protected int indexOfPlayerWhoThrewMe = 0;
        public int IndexOfPlayerWhoThrewMe { get { return indexOfPlayerWhoThrewMe; } protected set {} }

        private TimeSpan secondsAlive;

        public Rock(Game game, float startXPos, float startYPos, Vector2 velocity, int playerIndex)
            : base(game)
        {
            startPosition = new Vector2(startXPos, startYPos);
            currentPosition = startPosition;
            this.velocity = velocity;
            hitbox = new Rectangle((int)startXPos,
                (int)startYPos,
                WH,
                WH);
            indexOfPlayerWhoThrewMe = playerIndex;
            if (rockTex == null)
            {
                rockTex = Game.Content.Load<Texture2D>("asteroid_cell64_Warspawn_OpenGameArt");
            }
            
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Update(GameTime gameTime, Platform ground)
        {
            secondsAlive += gameTime.ElapsedGameTime;
            if (secondsAlive.TotalSeconds > TIME_TO_LIVE)
            {
                this.Enabled = false; // Deem this rock ready to be disposed.
            }

            // In order for gravity to work...this must -=.
            velocity.Y -= ROCKGRAVITY * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentPosition.X += velocity.X / 1.1f;
            currentPosition.Y += velocity.Y;

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            hitbox.X = (int)(currentPosition.X - WH / 2);
            hitbox.Y = (int)(currentPosition.Y + offset.Y + WH / 2);
            hitbox.Width = WH;
            hitbox.Height = WH;
            spriteBatch.Draw(rockTex, hitbox, shade);
        }
    }
}
