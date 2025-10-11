// Assets/Scripts/Editor/SkillEditor.cs
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Models.Scriptables;

[CustomEditor(typeof(Skill))]
public class SkillEditor : Editor
{
    SerializedProperty skillName, description, skillIcon, skillType, manaCost, cooldown,
                       castingTarget, effects, vfxPrefab, sfxClip, castBehavior;

    ReorderableList effectsList;

    const float HeaderH   = 22f;
    const float Padding   = 6f;
    const float Spacing   = 6f;

    void OnEnable()
    {
        skillName     = serializedObject.FindProperty("skillName");
        description   = serializedObject.FindProperty("description");
        skillIcon     = serializedObject.FindProperty("skillIcon");
        skillType     = serializedObject.FindProperty("skillType");
        manaCost      = serializedObject.FindProperty("manaCost");
        cooldown      = serializedObject.FindProperty("cooldown");
        castingTarget = serializedObject.FindProperty("castingTarget");
        effects       = serializedObject.FindProperty("effects");
        vfxPrefab     = serializedObject.FindProperty("vfxPrefab");
        sfxClip       = serializedObject.FindProperty("sfxClip");
        castBehavior  = serializedObject.FindProperty("castBehavior");

        effectsList = new ReorderableList(serializedObject, effects, true, true, true, true);

        effectsList.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Effects");
        };

        effectsList.elementHeightCallback = index =>
        {
            var element = effects.GetArrayElementAtIndex(index);
            var innerHeight = EditorGUI.GetPropertyHeight(element, true); // height from SkillEffectDrawer
            // header + spacing + padding around inner
            return HeaderH + Spacing + innerHeight + Padding * 2f;
        };

        effectsList.drawElementCallback = (rect, index, active, focused) =>
        {
            var element      = effects.GetArrayElementAtIndex(index);
            var effectType   = element.FindPropertyRelative("effectType");
            var targetType   = element.FindPropertyRelative("targetType");

            // Header area
            var headerRect = new Rect(rect.x, rect.y + 2f, rect.width, HeaderH - 2f);
            EditorGUI.LabelField(headerRect,
                $"{(EffectType)effectType.enumValueIndex} → {(SkillTarget)targetType.enumValueIndex}",
                EditorStyles.boldLabel);

            // Content area (drawer)
            var contentY   = headerRect.yMax + Spacing;
            var contentH   = EditorGUI.GetPropertyHeight(element, true);
            var contentRect = new Rect(rect.x + Padding, contentY, rect.width - Padding * 2f, contentH);

            EditorGUI.PropertyField(contentRect, element, GUIContent.none, true);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Identity", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(skillName);
        EditorGUILayout.PropertyField(description);
        EditorGUILayout.PropertyField(skillIcon);
        EditorGUILayout.PropertyField(skillType);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Cost & Cooldown", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(manaCost);
        EditorGUILayout.PropertyField(cooldown);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Casting", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(castingTarget, new GUIContent("Casting Target"));

        EditorGUILayout.Space(6);
        effectsList.DoLayoutList();

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("VFX / SFX", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(vfxPrefab);
        EditorGUILayout.PropertyField(sfxClip);

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Casting Logic", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(castBehavior);

        serializedObject.ApplyModifiedProperties();
    }
}
