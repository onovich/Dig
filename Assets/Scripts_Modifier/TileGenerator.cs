#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;

namespace Dig.Modifier {

    public class TileGenerator : MonoBehaviour {
        private const string ColorsPath = "Assets/Resources_Runtime/Colors";
        private const string TilesPath = "Assets/Resources_Runtime/Tiles";

        [ContextMenu("Check and Fix Tiles")]
        public void CheckAndFixTiles() {
            var colorFiles = GetFilesWithoutExtension(ColorsPath, "*.png");
            var tileFiles = GetFilesWithoutExtension(TilesPath, "*.asset");

            FixExistingTiles(tileFiles, colorFiles);
            CreateMissingTiles(tileFiles, colorFiles);

            AssetDatabase.Refresh();
        }

        private string[] GetFilesWithoutExtension(string path, string searchPattern) {
            return Directory.GetFiles(path, searchPattern)
                            .Select(Path.GetFileNameWithoutExtension)
                            .ToArray();
        }

        private void FixExistingTiles(string[] tileFiles, string[] colorFiles) {
            foreach (var tileFile in tileFiles) {
                if (colorFiles.Contains(tileFile)) {
                    UpdateTileSprite(tileFile);
                } else {
                    DeleteTile(tileFile);
                }
            }
        }

        private void CreateMissingTiles(string[] tileFiles, string[] colorFiles) {
            foreach (var colorFile in colorFiles) {
                if (!tileFiles.Contains(colorFile)) {
                    CreateTile(colorFile);
                }
            }
        }

        private void UpdateTileSprite(string tileName) {
            string tilePath = Path.Combine(TilesPath, tileName + ".asset");
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
            if (tile != null) {
                string spritePath = Path.Combine(ColorsPath, tileName + ".png");
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (sprite != null && tile.sprite != sprite) {
                    tile.sprite = sprite;
                    EditorUtility.SetDirty(tile);
                    AssetDatabase.SaveAssets();
                    Debug.Log($"Updated tile: {tilePath} with sprite: {spritePath}");
                }
            }
        }

        private void DeleteTile(string tileName) {
            string tilePath = Path.Combine(TilesPath, tileName + ".asset");
            AssetDatabase.DeleteAsset(tilePath);
            Debug.Log($"Deleted tile: {tilePath}");
        }

        private void CreateTile(string colorName) {
            Tile newTile = ScriptableObject.CreateInstance<Tile>();
            string spritePath = Path.Combine(ColorsPath, colorName + ".png");
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sprite != null) {
                newTile.sprite = sprite;
                string newTilePath = Path.Combine(TilesPath, colorName + ".asset");
                AssetDatabase.CreateAsset(newTile, newTilePath);
                Debug.Log($"Created new tile: {newTilePath} with sprite: {spritePath}");
            }
        }
    }

}
#endif