using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.AudioManager.Editor.Carter_Games.Audio_Manager.Editor.Property_Drawers
{
    [CustomPropertyDrawer(typeof(MinMaxFloat))]
    public class MinMaxFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            var _elementOne = property.FindPropertyRelative("min");
            var _elementTwo = property.FindPropertyRelative("max");
            
            EditorGUI.BeginChangeCheck();

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var _left = new Rect(position.x, position.y, (position.width / 2) - 1.5f, EditorGUIUtility.singleLineHeight);
            var _right = new Rect(position.x + (position.width / 2) + 1.5f, position.y, (position.width / 2) - 1.5f, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(_left, _elementOne, GUIContent.none);
            EditorGUI.PropertyField(_right, _elementTwo, GUIContent.none);
   

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();            
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}