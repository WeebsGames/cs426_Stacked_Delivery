using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ListWorkerClips
{
    public static void PrintWorkerClips()
    {
        const string assetPath = "Assets/Models/Worker.fbx";
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        Debug.Log($"Listing sub-assets for {assetPath}");

        foreach (Object asset in assets)
        {
            if (asset == null)
            {
                continue;
            }

            string typeName = asset.GetType().Name;
            Debug.Log($"{typeName}: {asset.name}");
        }

        AnimationClip[] clips = assets.OfType<AnimationClip>().ToArray();
        Debug.Log($"Animation clip count: {clips.Length}");

        foreach (AnimationClip clip in clips)
        {
            Debug.Log($"Clip '{clip.name}' length={clip.length:F3}");
        }
    }
}
