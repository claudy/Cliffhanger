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

    public class RockMachine : Microsoft.Xna.Framework.GameComponent
    {
        public const int COOLDOWN = 3; // In seconds.
        private readonly Color active = Color.White, inactive = Color.Gray;
        public Rectangle rect;
        private static Texture2D rockMachineTex;
        private bool isActive = true;
        public bool IsActive { get { return isActive; } protected set { } }
        private TimeSpan secondsCooldown; 

        public RockMachine(Game game, float xPos, float yPos)
            : base(game)
        {
            rect = new Rectangle((int)xPos, (int)yPos, 47, 125);

            if (rockMachineTex == null)
            {
                rockMachineTex = Game.Content.Load<Texture2D>("RockMachine_kentz_96");
            }
            this.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            secondsCooldown += gameTime.ElapsedGameTime;
            if (secondsCooldown.TotalSeconds >= COOLDOWN)
            {
                isActive = true;
            }
            base.Update(gameTime);
        }

        //Similar to strum...called after making rocks fall from the sky.
        public void Fired(GameTime gameTime)
        {
            isActive = false;
            secondsCooldown = gameTime.ElapsedGameTime;
            // TODO: Play a cool guitar sound here.
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (isActive)
            {
                spriteBatch.Draw(rockMachineTex,
                    new Rectangle(rect.X, rect.Y + (int)offset.Y, rect.Width, rect.Height),
                    active);
            }
            else
            {
                spriteBatch.Draw(rockMachineTex, 
                    new Rectangle(rect.X, rect.Y + (int)offset.Y, rect.Width, rect.Height),
                    inactive);
            }
        }
    }
}
