using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public class PlayerController : AnimatedGameObject
    {
        private Vector2 direction;
        private Vector2 newDirection;
        private Vector2 destination;
        bool moving = false;

        public PlayerController(Texture2D texture, Vector2 position, Color color, float rotation, float size, float layerDepth, Vector2 origin, Dictionary<string, AnimationClip> animationClips) : base(texture, position, color, rotation, size, layerDepth, origin, animationClips)
        {
        }

        public override void Update(GameTime gameTime)
        {

            if (!moving)
            {
                newDirection = GetNewDirection();

                if (newDirection.Length() != 0)
                    direction = newDirection;

                var newDestination = Position + direction * 32;
                destination = newDestination;
                moving = true;
            }
            else
            {
                int tileSize = 32;
                int numberToTestSpeed = 50;
                Position += direction * (tileSize + numberToTestSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Vector2.Distance(Position, destination) < 1)
                {
                    Position = destination;
                    moving = false;
                }
                //Debug.WriteLine(destination);
            }

            base.Update(gameTime);
        }
        private Vector2 GetNewDirection()
        {
            Vector2 direction = Vector2.Zero;
            //if (InputManager.CurrentKeyboard != InputManager.PreviousKeyboard)
            //{
               
            //Debug.WriteLine("I go");
            //}

            direction = InputManager.GetMovement();
            if(direction.Length() != 0)
            {
                if (direction.X != 0)
                {
                    if (direction.X == -1)
                    {
                        Rotation = MathHelper.ToRadians(180);
                    }
                    else
                    {
                        Rotation = MathHelper.ToRadians(0);
                    }
                }
                else
                {
                    if (direction.Y == -1)
                    {
                        Rotation = MathHelper.ToRadians(270);
                    }
                    else
                    {
                        Rotation = MathHelper.ToRadians(90);
                    }
                }
            }
            return direction;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
