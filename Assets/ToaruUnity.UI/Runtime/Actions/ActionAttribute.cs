using System;

namespace ToaruUnity.UI
{
    /// <summary>
    /// 指示该方法为一个操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class ActionAttribute : Attribute
    {
        /// <summary>
        /// 获取该操作的名称，如果该值为null，则默认使用对应方法的名称
        /// </summary>
        public string ActionName { get; }

        /// <summary>
        /// 指示该方法为一个操作，操作的名称使用对应方法的名称
        /// </summary>
        public ActionAttribute() { }

        /// <summary>
        /// 指示该方法为一个操作
        /// </summary>
        /// <param name="actionName">该操作的名称，如果该值为null，则默认使用对应方法的名称</param>
        public ActionAttribute(string actionName)
        {
            ActionName = actionName;
        }
    }
}