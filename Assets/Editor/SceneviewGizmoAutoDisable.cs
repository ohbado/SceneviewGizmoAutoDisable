using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンビューで右クリック、ホイールボタンを押している間のみTransformのGizmoを無効にする
/// </summary>
[InitializeOnLoad]
public class SceneviewGizmoAutoDisable
{
    private static GameObject anyObject = null;

    static SceneviewGizmoAutoDisable()
    {
        SceneView.beforeSceneGui -= OnSceneGui;
        SceneView.beforeSceneGui += OnSceneGui;
    }

    private static void OnSceneGui(SceneView view)
    {
        Event e = Event.current;
        if (e.isMouse && (e.button == 1 || e.button == 2))
        {
            if (e.type == EventType.MouseDown)
            {
                if (anyObject == null)
                {
                    anyObject = GetAnyGameObject();
                }
                SetTransformInspectorExpand(anyObject, false);
            }
            else if (e.type == EventType.MouseUp)
            {
                if (anyObject == null)
                {
                    anyObject = GetAnyGameObject();
                }
                SetTransformInspectorExpand(anyObject, true);
            }
        }
    }

    private static void SetTransformInspectorExpand(GameObject go, bool expanded)
    {
        if (go == null)
        {
            return;
        }
        InternalEditorUtility.SetIsInspectorExpanded(go.transform, expanded);
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    private static GameObject GetAnyGameObject()
    {
        if (Selection.activeGameObject != null)
        {
            return Selection.activeGameObject;
        }
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.IsValid())
            {
                foreach (var obj in scene.GetRootGameObjects())
                {
                    return obj;
                }
            }
        }
        return null;
    }
}
