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
        Tile[,] maze;

        void Start() {
            GenerateMazeDFS();
            RenderMaze();
        }

        void GenerateMazeDFS() {
            maze = new Tile[width, height];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    maze[x, y] = terrainTM.tile_wall;
                }
            }

            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            Vector2Int start = new Vector2Int(1, 1);
            maze[start.x, start.y] = terrainTM.tile_ground;
            stack.Push(start);

            int[,] directions = { { 0, 2 }, { 2, 0 }, { 0, -2 }, { -2, 0 } };

            while (stack.Count > 0) {
                Vector2Int current = stack.Pop();
                List<Vector2Int> neighbors = new List<Vector2Int>();

                for (int i = 0; i < directions.GetLength(0); i++) {
                    Vector2Int neighbor = new Vector2Int(current.x + directions[i, 0], current.y + directions[i, 1]);
                    if (IsInBounds(neighbor) && maze[neighbor.x, neighbor.y] == terrainTM.tile_wall) {
                        neighbors.Add(neighbor);
                    }
                }

                if (neighbors.Count > 0) {
                    stack.Push(current);
                    Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                    maze[(current.x + chosen.x) / 2, (current.y + chosen.y) / 2] = terrainTM.tile_ground;
                    maze[chosen.x, chosen.y] = terrainTM.tile_ground;
                    stack.Push(chosen);
                }
            }
        }

        void RenderMaze() {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (maze[x, y] == terrainTM.tile_ground) {
                        tilemap.SetTile(new Vector3Int(x, y, 0), terrainTM.tile_ground);
                    } else {
                        tilemap.SetTile(new Vector3Int(x, y, 0), terrainTM.tile_wall);
                    }
                }
            }
            var offset = new Vector3(width / 2 * scale, height / 2 * scale, 0);
            map.transform.position = -offset;
            map.transform.localScale = new Vector3(scale, scale, 1);
        }

        bool IsInBounds(Vector2Int position) {
            return position.x > 0 && position.x < width && position.y > 0 && position.y < height;
        }

    }

}