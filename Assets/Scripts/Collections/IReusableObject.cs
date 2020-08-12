namespace Minecraft.Collections
{
    /// <summary>
    /// 可重用对象接口
    /// </summary>
    public interface IReusableObject
    {
        /// <summary>
        /// 回调：对象被分配/重用
        /// </summary>
        void OnAllocated();

        /// <summary>
        /// 回调：对象被释放/回收
        /// </summary>
        /// <param name="destroy">是否销毁对象</param>
        void OnFree(bool destroy);
    }
}