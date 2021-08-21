using Minecraft.InspectorExtensions;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor.InspectorExtensions
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;
            label = EditorGUI.BeginProperty(position, label, property);

            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            position.height -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;
            position = EditorGUI.IndentedRect(position);
            EditorGUI.indentLevel--;

            Rect minRect = new Rect(position.x, position.y, 60, position.height);
            Rect sliderRect = new Rect(minRect.xMax, position.y, position.width - minRect.width * 2, position.height);
            Rect maxRect = new Rect(sliderRect.xMax, position.y, minRect.width, position.height);

            EditorGUI.BeginChangeCheck();

            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    Vector2 vec2 = property.vector2Value;
                    vec2.x = EditorGUI.FloatField(minRect, vec2.x);
                    EditorGUI.MinMaxSlider(sliderRect, ref vec2.x, ref vec2.y, attr.Min, attr.Max);
                    vec2.y = EditorGUI.FloatField(maxRect, vec2.y);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.vector2Value = vec2;
                    }
                    break;
                case SerializedPropertyType.Vector2Int:
                    Vector2 veci2 = property.vector2IntValue;
                    veci2.x = EditorGUI.IntField(minRect, (int)veci2.x);
                    EditorGUI.MinMaxSlider(sliderRect, ref veci2.x, ref veci2.y, (int)attr.Min, (int)attr.Max);
                    veci2.y = EditorGUI.IntField(maxRect, (int)veci2.y);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.vector2IntValue = new Vector2Int((int)veci2.x, (int)veci2.y);
                    }
                    break;
                default:
                    Color color = GUI.contentColor;
                    GUI.contentColor = Color.red;
                    EditorGUI.LabelField(position, "Unsupported field type.");
                    GUI.contentColor = color;
                    EditorGUI.EndChangeCheck();
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}
