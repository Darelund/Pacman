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

        private float _health = 3;
        public float Health
        {
            get => _health;
        }
        public bool IsImmune { get; private set; } = false;
        private bool _inAttackMode = false;
        private bool _isActive = true;
        public string Name { get; set; }
        private float _speed = 50;

        private static PlayerController _instance;
        public static PlayerController Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                    return null;
            }
        }
        public PlayerController(Texture2D texture, Vector2 position, Color color, float rotation, float size, float layerDepth, Vector2 origin, Dictionary<string, AnimationClip> animationClips) : base(texture, position, color, rotation, size, layerDepth, origin, animationClips)
        {
            _instance = this;
        }

        public override void Update(GameTime gameTime)
        {

            if (!moving)
            {
                newDirection = GetNewDirection();

                if (newDirection.Length() != 0)
                    direction = newDirection;

                var newDestination = Position + (direction * Level.TileSize);
                destination = newDestination;
                moving = true;
            }
            else
            {
                Position += direction * (Level.TileSize.X + _speed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Vector2.Distance(Position, destination) < 1)
                {
                    Position = destination;
                    moving = false;
                }
                //Debug.WriteLine(destination);
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
        
        public void TakeDamage(int amount)
        {
            //Maybe add thís back later
            // if(!IsImmune)
            _health -= amount;
            if (_health <= 0)
            {
                _isActive = false;
               // HighScore.UpdateScore(Name, ScoreManager.PlayerScore, LevelManager.LevelIndex);
                string deathSound = "DeathSound";
                AudioManager.PlaySoundEffect(deathSound);
                ScoreManager.ResetScore();
            }
            Debug.WriteLine(_health);
        }
        public override void OnCollision(GameObject gameObject)
        {
            if (!_isActive) return;
            if (gameObject is EnemyController)
            {
                // var enemsy = (EnemyController)gameObject;
                if (!IsImmune)
                {
                    Debug.WriteLine("Taking damage");
                    float volume = 0.5f;
                    string damageSound = "FlameDamage";
                    AudioManager.PlaySoundEffect(damageSound, volume);
                    float flashTime = 2f;
                    Color flashColor = Color.White;
                    var flash = new FlashEffect(ResourceManager.GetEffect("FlashEffect"), flashTime, this, flashColor);
                    GameManager.AddFlashEffect(flash);
                    IsImmune = true;
                    flash.OnFlashing += ImmuneHandler;
                    //YEs memory leak, so what??? Or is it?
                    TakeDamage(1);
                }
            }
        }
        public void ImmuneHandler(bool immune)
        {
            IsImmune = immune;
        }
    }
}
