using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SetupWorkerFlagger
{
    [MenuItem("Tools/Setup Worker Flagger")]
    public static void Apply()
    {
        const string workerPath = "Assets/Models/Worker.fbx";
        const string controllerPath = "Assets/Animations/FlaggerPlaceholder.controller";
        const string scenePath = "Assets/Scenes/Track1.unity";

        AnimationClip[] clips = AssetDatabase
            .LoadAllAssetsAtPath(workerPath)
            .OfType<AnimationClip>()
            .Where(clip => !clip.name.StartsWith("__preview__"))
            .ToArray();

        Debug.Log($"Worker clip count: {clips.Length}");
        foreach (AnimationClip clip in clips)
        {
            Debug.Log($"Worker clip: {clip.name}");
        }

        AnimationClip idleClip = clips.FirstOrDefault(c => c.name == "Idle_Neutral")
            ?? clips.FirstOrDefault(c => c.name == "Idle")
            ?? clips.FirstOrDefault(c => c.name.Contains("Idle"));

        AnimationClip waveClip = clips.FirstOrDefault(c => c.name == "Wave")
            ?? clips.FirstOrDefault(c => c.name.Contains("Wave"));

        if (idleClip == null || waveClip == null)
        {
            Debug.LogError("Could not find required Worker clips (Idle/Idle_Neutral and Wave).");
            return;
        }

        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        if (controller == null)
        {
            Debug.LogError($"Could not load controller at {controllerPath}");
            return;
        }

        AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
        foreach (ChildAnimatorState childState in stateMachine.states)
        {
            if (childState.state.name == "Idle")
            {
                childState.state.motion = idleClip;
            }
            else if (childState.state.name == "Wave")
            {
                childState.state.motion = waveClip;
            }
        }

        EditorUtility.SetDirty(controller);

        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (sceneAsset == null)
        {
            Debug.LogError($"Could not load scene at {scenePath}");
            AssetDatabase.SaveAssets();
            return;
        }

        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        GameObject flagger = GameObject.Find("RoadsideFlagger");
        if (flagger != null)
        {
            Animator animator = flagger.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = controller;
                EditorUtility.SetDirty(animator);
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Debug.Log($"Assigned Worker clips. Idle={idleClip.name}, Wave={waveClip.name}");
    }
}
