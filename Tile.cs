using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public enum TileType
    {
        Wall,
        Path
    }
    public class Tile
    {
        public TileType Type { get; set; }
        private Texture2D _texture;
        public Vector2 Pos { get; private set; }
        public char Name { get; set; }
        private Color _color;
        private const float _FallSpeed = 90f;
        public Tile(Vector2 pos, Texture2D texture, TileType type, Color color, char name)
        {
            Pos = pos;
            _texture = texture;
            Type = type;
            _color = color;
            Name = name;
        }
        public void Update(GameTime gameTime)
        {
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Pos, null, _color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }
        public void SwitchTile(Texture2D newTexture)
        {
            _texture = newTexture;
        }
    }
}
