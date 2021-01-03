using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace ToaruUnity.UI
{
    /// <summary>
    /// 表示一个页面的LIFO集合，能自动管理各个页面的状态。
    /// </summary>
    public class ViewStack : IEnumerable<AbstractView>
    {
        private readonly struct Element
        {
            public readonly object ViewKey;
            public readonly AbstractView View;

            public Element(object viewKey, AbstractView view)
            {
                ViewKey = viewKey;
                View = view;
            }
        }

        public struct Enumerator : IEnumerator<AbstractView>
        {
            private readonly ViewStack m_Stack;
            private readonly int m_Version;
            private int m_Index; // -1 = not started, -2 = ended/disposed
            private AbstractView m_Current;

            public AbstractView Current
            {
                get
                {
                    switch (m_Index)
                    {
                        case -1:
                            throw new InvalidOperationException("迭代没有开始");
                        case -2:
                            throw new InvalidOperationException("迭代已经结束");
                        default:
                            return m_Current;
                    }
                }
            }

            internal Enumerator(ViewStack stack)
            {
                m_Stack = stack;
                m_Version = stack.m_Version;
                m_Index = -1;
                m_Current = default;
            }

            public void Dispose()
            {
                m_Index = -2;
                m_Current = default;
            }

            public bool MoveNext()
            {
                if (m_Version != m_Stack.m_Version)
                    throw new InvalidOperationException("迭代时修改集合");

                if (m_Index == -2)
                    return false;

                m_Index++;

                if (m_Index == m_Stack.Count)
                {
                    m_Index = -2;
                    m_Current = default;
                    return false;
                }

                m_Current = m_Stack[m_Index];
                return true;
            }

            public void Reset()
            {
                if (m_Version != m_Stack.m_Version)
                    throw new InvalidOperationException("迭代时修改集合");

                m_Index = -1;
                m_Current = default;
            }

            object IEnumerator.Current => Current;
        }


        private int m_Version;
        private int m_TopIndex;
        private Element[] m_Stack;
        private readonly int m_MinGrow;
        private readonly int m_MaxGrow;
        private readonly IEqualityComparer<object> m_KeyComparer;

        /// <summary>
        /// 获取元素的数量
        /// </summary>
        public int Count => m_TopIndex + 1;

        /// <summary>
        /// 获取指定索引处的元素
        /// </summary>
        /// <param name="index">元素的索引（栈顶元素的索引为0，向下依次以1递增）</param>
        /// <returns>如果索引位置有元素，则返回该元素；否则返回null</returns>
        public AbstractView this[int index] => Peek(index, out _);


        /// <summary>
        /// 创建一个新的ViewStack对象
        /// </summary>
        /// <param name="minGrow">栈长度不够时，重新分配的栈的长度的最小增长量，该值必须大于0</param>
        /// <param name="maxGrow">栈长度不够时，重新分配的栈的长度的最大增长量，该值必须大于0</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minGrow"/>小于1或者大于<paramref name="maxGrow"/></exception>
        public ViewStack(int minGrow, int maxGrow) : this(minGrow, maxGrow, EqualityComparer<object>.Default) { }

        /// <summary>
        /// 创建一个新的ViewStack对象
        /// </summary>
        /// <param name="minGrow">栈长度不够时，重新分配的栈的长度的最小增长量，该值必须大于0</param>
        /// <param name="maxGrow">栈长度不够时，重新分配的栈的长度的最大增长量，该值必须大于0</param>
        /// <param name="objKeyComparer">元素的Key的比较器</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minGrow"/>小于1或者大于<paramref name="maxGrow"/></exception>
        /// <exception cref="ArgumentNullException"><paramref name="objKeyComparer"/>为null</exception>
        public ViewStack(int minGrow, int maxGrow, IEqualityComparer<object> objKeyComparer)
        {
            if (minGrow < 1 || maxGrow < minGrow)
            {
                throw new ArgumentOutOfRangeException(nameof(minGrow));
            }

            m_Version = int.MinValue;
            m_TopIndex = -1;
            m_Stack = Array.Empty<Element>();
            m_MinGrow = minGrow;
            m_MaxGrow = maxGrow;
            m_KeyComparer = objKeyComparer ?? throw new ArgumentNullException(nameof(objKeyComparer));
        }


        /// <summary>
        /// 在栈顶添加一个新元素
        /// </summary>
        /// <param name="viewKey">元素的Key</param>
        /// <param name="view">要添加的元素，该值不能为null</param>
        /// <param name="openViewParam">打开页面的参数</param>
        /// <param name="suspendViewParam">暂停页面的参数</param>
        /// <exception cref="ArgumentNullException"><paramref name="view"/>为null</exception>
        public void Push(object viewKey, AbstractView view, object openViewParam, object suspendViewParam)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (m_TopIndex > -1)
            {
                AbstractView last = m_Stack[m_TopIndex].View;
                last.SetState(ViewState.Suspended, suspendViewParam); // 暂停上一个页面
            }

            m_TopIndex++;

            if (m_Stack.Length == m_TopIndex)
            {
                Grow();
            }

            m_Stack[m_TopIndex] = new Element(viewKey, view); // 放到栈顶

            view.OnBeforeTransition += OnBeforeViewTransition;
            view.OnAfterTransition += OnAfterViewTransition;
            view.SetState(ViewState.Active, openViewParam);

            m_Version++;
        }

        /// <summary>
        /// 根据<paramref name="viewKey"/>尝试从**栈顶元素后面一个元素开始匹配**，并将第一个匹配的元素移动至栈顶
        /// </summary>
        /// <param name="viewKey">匹配元素的Key</param>
        /// <param name="navigatedView">被导航的页面</param>
        /// <param name="navigateViewParam">导航页面的参数</param>
        /// <param name="suspendViewParam">暂停页面的参数</param>
        /// <returns>如果成功匹配元素并移动至栈顶，返回true；否则返回false</returns>
        public bool TryMoveToTop(object viewKey, out AbstractView navigatedView, object navigateViewParam, object suspendViewParam)
        {
            // 不检测最上方的页面
            for (int i = m_TopIndex - 1; i > -1; i--)
            {
                Element element = m_Stack[i];

                if (m_KeyComparer.Equals(viewKey, element.ViewKey))
                {
                    for (int j = i; j < m_TopIndex; j++)
                    {
                        m_Stack[j] = m_Stack[j + 1];
                    }

                    // 这里一定有需要被暂停的页面
                    AbstractView last = m_Stack[m_TopIndex - 1].View;
                    last.SetState(ViewState.Suspended, suspendViewParam);

                    m_Stack[m_TopIndex] = element; // 放到栈顶

                    navigatedView = element.View;
                    navigatedView.SetState(ViewState.Active, navigateViewParam);

                    m_Version++;
                    return true;
                }
            }

            navigatedView = default;
            return false;
        }

        /// <summary>
        /// 如果栈中有元素，则移除栈顶元素
        /// </summary>
        /// <param name="closeViewParam">关闭页面的参数</param>
        /// <param name="resumeViewParam">恢复页面的参数</param>
        /// <param name="removedViewKey">被移除的元素的Key</param>
        /// <param name="removedView">被移除的元素</param>
        /// <returns>如果成功移除了一个元素，返回true；否则返回false</returns>
        public bool TryPop(object closeViewParam, object resumeViewParam, out object removedViewKey, out AbstractView removedView)
        {
            if (m_TopIndex < 0)
            {
                removedViewKey = default;
                removedView = default;
                return false;
            }

            Element element = m_Stack[m_TopIndex];
            m_Stack[m_TopIndex--] = default;

            removedViewKey = element.ViewKey;
            removedView = element.View;
            removedView.SetState(ViewState.Closed, closeViewParam);

            if (m_TopIndex > -1)
            {
                AbstractView last = m_Stack[m_TopIndex].View;
                last.SetState(ViewState.Active, resumeViewParam); // 恢复上一个页面
            }

            m_Version++;
            return true;
        }

        /// <summary>
        /// 获取栈顶的元素
        /// </summary>
        /// <param name="viewKey">元素的Key</param>
        /// <returns>如果栈中有元素，则返回栈顶元素；否则返回null</returns>
        public AbstractView Peek(out object viewKey)
        {
            return Peek(0, out viewKey);
        }

        /// <summary>
        /// 获取指定索引处的元素
        /// </summary>
        /// <param name="index">元素的索引（栈顶元素的索引为0，向下依次以1递增）</param>
        /// <param name="viewKey">元素的Key</param>
        /// <returns>如果索引位置有元素，则返回该元素；否则返回null</returns>
        public AbstractView Peek(int index, out object viewKey)
        {
            int i = m_TopIndex - index;

            if (i > -1 && i <= m_TopIndex)
            {
                Element element = m_Stack[i];
                viewKey = element.ViewKey;
                return element.View;
            }

            viewKey = default;
            return default;
        }

        /// <summary>
        /// 匹配顶部元素的Key是否与<paramref name="viewKey"/>相等
        /// </summary>
        /// <param name="viewKey">需要匹配的Key</param>
        /// <returns>如果顶部元素的Key与<paramref name="viewKey"/>相等，返回true；否则返回false</returns>
        public bool MatchTopKey(object viewKey)
        {
            if (m_TopIndex < 0)
            {
                return false;
            }

            object key = m_Stack[m_TopIndex].ViewKey;
            return m_KeyComparer.Equals(viewKey, key);
        }

        /// <summary>
        /// 获取栈中是否包含指定Key
        /// </summary>
        /// <param name="viewKey">查询的Key</param>
        /// <returns>如果包含该Key，返回true；否则返回false</returns>
        public bool ContainsKey(object viewKey)
        {
            for (int i = m_TopIndex; i > -1; i--)
            {
                if (m_KeyComparer.Equals(viewKey, m_Stack[i].ViewKey))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取栈中是否包含指定元素
        /// </summary>
        /// <param name="view">查询的元素</param>
        /// <returns>如果包含该元素，返回true；否则返回false</returns>
        public bool ContainsView(AbstractView view)
        {
            if (view != null)
            {
                for (int i = m_TopIndex; i > -1; i--)
                {
                    if (m_Stack[i].View == view)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取当前实例的迭代器。迭代器将从栈顶开始向下依次遍历。
        /// </summary>
        /// <returns>当前实例的迭代器</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }


        private void OnBeforeViewTransition(AbstractView view, ViewState nextState)
        {
            switch (nextState)
            {
                case ViewState.Closed:
                    view.OnBeforeTransition -= OnBeforeViewTransition; // 页面被关闭，取消对事件的监听
                    break;

                case ViewState.Active:
                    if (view.State == ViewState.Closed)
                    {
                        view.gameObject.SetActive(true); // 以防万一
                        view.Actions?.Reset(); // 打开页面时，清空之前的状态
                    }
                    else // if (view.State == ViewState.Suspended)
                    {
                        view.enabled = true;
                    }

                    view.Transform.SetAsLastSibling();
                    break;
            }
        }

        private void OnAfterViewTransition(AbstractView view)
        {
            switch (view.State)
            {
                case ViewState.Closed:
                    //view.enabled = false;
                    view.gameObject.SetActive(false); // 以防万一
                    view.OnAfterTransition -= OnAfterViewTransition; // 页面被关闭，取消对事件的监听
                    break;

                case ViewState.Suspended:
                    view.enabled = false;
                    break;
            }
        }

        private void Grow()
        {
            int newCapacity = Mathf.Clamp(m_Stack.Length << 1, m_Stack.Length + m_MinGrow, m_Stack.Length + m_MaxGrow);
            Element[] array = new Element[newCapacity];

            Array.Copy(m_Stack, 0, array, 0, m_Stack.Length);
            m_Stack = array;
        }


        IEnumerator<AbstractView> IEnumerable<AbstractView>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}