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
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix view { get; set; }
        public Matrix projection { get; set; }
        public Vector3 cameraPos, cameraDisplacement;
        public float radius = .1f;

        private KeyboardState ks;
        private MouseState previousMouseState;
        public Vector3 lookAt;
        private Vector3 cameraTarget = Vector3.Zero;
        private Vector3 cameraUpVector = Vector3.Up;
        private Vector3 cameraReference = new Vector3(0.0f, 0.0f, -1.0f);

        private float yaw;
        private float pitch;
        private float roll;

        private float movementVelocity = .5f;

        private float spinRate = 90f;

        private Vector3 dir; //direction the user wants to move the camera

        private float timeDelta; //time between calls to  update

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Initialize view matrix
            cameraPos = pos;
            lookAt = target;
            view = Matrix.CreateLookAt(pos, target, up);
            cameraDisplacement = Vector3.Zero;
            // Initialize projection matrix
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.Pi / 8f,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                radius, 5000);
        }


        public override void Initialize()
        {

            base.Initialize();
        }

        private void cameraInput()
        {

            dir = new Vector3(0);
            ks = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            if (ks.IsKeyDown(Keys.I)) dir.Z--;
            if (ks.IsKeyDown(Keys.K)) dir.Z++;
            if (ks.IsKeyDown(Keys.J)) dir.X--;
            if (ks.IsKeyDown(Keys.L)) dir.X++;

            if (ks.IsKeyDown(Keys.OemPeriod)) dir.Y++;
            if (ks.IsKeyDown(Keys.OemComma)) dir.Y--;

            if (ks.IsKeyDown(Keys.R)) cameraDisplacement = Vector3.Zero;

            if ((previousMouseState.X > mState.X) && (mState.LeftButton == ButtonState.Pressed))
            {
                yaw += (spinRate * timeDelta);
            }
            else if ((previousMouseState.X < mState.X) && (mState.LeftButton == ButtonState.Pressed))
            {
                yaw -= (spinRate * timeDelta);
            }
            if (yaw > 360)
                yaw -= 360;
            else if (yaw < 0)
                yaw += 360;

            if ((previousMouseState.Y > mState.Y) && (mState.LeftButton == ButtonState.Pressed))
                pitch += (spinRate * timeDelta);
            else if ((previousMouseState.Y < mState.Y) && (mState.LeftButton == ButtonState.Pressed))
                pitch -= (spinRate * timeDelta);
            if (pitch > 360)
                pitch -= 360;
            else if (pitch < 0)
                pitch += 360;
            if ((previousMouseState.ScrollWheelValue > mState.ScrollWheelValue))
                roll++;
            else if ((previousMouseState.ScrollWheelValue < mState.ScrollWheelValue))
                roll--;
            if (roll > 360)
                roll -= 360;
            else if (roll < 0)
                roll += 360;
            previousMouseState = mState;
        }
        public override void Update(GameTime gameTime)
        {
            timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //cameraInput();



            Matrix yawR = Matrix.CreateRotationY(MathHelper.ToRadians(yaw));
            Matrix pitchR = Matrix.CreateRotationX(MathHelper.ToRadians(pitch));
            Matrix rollr = Matrix.CreateRotationX(MathHelper.ToRadians(roll));

            Matrix dirRotation = Matrix.Identity;

            Vector3 dt = Vector3.Zero;

            // cameraPos update
            if (dir != Vector3.Zero)
            {

                dir.Normalize();
                dt = movementVelocity * dir * gameTime.ElapsedGameTime.Milliseconds;
                Vector3.Transform(ref dt, ref dirRotation, out dt);
                cameraDisplacement += dt;
            }
            //lookAt Update
            int cameraMovementCap = 300;
            if (cameraDisplacement.X > cameraMovementCap)
                cameraDisplacement.X = cameraMovementCap;
            if (cameraDisplacement.X < -cameraMovementCap)
                cameraDisplacement.X = -cameraMovementCap;

            if (cameraDisplacement.Y > cameraMovementCap)
                cameraDisplacement.Y = cameraMovementCap;
            if (cameraDisplacement.Y < -cameraMovementCap)
                cameraDisplacement.Y = -cameraMovementCap;

            if (cameraDisplacement.Z > cameraMovementCap)
                cameraDisplacement.Z = cameraMovementCap;
            if (cameraDisplacement.Z < -cameraMovementCap)
                cameraDisplacement.Z = -cameraMovementCap;

            Matrix bar = view;
            cameraTarget = lookAt + dt;
            lookAt += dt;
            Vector3 foo = cameraPos + cameraDisplacement;
            Matrix.CreateLookAt(ref foo, ref cameraTarget, ref cameraUpVector, out bar);
            view = bar;
            roll = 0;


            base.Update(gameTime);
        }


    }
}
