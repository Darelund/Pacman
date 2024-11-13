using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pacman.GameFiles;

namespace Pacman
{
    public class Level
    {
        protected Vector2 _startPosition;
        protected Tile[,] _tiles;
        public bool LevelCompleted { get; set; } = false;
       // public event Action<Tile> TileSteppedOnHandler;
       // private GameObjectFactory _factory;
       // public List<GameObject> GameObjectsInLevel = new();
        public Level()
        {
          //  _factory = new GameObjectFactory();
        }


        //public List<GameObject> GameObjects = new List<GameObject>();
        public void ActivateLevel()
        {
            CreateLevel(Levels.LevelData.LevelFile, Levels.LevelData.LevelStartPosition, Levels.LevelData.TileData, Levels.LevelData.GameObjectData);
        }

        /// <summary>
        /// Creates a 2D grid level based on the file you give it. In the file each character represents one tile. 
        /// So you give it a list of tuples where each tuple is its character, texture and if you can walk on it.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="tileTexture"></param>
        public void CreateLevel(string file, Vector2 startPosition, List<(char TileName, Texture2D tileTexture, TileType type, Color tileColor, string SpriteCode)> tileConfigurations, List<char> GameObjectConfigurations)
        {
            List<string> result = FileManager.ReadFromFile(file);
            _startPosition = startPosition;
            _tiles = new Tile[result[0].Length, result.Count];

            Vector2 size = new Vector2(40, 40);
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[0].Length; j++)
                {
                    foreach (var tileConfig in tileConfigurations)
                    {
                        if (result[i][j] == tileConfig.TileName)
                        {
                            _tiles[j, i] = new Tile(new Vector2(size.X * j + startPosition.X, size.Y * i + startPosition.Y), tileConfig.tileTexture, tileConfig.type, tileConfig.tileColor, tileConfig.TileName, TileEditor.rectMap[tileConfig.SpriteCode]);
                            break;
                        }
                        else
                        {
                            //Default - Path
                            string defaultTextureString = "empty";
                            TileType defaultTileType = TileType.Path;
                            Color defaultColor = Color.White;
                            char defaultName = 'p';
                            Rectangle _sourceRec = new Rectangle(0, 0, 39, 39);
                            _tiles[j, i] = new Tile(new Vector2(size.X * j + startPosition.X, size.Y * i + startPosition.Y), ResourceManager.GetTexture(defaultTextureString), defaultTileType, defaultColor, defaultName, _sourceRec);
                        }

                        foreach (char GameObjectName in GameObjectConfigurations)
                        {
                            if (result[i][j] == GameObjectName)
                            {
                                //Create a player
                                break;
                            }
                            if (result[i][j] == GameObjectName)
                            {
                                //Create an enemy
                                break;
                            }
                            if (result[i][j] == GameObjectName)
                            {
                                //Create a pickup
                                break;
                            }
                        }
                        
                    }
                }
            }
        }
        
        public virtual bool CheckLevelCompletion()
        {
            return false;
        }
       
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in _tiles)
            {
                tile.Draw(spriteBatch);
            }
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void UnloadLevel()
        {
            _tiles = null;
          //  GameObjectsInLevel.Clear();
        }


        //Maybe use?
        public void CreateGameObjects(string objectsFilePath)
        {
            //List<string> fileLines = FileManager.ReadFromFile(objectsFilePath);

            //int i = 0;
            //while (i < fileLines.Count)
            //{
            //    // Get the object type from the first line
            //    string objectType = fileLines[i].Trim();
            //    i++;  // Move to the next line for object data

            //    // Read the relevant properties for each object
            //    List<string> objectData = new List<string>();

            //    while (i < fileLines.Count && !string.IsNullOrWhiteSpace(fileLines[i]))
            //    {
            //        objectData.Add(fileLines[i].Trim());
            //        i++;
            //    }

            //    // Create the appropriate game object based on the object type
            //    GameObject newObject = _factory.CreateGameObjectFromType(objectType, objectData);

            //    if (newObject != null)
            //    {
            //        GameObjectsInLevel.Add(newObject);
            //    }

            //    i++;  // Skip the blank line between object definitions
            //}
        }
       // public abstract void SetTarget();


        public static List<(char TileName, Texture2D tileTexture, TileType Type, Color tileColor, string SpriteCode)> ReadTileDataFromFile(string fileName)
        {
            List<(char, Texture2D, TileType, Color, string)> tileData = new List<(char, Texture2D, TileType, Color, string)>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(' ');

                    if (line.Length == 5)
                    {
                        char tileName = line[0][0];
                        string textureName = line[1];
                        TileType type = (TileType)Enum.Parse(typeof(TileType), line[2]);
                        string colorName = line[3].Trim();
                        Color color = colorName switch
                        {
                            "White" => Color.White,
                            "Red" => Color.Red,
                            "Brown" => Color.Brown,
                            "Blue" => Color.Blue,
                            "Green" => Color.Green,
                            "DarkGreen" => Color.DarkGreen,
                            _ => Color.White // Default color if not found
                        };
                        string spriteCodeName = line[4].Trim();
                        // Add the tile to the list, converting the texture name to a Texture2D object
                        tileData.Add((tileName, ResourceManager.GetTexture(textureName), type, color, spriteCodeName));
                    }
                }
            }

            return tileData;
        }
        public static List<char> ReadGameObjectDataFromFile(string fileName)
        {
            List<char> tileData = new List<char>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    tileData.Add(line[0]);
                }
            }
            return tileData;
        }









        //Used to check if tiles exists and are of x type
        //public bool IsTileWalkable(Vector2 vec)
        //{
        //    Point tilePos = GetTileAtPosition(vec);

        //    if (!TileExistsAtPosition(tilePos)) return false;

        //    return !(_tiles[tilePos.X, tilePos.Y].Type == TileType.NonWalkable);
        //}

        public bool IsTilePath(Vector2 vec)
        {
            Point tilePos = GetTileAtPosition(vec);

            tilePos.Y += 1;
            TileExistsAtPosition(tilePos);

            return (_tiles[tilePos.X, tilePos.Y].Type == TileType.Path);
        }
        private Point GetTileAtPosition(Vector2 vec)
        {
            int tileSize = 40;
            vec -= _startPosition;
            return new Point((int)vec.X / tileSize, (int)vec.Y / tileSize);
        }
        private bool TileExistsAtPosition(Point tilePos)
        {
            if (tilePos.X < 0 || tilePos.X >= _tiles.GetLength(0)) return false;
            if (tilePos.Y < 0 || tilePos.Y >= _tiles.GetLength(1)) return false;
           // TileSteppedOnHandler?.Invoke(_tiles[tilePos.X, tilePos.Y]);
            return true;
        }
    }
}
