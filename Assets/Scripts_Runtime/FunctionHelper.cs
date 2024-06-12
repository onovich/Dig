using System.Collections.Generic;
using UnityEngine;

namespace Dig {

    public static class FunctionHelper {

        public static void GenMaze(MazeGenerateType genType, int width, int height, int[,] maze) {
            switch (genType) {
                case MazeGenerateType.DFS:
                    GenerateMazeDFS(width, height, maze);
                    break;
                case MazeGenerateType.BinaryTree:
                    GenerateMazeBinaryTree(width, height, maze);
                    break;
                case MazeGenerateType.HuntAndKill:
                    GenerateMazeHuntAndKill(width, height, maze);
                    break;
            }
        }

        public static int[,] GenerateMazeDFS(int width, int height, int[,] maze) {
            InitializeMazeWithWalls(width, height, maze);

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
                    if (IsInBounds(neighbor, width, height) && maze[neighbor.x, neighbor.y] == 1) {
                        neighbors.Add(neighbor);
                    }
                }

                if (neighbors.Count > 0) {
                    stack.Push(current);
                    Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                    maze[(current.x + chosen.x) / 2, (current.y + chosen.y) / 2] = 0;
                    maze[chosen.x, chosen.y] = 0;
                    stack.Push(chosen);
                }
            }
            return maze;
        }

        static void RecursiveDivision(int[,] maze, int x, int y, int width, int height) {
            if (width <= 2 || height <= 2) return;

            bool horizontal = width < height;

            int wx = x + (horizontal ? 0 : Random.Range(1, width / 2) * 2);
            int wy = y + (horizontal ? Random.Range(1, height / 2) * 2 : 0);

            int px = wx + (horizontal ? Random.Range(0, width) : 0);
            int py = wy + (horizontal ? 0 : Random.Range(0, height));

            int dx = horizontal ? 1 : 0;
            int dy = horizontal ? 0 : 1;

            int length = horizontal ? width : height;
            int direction = horizontal ? 1 : 0;

            for (int i = 0; i < length; i++) {
                if (wx != px || wy != py) {
                    maze[wx, wy] = 1;
                }
                wx += dx;
                wy += dy;
            }

            int nx, ny, w, h;
            nx = x;
            ny = y;
            w = horizontal ? width : wx - x + 1;
            h = horizontal ? wy - y + 1 : height;
            RecursiveDivision(maze, nx, ny, w, h);

            nx = horizontal ? x : wx + 1;
            ny = horizontal ? wy + 1 : y;
            w = horizontal ? width : width - (wx - x + 1);
            h = horizontal ? height - (wy - y + 1) : height;
            RecursiveDivision(maze, nx, ny, w, h);
        }

        public static int[,] GenerateMazeBinaryTree(int width, int height, int[,] maze) {
            InitializeMazeWithWalls(width, height, maze);

            for (int y = 1; y < height; y += 2) {
                for (int x = 1; x < width; x += 2) {
                    maze[x, y] = 0;
                    if (x == width - 2) {
                        if (y < height - 2) {
                            maze[x, y + 1] = 0;
                        }
                    } else if (y == height - 2) {
                        maze[x + 1, y] = 0;
                    } else {
                        if (Random.value > 0.5f) {
                            maze[x + 1, y] = 0;
                        } else {
                            maze[x, y + 1] = 0;
                        }
                    }
                }
            }
            return maze;
        }

        public static int[,] GenerateMazeHuntAndKill(int width, int height, int[,] maze) {
            InitializeMazeWithWalls(width, height, maze);

            Vector2Int current = new Vector2Int(1, 1);
            maze[current.x, current.y] = 0;

            while (true) {
                List<Vector2Int> neighbors = new List<Vector2Int>();

                if (IsInBounds(new Vector2Int(current.x - 2, current.y), width, height) && maze[current.x - 2, current.y] == 1) {
                    neighbors.Add(new Vector2Int(current.x - 2, current.y));
                }
                if (IsInBounds(new Vector2Int(current.x + 2, current.y), width, height) && maze[current.x + 2, current.y] == 1) {
                    neighbors.Add(new Vector2Int(current.x + 2, current.y));
                }
                if (IsInBounds(new Vector2Int(current.x, current.y - 2), width, height) && maze[current.x, current.y - 2] == 1) {
                    neighbors.Add(new Vector2Int(current.x, current.y - 2));
                }
                if (IsInBounds(new Vector2Int(current.x, current.y + 2), width, height) && maze[current.x, current.y + 2] == 1) {
                    neighbors.Add(new Vector2Int(current.x, current.y + 2));
                }

                if (neighbors.Count > 0) {
                    Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                    maze[(current.x + chosen.x) / 2, (current.y + chosen.y) / 2] = 0;
                    maze[chosen.x, chosen.y] = 0;
                    current = chosen;
                } else {
                    bool found = false;
                    for (int x = 1; x < width; x += 2) {
                        for (int y = 1; y < height; y += 2) {
                            if (maze[x, y] == 1) {
                                List<Vector2Int> adjacent = new List<Vector2Int>();

                                if (IsInBounds(new Vector2Int(x - 2, y), width, height) && maze[x - 2, y] == 0) {
                                    adjacent.Add(new Vector2Int(x - 2, y));
                                }
                                if (IsInBounds(new Vector2Int(x + 2, y), width, height) && maze[x + 2, y] == 0) {
                                    adjacent.Add(new Vector2Int(x + 2, y));
                                }
                                if (IsInBounds(new Vector2Int(x, y - 2), width, height) && maze[x, y - 2] == 0) {
                                    adjacent.Add(new Vector2Int(x, y - 2));
                                }
                                if (IsInBounds(new Vector2Int(x, y + 2), width, height) && maze[x, y + 2] == 0) {
                                    adjacent.Add(new Vector2Int(x, y + 2));
                                }

                                if (adjacent.Count > 0) {
                                    Vector2Int chosen = adjacent[Random.Range(0, adjacent.Count)];
                                    maze[(x + chosen.x) / 2, (y + chosen.y) / 2] = 0;
                                    maze[x, y] = 0;
                                    current = new Vector2Int(x, y);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found) break;
                    }
                    if (!found) break;
                }
            }
            return maze;
        }

        static void InitializeMazeWithWalls(int width, int height, int[,] maze) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    maze[i, j] = 1;
                }
            }
        }

        static void InitializeMazeWithPaths(int width, int height, int[,] maze) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    maze[i, j] = 0;
                }
            }
        }

        static bool IsInBounds(Vector2Int position, int width, int height) {
            return position.x > 0 && position.x < width - 1 && position.y > 0 && position.y < height - 1;
        }

        static void Shuffle<T>(T[] array) {
            for (int i = array.Length - 1; i > 0; i--) {
                int j = Random.Range(0, i + 1);
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

    }

}