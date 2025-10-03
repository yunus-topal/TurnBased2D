// using UnityEditor;
// using UnityEngine;
//
// namespace Editor
// {
//     [CustomPropertyDrawer(typeof(Models.Scriptables.SkillEffect))]
//     public class SkillEffectDrawer : PropertyDrawer
//     {
//         const float lineHeight = 18f;
//         const float verticalSpacing = 4f;
//
//         public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
//         {
//             float height = lineHeight + 2 * verticalSpacing; // effectType
//
//             var effectTypeProp = prop.FindPropertyRelative("effectType");
//             var effectType = (Models.Scriptables.EffectType)effectTypeProp.enumValueIndex;
//
//             switch (effectType)
//             {
//                 case Models.Scriptables.EffectType.Damage:
//                     height += (lineHeight + verticalSpacing) * 2; // magnitude
//                     break;
//                 case Models.Scriptables.EffectType.Heal:
//                     height += (lineHeight + verticalSpacing) * 2; // magnitude
//                     break;
//                 case Models.Scriptables.EffectType.Status:
//                     height += (lineHeight + verticalSpacing) * 3; // statusEffect + duration
//                     break;
//             }
//
//             return height;
//         }
//
//         public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
//         {
//             var effectTypeProp = prop.FindPropertyRelative("effectType");
//             var magnitudeProp  = prop.FindPropertyRelative("magnitude");
//             var statusProp     = prop.FindPropertyRelative("statusEffect");
//             var durationProp   = prop.FindPropertyRelative("durationInTurns");
//
//             // Draw effectType
//             rect.height = lineHeight;
//             EditorGUI.BeginProperty(rect, label, prop);
//             rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);
//             EditorGUI.PropertyField(rect, effectTypeProp, GUIContent.none);
//
//             // Next line
//             rect.y += 2 * lineHeight + 3 * verticalSpacing;
//             var effectType = (Models.Scriptables.EffectType)effectTypeProp.enumValueIndex;
//             
//             switch (effectType)
//             {
//                 case Models.Scriptables.EffectType.Damage:
//                 case Models.Scriptables.EffectType.Heal:
//                     // single-line int field for magnitude
//                     EditorGUI.BeginChangeCheck();
//                     int newMag = EditorGUI.IntField(rect, new GUIContent("Magnitude"), magnitudeProp.intValue);
//                     if (EditorGUI.EndChangeCheck())
//                         magnitudeProp.intValue = Mathf.Max(0, newMag);
//                     break;
//             
//                 case Models.Scriptables.EffectType.Status:
//                     // statusEffect enum
//                     EditorGUI.PropertyField(rect, statusProp);
//                     rect.y += lineHeight + verticalSpacing;
//                     // single-line int field for durationInTurns
//                     EditorGUI.BeginChangeCheck();
//                     int newDur = EditorGUI.IntField(rect, new GUIContent("Duration (turns)"), durationProp.intValue);
//                     if (EditorGUI.EndChangeCheck())
//                         durationProp.intValue = Mathf.Max(0, newDur);
//                     break;
//             }
//
//             EditorGUI.EndProperty();
//         }
//     }
// }
