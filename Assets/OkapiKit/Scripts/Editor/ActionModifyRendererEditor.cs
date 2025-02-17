using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActionModifyRenderer))]
public class ActionModifyRendererEditor : ActionEditor
{
    SerializedProperty propRenderer;
    SerializedProperty propChangeType;
    SerializedProperty propVisibility;

    protected override void OnEnable()
    {
        base.OnEnable();

        propRenderer = serializedObject.FindProperty("renderer");
        propChangeType = serializedObject.FindProperty("changeType");
        propVisibility = serializedObject.FindProperty("visibility");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (WriteTitle())
        {
            StdEditor(false);

            var action = (target as ActionModifyRenderer);
            if (action == null) return;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(propRenderer, new GUIContent("Renderer"));
            EditorGUILayout.PropertyField(propChangeType, new GUIContent("Change Type"));

            if (propChangeType.enumValueIndex == (int)ActionModifyRenderer.ChangeType.Visibility)
            {
                EditorGUILayout.PropertyField(propVisibility, new GUIContent("Visibility"));
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                (target as Action).UpdateExplanation();
            }
        }
    }
}
