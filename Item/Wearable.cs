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
    public class Wearable : Item
    {
        public ItemState state { get; private set; } = ItemState.pickupState;
        protected float _lifeTimeDuration = 5f;
        protected float _remainingTimeLeft = 0;
        public event Action<Item> OnExpired;

        public Wearable(Texture2D texture, Vector2 position, Rectangle rect, Color color, float rotation, float size, float layerDepth, Vector2 origin) : base(texture, position, rect, color, rotation, size, layerDepth, origin)
        {
            _minScore = 50;
            _maxScore = 150;
            _score = _random.Next(_minScore, _maxScore);

            _remainingTimeLeft = _lifeTimeDuration;
        }
        public override void OnCollision(GameObject gameObject)
        {
            //Maybe add it back to a pool and respawn it
            if (gameObject is PlayerController)
            {
                GameManager.GameObjects.Remove(this);
                state = ItemState.usingState;
                ScoreManager.UpdateScore(_score);
                int resetRotation = 0;
                Rotation = resetRotation;
                AudioManager.PlaySoundEffect("CoinPickupSound");
            }
            // GameManager.GameObjects.Remove(this);
        }
        public override void Update(GameTime gameTime)
        {
            if (state == ItemState.pickupState)
                Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
            {
                _remainingTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_remainingTimeLeft <= 0)
                {
                    Debug.WriteLine("Item expired");
                    OnExpired?.Invoke(this);
                }
            }
        }
    }
}
