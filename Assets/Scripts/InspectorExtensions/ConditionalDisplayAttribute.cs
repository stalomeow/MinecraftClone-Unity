using System;
using UnityEngine;

namespace Minecraft.InspectorExtensions
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class ConditionalDisplayAttribute : PropertyAttribute
    {
        public string ConditionField { get; }

        private ConditionalDisplayAttribute() { }

        public ConditionalDisplayAttribute(string conditionField)
        {
            ConditionField = conditionField;
        }
    }
}
