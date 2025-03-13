using System.IO;
using UnityEditor;
using UnityEngine;

public class SceneViewSnapshot
{
    [MenuItem("Tools/Capture Scene View")]
    static void CaptureScreenViewMenuItem()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;

        if (sceneView == null || sceneView.camera == null)
        {
            Debug.LogError("No active SceneView camera found!");
            return;
        }

        int width = sceneView.camera.pixelWidth;
        int height = sceneView.camera.pixelHeight;

        Texture2D capture = new Texture2D(width, height);
        sceneView.camera.Render();
        RenderTexture.active = sceneView.camera.targetTexture;
        capture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        capture.Apply();

        string folderPath = Path.Combine(Application.dataPath, "image");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        int index = 1;
        string filename;
        do
        {
            filename = $"image_{index}.png";
            index++;
        } while (File.Exists(Path.Combine(folderPath, filename)));

        string filePath = Path.Combine(folderPath, filename);
        File.WriteAllBytes(filePath, capture.EncodeToPNG());

        Debug.Log($"Saved snapshot to: {filePath}");

        AssetDatabase.Refresh();
    }
}
