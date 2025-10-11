// Assets/Scripts/Editor/SkillEffectDrawer.cs
using UnityEditor;
using UnityEngine;
using Models.Scriptables;

[CustomPropertyDrawer(typeof(SkillEffect))]
public class SkillEffectDrawer : PropertyDrawer
{
    const float VSpace = 4f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var etProp = property.FindPropertyRelative("effectType");
        var et = (EffectType)etProp.enumValueIndex;

        int lines = 2; // Effect Type + Target
        lines += (et == EffectType.Status) ? 3 : 2; // status: statusEffect + 2 durations, else: 2 magnitudes

        return lines * (EditorGUIUtility.singleLineHeight + VSpace) - VSpace;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float y = position.y;
        float h = EditorGUIUtility.singleLineHeight;

        Rect Row()
        {
            var r = new Rect(position.x, y, position.width, h);
            y += h + VSpace;
            return r;
        }

        // Grab props
        var effectTypeProp   = property.FindPropertyRelative("effectType");
        var targetTypeProp   = property.FindPropertyRelative("targetType");
        var magnitudeProp    = property.FindPropertyRelative("magnitude");
        var magnitudeUpProp  = property.FindPropertyRelative("magnitudeUpgraded");
        var statusEffectProp = property.FindPropertyRelative("statusEffect");
        var durProp          = property.FindPropertyRelative("durationInTurns");
        var durUpProp        = property.FindPropertyRelative("durationInTurnsUpgraded");

        // Tidy labels
        float oldLW = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 140f;

        // ---- Effect Type FIRST (explicit dropdown) ----
        {
            var et = (EffectType)effectTypeProp.enumValueIndex;
            var newEt = (EffectType)EditorGUI.EnumPopup(Row(), "Effect Type", et);
            if (newEt != et) effectTypeProp.enumValueIndex = (int)newEt;
        }

        // ---- Target dropdown (explicit) ----
        {
            var t = targetTypeProp.enumDisplayNames.Length > 0
                ? (SkillTarget)targetTypeProp.enumValueIndex
                : 0;
            var newT = (SkillTarget)EditorGUI.EnumPopup(Row(), "Target", t);
            if ((int)newT != targetTypeProp.enumValueIndex) targetTypeProp.enumValueIndex = (int)newT;
        }

        // ---- Conditional block ----
        if ((EffectType)effectTypeProp.enumValueIndex == EffectType.Status)
        {
            // statusEffect: keep PropertyField (usually no headers on this one)
            EditorGUI.PropertyField(Row(), statusEffectProp, new GUIContent("Status Effect"), false);

            DrawNumberField(Row(), durProp,   "Duration (turns)");
            DrawNumberField(Row(), durUpProp, "Duration Upgraded (turns)");
        }
        else
        {
            DrawNumberField(Row(), magnitudeProp,   "Magnitude");
            DrawNumberField(Row(), magnitudeUpProp, "Magnitude Upgraded");
        }

        EditorGUIUtility.labelWidth = oldLW;
        EditorGUI.EndProperty();
    }

    static void DrawNumberField(Rect r, SerializedProperty p, string label)
    {
        EditorGUI.BeginChangeCheck();

        // Draw an int field even if it's a float property, to ensure integers only
        int value = (p.propertyType == SerializedPropertyType.Float)
            ? Mathf.RoundToInt(p.floatValue)
            : p.intValue;

        value = EditorGUI.DelayedIntField(r, label, Mathf.Max(0, value));

        if (EditorGUI.EndChangeCheck())
        {
            if (p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
            else
                p.intValue = value;
        }
    }

}
