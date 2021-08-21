using Minecraft.InspectorExtensions;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor.InspectorExtensions
{
    [CustomPropertyDrawer(typeof(ConditionalDisplayAttribute))]
    public class ConditionalDisplayAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldDraw(property) ? EditorGUI.GetPropertyHeight(property, label) : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldDraw(property))
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        private bool ShouldDraw(SerializedProperty property)
        {
            string path = (attribute as ConditionalDisplayAttribute).ConditionField;
            int dotIndex = property.propertyPath.LastIndexOf('.');

            if (dotIndex != -1)
            {
                path = property.propertyPath.Substring(0, dotIndex + 1) + path;
            }

            SerializedProperty prop = property.serializedObject.FindProperty(path);
            return prop.boolValue;
        }
    }
}
