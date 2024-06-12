using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dig {

    public class MazeGenerator : MonoBehaviour {

        public TerrainTM terrainTM;
        public MazeGenerateType genType;

        public int width = 20;
        public int height = 20;
        public float scale = 1;

        public Tilemap tilemap;
        public GameObject map;
        int[,] maze;

        void Start() {
            maze = new int[width, height];
            Gen();
        }

        void Gen() {
            FunctionHelper.GenMaze(genType, width, height, maze);
            RenderMaze();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Gen();
            }
        }

        void RenderMaze() {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (maze[x, y] == 0) {
                        tilemap.SetTile(new Vector3Int(x, y, 0), terrainTM.tile_ground);
                    } else {
                        tilemap.SetTile(new Vector3Int(x, y, 0), terrainTM.tile_wall);
                    }
                }
            }
            var offset = new Vector3((float)width / 2 * scale, (float)height / 2 * scale, 0);
            map.transform.position = -offset;
            map.transform.localScale = new Vector3(scale, scale, 1);
        }

    }

}