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
        private Vector2 _startPosition;
        public Tile[,] _tiles;

        public static Vector2 TileSize { get; private set; } = new Vector2(31, 31);

        public bool LevelCompleted { get; set; } = false;
       // public event Action<Tile> TileSteppedOnHandler;
        private GameObjectFactory _factory;
        public List<GameObject> GameObjectsInLevel { get; } = new();
        public Level()
        {
            _factory = new GameObjectFactory();
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
        public void CreateLevel(string file, Vector2 startPosition, List<(char TileName, Texture2D tileTexture, TileType type, Color tileColor)> tileConfigurations, List<char> GameObjectConfigurations)
        {
            List<string> result = FileManager.ReadFromFile(file);
            _startPosition = startPosition;
            _tiles = new Tile[result[0].Length, result.Count];

            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[0].Length; j++)
                {
                    foreach (var tileConfig in tileConfigurations)
                    {
                        if (result[i][j] == tileConfig.TileName)
                        {
                            _tiles[j, i] = new Tile(new Vector2(TileSize.X * j + startPosition.X, TileSize.Y * i + startPosition.Y), tileConfig.tileTexture, tileConfig.type, tileConfig.tileColor, tileConfig.TileName);
                            break;
                        }
                        else
                        {
                            //Default - Path
                            string defaultTextureString = "empty";
                            TileType defaultTileType = TileType.Path;
                            Color defaultColor = Color.White;
                            char defaultName = 'p';
                            Rectangle _sourceRec = new Rectangle(0, 0, 30, 30);
                            _tiles[j, i] = new Tile(new Vector2(TileSize.X * j + startPosition.X, TileSize.Y * i + startPosition.Y), ResourceManager.GetTexture(defaultTextureString), defaultTileType, defaultColor, defaultName);
                        }

                        foreach (char GameObjectName in GameObjectConfigurations)
                        {
                              
                            if (result[i][j] == GameObjectName)
                            {
                                GameObjectsInLevel.Add(_factory.CreateGameObjectFromType(GameObjectName.ToString()));
                                break;
                            }
                            //if (result[i][j] == GameObjectName)
                            //{
                            //    //Create a player
                            //    break;
                            //}
                            //if (result[i][j] == GameObjectName)
                            //{
                            //    //Create an enemy
                            //    break;
                            //}
                            //if (result[i][j] == GameObjectName)
                            //{
                            //    //Create a pickup
                            //    break;
                            //}
                        }

                    }
                }
            }
            GameManager.GameObjects.AddRange(GameObjectsInLevel);
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


        public static List<(char TileName, Texture2D tileTexture, TileType Type, Color tileColor)> ReadTileDataFromFile(string fileName)
        {
            List<(char, Texture2D, TileType, Color)> tileData = new List<(char, Texture2D, TileType, Color)>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(' ');

                    if (line.Length == 4)
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
                        // Add the tile to the list, converting the texture name to a Texture2D object
                        tileData.Add((tileName, ResourceManager.GetTexture(textureName), type, color));
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

        public bool IsTileWall(Vector2 vec)
        {
            Point tilePos = GetTileAtPosition(vec);

           // tilePos.Y += 1;
            if(!TileExistsAtPosition(tilePos))
            {
                Console.WriteLine("Outside of bounds, so will default to false");
                return false;
            }

            return (_tiles[tilePos.X, tilePos.Y].Type == TileType.Wall);
        }
        private Point GetTileAtPosition(Vector2 vec)
        {
            vec -= _startPosition;
            return new Point((int)vec.X / (int)TileSize.X, (int)vec.Y / (int)TileSize.Y);
        }
        private bool TileExistsAtPosition(Point tilePos)
        {
            if (tilePos.X < 0 || tilePos.X >= _tiles.GetLength(0)) return false;
            if (tilePos.Y < 0 || tilePos.Y >= _tiles.GetLength(1)) return false;
           // TileSteppedOnHandler?.Invoke(_tiles[tilePos.X, tilePos.Y]);
            return true;
        }

        public string GetTileKey(Point tilePosition)
        {
            Point tilePos = GetTileAtPosition(tilePosition.ToVector2());

            string tileKey = string.Empty;
            (int y, int x)[] directions = { (-1, 0), (0, 1), (1, 0), (0, -1) };

            foreach (var (y, x) in directions)
            {
                int newY = tilePos.Y + y;
                int newX = tilePos.X + x;

               // tileKey += IsTileWall(new(newX, newY)) ? "0" : "1";
               if(!TileExistsAtPosition(new Point(newX, newY)))
                {
                    tileKey += "0";
                    continue;
                }
                tileKey += (_tiles[newX, newY].Type == TileType.Path) ? "0" : "1";
            }
            return tileKey;
        }
    }
}
