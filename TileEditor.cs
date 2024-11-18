using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public class TileEditor
    {
        public static Dictionary<string, Rectangle> rectMap =
        new Dictionary<string, Rectangle>
        {
            { "0000", new Rectangle(0, 0, 32, 32) },
            { "0100", new Rectangle(33, 0, 32, 32) },
            { "1000", new Rectangle(66, 0, 32, 32) },
            { "1100", new Rectangle(99, 0, 32, 32) },
            { "0001", new Rectangle(0, 33, 32, 32) },
            { "0101", new Rectangle(33, 33, 32, 32) },
            { "1001", new Rectangle(66, 33, 32, 32) },
            { "1101", new Rectangle(99, 33, 32, 32) },
            { "0010", new Rectangle(0, 66, 32, 32) },
            { "0110", new Rectangle(33, 66, 32, 32) },
            { "1010", new Rectangle(66, 66, 32, 32) },
            { "1110", new Rectangle(99, 66, 32, 32) },
            { "0011", new Rectangle(0, 99, 32, 32) },
            { "0111", new Rectangle(33, 99, 32, 32) },
            { "1011", new Rectangle(66, 99, 32, 32) },
            { "1111", new Rectangle(99, 99, 32, 32) }
        };
    }
}
