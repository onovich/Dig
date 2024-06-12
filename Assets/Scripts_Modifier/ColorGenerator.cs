using System.IO;
using UnityEngine;

public class ColorGenerator : MonoBehaviour {
    [ContextMenu("Generate Colors")]
    private void Gen() {
        GenerateColors();
    }

    private void GenerateColors() {
        int step = 256 / 4; // 4 steps for 64 colors

        for (int r = 0; r < 256; r += step) {
            for (int g = 0; g < 256; g += step) {
                for (int b = 0; b < 256; b += step) {
                    Color color = new Color(r / 255f, g / 255f, b / 255f);
                    CreateColorTexture(color, r, g, b);
                }
            }
        }
    }

    private void CreateColorTexture(Color color, int r, int g, int b) {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        string directoryPath = Application.dataPath + "/Resources_Runtime/Colors";
        if (!Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, $"{r:D3}{g:D3}{b:D3}.png");
        File.WriteAllBytes(filePath, bytes);

#if UNITY_EDITOR
        DestroyImmediate(texture);
#else
        Destroy(texture);
#endif
    }
}