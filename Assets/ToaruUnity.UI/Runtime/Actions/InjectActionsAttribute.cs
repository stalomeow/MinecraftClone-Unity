using System;

namespace ToaruUnity.UI
{
    /// <summary>
    /// 指示为类型注入<see cref="ActionCenter"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class InjectActionsAttribute : Attribute
    {
        /// <summary>
        /// 获取注入的<see cref="ActionCenter"/>的类型
        /// </summary>
        public Type ActionCenterType { get; }

        /// <summary>
        /// 指示为类型注入<see cref="ActionCenter"/>
        /// </summary>
        /// <param name="actionCenterType">注入的<see cref="ActionCenter"/>的类型</param>
        public InjectActionsAttribute(Type actionCenterType)
        {
            ActionCenterType = actionCenterType;
        }
    }
}
