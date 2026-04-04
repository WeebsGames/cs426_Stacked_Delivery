using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ExtractWorkerAnimations
{
    [MenuItem("Tools/Extract Worker Animations")]
    public static void Extract()
    {
        const string workerPath = "Assets/Models/Worker.fbx";
        const string outputFolder = "Assets/Animations";

        if (!AssetDatabase.IsValidFolder(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
            AssetDatabase.Refresh();
        }

        AnimationClip[] clips = AssetDatabase
            .LoadAllAssetsAtPath(workerPath)
            .OfType<AnimationClip>()
            .Where(clip => !clip.name.StartsWith("__preview__"))
            .ToArray();

        if (clips.Length == 0)
        {
            Debug.LogError("No animation clips were found in Assets/Models/Worker.fbx");
            return;
        }

        foreach (AnimationClip clip in clips)
        {
            string safeName = clip.name.Replace("|", "_");
            string assetPath = $"{outputFolder}/{safeName}.anim";

            AnimationClip clipCopy = Object.Instantiate(clip);
            AssetDatabase.CreateAsset(clipCopy, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Extracted {clips.Length} Worker clips to {outputFolder}");
    }
}
