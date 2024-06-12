using System.Collections.Generic;
using UnityEngine;

namespace Dig {

    public static class FunctionHelper {

        public static void GenMaze(MazeGenerateType genType, int width, int height, bool isBoundaryWall, int[,] maze) {
            if (genType == MazeGenerateType.DFS) {
                GenerateMazeDFS(width, height, isBoundaryWall, maze);
            }
        }

        public static int[,] GenerateMazeDFS(int width, int height, bool isBoundaryWall, int[,] maze) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    maze[i, j] = isBoundaryWall ? 1 : 0;
                }
            }

            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            Vector2Int start = new Vector2Int(1, 1);
            maze[start.x, start.y] = 0;
            stack.Push(start);

            int[,] directions = { { 0, 2 }, { 2, 0 }, { 0, -2 }, { -2, 0 } };

            while (stack.Count > 0) {
                Vector2Int current = stack.Pop();
                List<Vector2Int> neighbors = new List<Vector2Int>();

                for (int i = 0; i < directions.GetLength(0); i++) {
                    Vector2Int neighbor = new Vector2Int(current.x + directions[i, 0], current.y + directions[i, 1]);
                    if (IsInBounds(neighbor, width, height) && maze[neighbor.x, neighbor.y] == (isBoundaryWall ? 1 : 0)) {
                        neighbors.Add(neighbor);
                    }
                }

                if (neighbors.Count > 0) {
                    stack.Push(current);
                    Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                    maze[(current.x + chosen.x) / 2, (current.y + chosen.y) / 2] = isBoundaryWall ? 0 : 1;
                    maze[chosen.x, chosen.y] = isBoundaryWall ? 0 : 1;
                    stack.Push(chosen);
                }
            }
            return maze;
        }

        static bool IsInBounds(Vector2Int position, int width, int height) {
            return position.x > 0 && position.x < width - 1 && position.y > 0 && position.y < height - 1;
        }

    }

}