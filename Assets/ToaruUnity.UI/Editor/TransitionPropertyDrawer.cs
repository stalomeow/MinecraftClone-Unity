using System;
using UnityEditor;
using UnityEngine;
using static ToaruUnity.UI.TweenUGUIView;

namespace ToaruUnityEditor.UI
{
    [CustomPropertyDrawer(typeof(Transition))]
    internal sealed class TransitionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty enabledProperty = property.FindPropertyRelative("Enabled");
            SerializedProperty modeProperty = property.FindPropertyRelative("Mode");

            Rect rect = GetLineRect(ref position);
            GUIContent enableContent = new GUIContent("Enable Transition", enabledProperty.tooltip);
            enabledProperty.boolValue = EditorGUI.ToggleLeft(rect, enableContent, enabledProperty.boolValue, EditorStyles.boldLabel);

            using (new EditorGUI.DisabledGroupScope(!enabledProperty.boolValue))
            {
                EditorGUI.PropertyField(GetLineRect(ref position), property.FindPropertyRelative("Type"));
                EditorGUI.PropertyField(GetLineRect(ref position), modeProperty);

                switch ((TransitionMode)modeProperty.intValue)
                {
                    case TransitionMode.Position:
                        {
                            SerializedProperty transform = property.FindPropertyRelative("TargetTransform");
                            SerializedProperty from = property.FindPropertyRelative("FromPosition");
                            SerializedProperty to = property.FindPropertyRelative("ToPosition");
                            SerializedProperty curve = property.FindPropertyRelative("Curve");
                            SerializedProperty duration = property.FindPropertyRelative("Duration");

                            EditorGUI.PropertyField(GetLineRect(ref position), transform);
                            EditorGUI.PropertyField(GetLineRect(ref position), curve);
                            EditorGUI.PropertyField(GetLineRect(ref position), duration);
                            EditorGUI.PropertyField(GetLineRect(ref position, SerializedPropertyType.Vector2), from);
                            EditorGUI.PropertyField(GetLineRect(ref position, SerializedPropertyType.Vector2), to);
                        }
                        break;
                    case TransitionMode.Rotation:
                        {
                            SerializedProperty transform = property.FindPropertyRelative("TargetTransform");
                            SerializedProperty from = property.FindPropertyRelative("FromRotation");
                            SerializedProperty to = property.FindPropertyRelative("ToRotation");
                            SerializedProperty curve = property.FindPropertyRelative("Curve");
                            SerializedProperty duration = property.FindPropertyRelative("Duration");

                            EditorGUI.PropertyField(GetLineRect(ref position), transform);
                            EditorGUI.PropertyField(GetLineRect(ref position), curve);
                            EditorGUI.PropertyField(GetLineRect(ref position), duration);
                            EditorGUI.PropertyField(GetLineRect(ref position, SerializedPropertyType.Vector3), from);
                            EditorGUI.PropertyField(GetLineRect(ref position, SerializedPropertyType.Vector3), to);
                        }
                        break;
                    case TransitionMode.Scale:
                        {
                            SerializedProperty transform = property.FindPropertyRelative("TargetTransform");
                            SerializedProperty from = property.FindPropertyRelative("FromScale");
                            SerializedProperty to = property.FindPropertyRelative("ToScale");
                            SerializedProperty curve = property.FindPropertyRelative("Curve");
                            SerializedProperty duration = property.FindPropertyRelative("Duration");

                            EditorGUI.PropertyField(GetLineRect(ref position), transform);
                            EditorGUI.PropertyField(GetLineRect(ref position), curve);
                            EditorGUI.PropertyField(GetLineRect(ref position), duration);
                            EditorGUI.PropertyField(GetLineRect(ref position, SerializedPropertyType.Vector3), from);
                            EditorGUI.PropertyField(GetLineRect(ref position, SerializedPropertyType.Vector3), to);
                        }
                        break;
                    case TransitionMode.Color:
                        {
                            SerializedProperty graphic = property.FindPropertyRelative("TargetGraphic");
                            SerializedProperty from = property.FindPropertyRelative("FromColor");
                            SerializedProperty to = property.FindPropertyRelative("ToColor");
                            SerializedProperty curve = property.FindPropertyRelative("Curve");
                            SerializedProperty duration = property.FindPropertyRelative("Duration");

                            EditorGUI.PropertyField(GetLineRect(ref position), graphic);
                            EditorGUI.PropertyField(GetLineRect(ref position), curve);
                            EditorGUI.PropertyField(GetLineRect(ref position), duration);
                            EditorGUI.PropertyField(GetLineRect(ref position), from);
                            EditorGUI.PropertyField(GetLineRect(ref position), to);
                        }
                        break;
                    case TransitionMode.Alpha:
                        {
                            SerializedProperty from = property.FindPropertyRelative("FromAlpha");
                            SerializedProperty to = property.FindPropertyRelative("ToAlpha");
                            SerializedProperty curve = property.FindPropertyRelative("Curve");
                            SerializedProperty duration = property.FindPropertyRelative("Duration");

                            EditorGUI.PropertyField(GetLineRect(ref position), curve);
                            EditorGUI.PropertyField(GetLineRect(ref position), duration);
                            EditorGUI.PropertyField(GetLineRect(ref position), from);
                            EditorGUI.PropertyField(GetLineRect(ref position), to);
                        }
                        break;
                    case TransitionMode.Animation:
                        {
                            SerializedProperty animator = property.FindPropertyRelative("TargetAnimator");
                            SerializedProperty name = property.FindPropertyRelative("AnimationName");

                            EditorGUI.PropertyField(GetLineRect(ref position), animator);
                            EditorGUI.PropertyField(GetLineRect(ref position), name);
                        }
                        break;
                }
            }
        }

        private Rect GetLineRect(ref Rect position, int lineCount = 1)
        {
            Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight * lineCount);
            position = new Rect(rect.x, rect.yMax, rect.width, rect.height);
            return rect;
        }

        private Rect GetLineRect(ref Rect position, SerializedPropertyType type)
        {
            Rect rect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(type, null));
            position = new Rect(rect.x, rect.yMax, rect.width, rect.height);
            return rect;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty modeProperty = property.FindPropertyRelative("Mode");
            TransitionMode mode = (TransitionMode)Enum.Parse(typeof(TransitionMode), modeProperty.enumNames[modeProperty.enumValueIndex], true);

            float line = EditorGUIUtility.singleLineHeight;
            float baseHeight = line * 3;
            
            switch (mode)
            {
                case TransitionMode.Position:
                    return baseHeight + line * 3 + EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, null) * 2;

                case TransitionMode.Rotation:
                case TransitionMode.Scale:
                    return baseHeight + line * 3 + EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, null) * 2;

                case TransitionMode.Color:
                    return baseHeight + line * 5;

                case TransitionMode.Alpha:
                    return baseHeight + line * 4;

                case TransitionMode.Animation:
                    return baseHeight + line * 2;

                default:
                    return baseHeight;
            }
        }
    }
}