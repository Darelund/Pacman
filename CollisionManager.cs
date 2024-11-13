﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public static class CollisionManager
    {
        public static List<GameObject> _collidables => GameManager.GetGameObjects;
        public static void CheckCollision()
        {
            Debug.WriteLine(_collidables.Count);
            for (int i = 0; i < _collidables.Count; i++)
            {
                for (int j = i + 1; j < _collidables.Count; j++)
                {
                    if (_collidables[i].Collision.Intersects(_collidables[j].Collision))
                    {
                        _collidables[j].OnCollision(_collidables[i]);
                        _collidables[i].OnCollision(_collidables[j]);//Problem med den sista?
                    }
                }
            }
          //  Debug.WriteLine("Checking collision");
        }
    }
}
