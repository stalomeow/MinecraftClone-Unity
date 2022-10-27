using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;

namespace ToaruUnity.UI
{
    /// <summary>
    /// 表示界面逻辑的操作中心
    /// </summary>
    public abstract class ActionCenter
    {
        internal sealed class ParameterCountException : Exception
        {
            public ParameterCountException(MethodInfo method, string message)
                : base($"{message}\n在({method.DeclaringType}) : {method}") { }
        }

        internal sealed class ReturnTypeMismatchException : Exception
        {
            public ReturnTypeMismatchException(MethodInfo method, string message)
                : base($"{message}\n在({method.DeclaringType}) : {method}") { }
        }

        internal sealed class InvalidTypeInjectionException : Exception
        {
            public InvalidTypeInjectionException(Type type, string message)
                : base($"{message}\n类型: {type}") { }
        }


        /// <summary>
        /// 当状态改变时的处理方法
        /// </summary>
        /// <param name="state">当前的状态</param>
        internal delegate void StateChangeHandler(IActionState state);

        // 如果返回值为true，自动调用StateChangeHandler
        private delegate bool ActionHandler();
        private delegate bool ActionHandler<in T0>(T0 arg0);
        private delegate bool ActionHandler<in T0, in T1>(T0 arg0, T1 arg1);
        private delegate bool ActionHandler<in T0, in T1, in T2>(T0 arg0, T1 arg1, T2 arg2);
        private delegate bool ActionHandler<in T0, in T1, in T2, in T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);

        // 如果返回值为true，自动调用StateChangeHandler
        private delegate IEnumerator<bool> ActionHandlerCoroutine();
        private delegate IEnumerator<bool> ActionHandlerCoroutine<in T0>(T0 arg0);
        private delegate IEnumerator<bool> ActionHandlerCoroutine<in T0, in T1>(T0 arg0, T1 arg1);
        private delegate IEnumerator<bool> ActionHandlerCoroutine<in T0, in T1, in T2>(T0 arg0, T1 arg1, T2 arg2);
        private delegate IEnumerator<bool> ActionHandlerCoroutine<in T0, in T1, in T2, in T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);

        private const int s_MaxParameterCount = 4;


        private IReadOnlyDictionary<string, Delegate> m_ActionMap; // may be null
        private List<IEnumerator<bool>> m_Coroutines; // may be null
        private StateChangeHandler m_StateChangeHandler;
        private IActionState m_State;

        /// <summary>
        /// 获取对<see cref="IUIManager"/>的引用
        /// </summary>
        protected IUIManager Manager { get; private set; }

        /// <summary>
        /// 获取ActionMap
        /// </summary>
        internal IEnumerable<KeyValuePair<string, Delegate>> ActionMap => m_ActionMap;

        /// <summary>
        /// 获取操作的数量
        /// </summary>
        public int ActionCount => m_ActionMap == null ? 0 : m_ActionMap.Count;

        /// <summary>
        /// 获取当前执行的协程的数量
        /// </summary>
        public int ExecutingCoroutineCount => m_Coroutines == null ? 0 : m_Coroutines.Count;

        /// <summary>
        /// 获取指定名称的操作的信息
        /// </summary>
        /// <param name="actionName">查询的操作的名称</param>
        /// <returns>如果查询到操作，则返回操作的信息对象；否则返回null</returns>
        public ActionInfo this[string actionName]
        {
            get
            {
                if (TryGetActionHandler(actionName, out Delegate func))
                {
                    MethodInfo method = func.Method;
                    return new ActionInfo(this, actionName, method);
                }

                return default;
            }
        }


        protected ActionCenter() { }

        
        public void Execute(string name)
        {
            if (TryGetActionHandler(name, out Delegate func))
            {
                switch (func)
                {
                    case ActionHandler handler:
                        {
                            if (handler())
                            {
                                m_StateChangeHandler(m_State);
                            }
                        }
                        break;
                    case ActionHandlerCoroutine coroutine:
                        {
                            IEnumerator<bool> routine = coroutine();
                            AddCoroutine(routine);
                        }
                        break;
                }
            }
            else
            {
                Debug.LogWarning("未找到操作：" + name);
            }
        }

        public void Execute<T0>(string name, T0 arg0)
        {
            if (TryGetActionHandler(name, out Delegate func))
            {
                switch (func)
                {
                    case ActionHandler<T0> handler:
                        {
                            if (handler(arg0))
                            {
                                m_StateChangeHandler(m_State);
                            }
                        }
                        break;
                    case ActionHandlerCoroutine<T0> coroutine:
                        {
                            IEnumerator<bool> routine = coroutine(arg0);
                            AddCoroutine(routine);
                        }
                        break;
                }
            }
            else
            {
                Debug.LogWarning("未找到操作：" + name);
            }
        }

        public void Execute<T0, T1>(string name, T0 arg0, T1 arg1)
        {
            if (TryGetActionHandler(name, out Delegate func))
            {
                switch (func)
                {
                    case ActionHandler<T0, T1> handler:
                        {
                            if (handler(arg0, arg1))
                            {
                                m_StateChangeHandler(m_State);
                            }
                        }
                        break;
                    case ActionHandlerCoroutine<T0, T1> coroutine:
                        {
                            IEnumerator<bool> routine = coroutine(arg0, arg1);
                            AddCoroutine(routine);
                        }
                        break;
                }
            }
            else
            {
                Debug.LogWarning("未找到操作：" + name);
            }
        }

        public void Execute<T0, T1, T2>(string name, T0 arg0, T1 arg1, T2 arg2)
        {
            if (TryGetActionHandler(name, out Delegate func))
            {
                switch (func)
                {
                    case ActionHandler<T0, T1, T2> handler:
                        {
                            if (handler(arg0, arg1, arg2))
                            {
                                m_StateChangeHandler(m_State);
                            }
                        }
                        break;
                    case ActionHandlerCoroutine<T0, T1, T2> coroutine:
                        {
                            IEnumerator<bool> routine = coroutine(arg0, arg1, arg2);
                            AddCoroutine(routine);
                        }
                        break;
                }
            }
            else
            {
                Debug.LogWarning("未找到操作：" + name);
            }
        }

        public void Execute<T0, T1, T2, T3>(string name, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            if (TryGetActionHandler(name, out Delegate func))
            {
                switch (func)
                {
                    case ActionHandler<T0, T1, T2, T3> handler:
                        {
                            if (handler(arg0, arg1, arg2, arg3))
                            {
                                m_StateChangeHandler(m_State);
                            }
                        }
                        break;
                    case ActionHandlerCoroutine<T0, T1, T2, T3> coroutine:
                        {
                            IEnumerator<bool> routine = coroutine(arg0, arg1, arg2, arg3);
                            AddCoroutine(routine);
                        }
                        break;
                }
            }
            else
            {
                Debug.LogWarning("未找到操作：" + name);
            }
        }

        // 不重置StateChangeHandler
        public void Reset()
        {
            m_Coroutines?.Clear();
            ResetState(ref m_State);
        }


        /// <summary>
        /// 获取表示当前状态的对象，并将其转换为<typeparamref name="T"/>类型
        /// </summary>
        /// <typeparam name="T">状态的类型，该类型必须为引用类型，且实现<see cref="IActionState"/>接口</typeparam>
        /// <returns>经过类型转换后的状态对象</returns>
        public T GetState<T>() where T : class, IActionState { return m_State as T; }

        /// <summary>
        /// 重写该方法，修改Action处理方法的匹配模式（默认为<see cref="BindingFlags.Instance"/> | <see cref="BindingFlags.Public"/>）。
        /// 如果该方法返回<see cref="BindingFlags.Default"/>，则不会匹配任何方法。
        /// </summary>
        /// <returns></returns>
        protected virtual BindingFlags GetBindingFlagsForActionHandler() { return BindingFlags.Instance | BindingFlags.Public; }

        /// <summary>
        /// 用于创建新的状态对象
        /// </summary>
        /// <returns></returns>
        protected virtual IActionState CreateState() { return null; }

        /// <summary>
        /// 用于重置状态对象
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ResetState(ref IActionState state) { }


        internal void UpdateCoroutines()
        {
            if (m_Coroutines == null)
                return;

            Profiler.BeginSample("ActionCenter.UpdateCoroutines");

            bool stateChanged = false;

            for (int i = m_Coroutines.Count - 1; i > -1; i--)
            {
                IEnumerator<bool> routine = m_Coroutines[i];

                if (routine.MoveNext())
                {
                    stateChanged |= routine.Current;
                }
                else
                {
                    routine.Dispose();
                    m_Coroutines.RemoveAt(i);
                }
            }

            if (stateChanged)
            {
                m_StateChangeHandler(m_State);
            }

            Profiler.EndSample();
        }

        internal void RegisterStateChangeHandler(StateChangeHandler stateChangeHandler)
        {
            m_StateChangeHandler = stateChangeHandler ?? throw new ArgumentNullException(nameof(stateChangeHandler));
        }

        private void Initialize(IUIManager manager)
        {
            Initialize(GetActionMap(), manager);
        }

        private void Initialize(IReadOnlyDictionary<string, Delegate> actionMap, IUIManager manager)
        {
            m_ActionMap = actionMap;
            m_Coroutines = null;
            m_StateChangeHandler = null;
            m_State = CreateState();
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        private bool TryGetActionHandler(string actionName, out Delegate func)
        {
            if (m_ActionMap == null)
            {
                func = null;
                return false;
            }

            return m_ActionMap.TryGetValue(actionName, out func);
        }

        private void AddCoroutine(IEnumerator<bool> routine)
        {
            if (m_Coroutines == null)
            {
                m_Coroutines = new List<IEnumerator<bool>>();
            }

            m_Coroutines.Add(routine);
        }

        private IReadOnlyDictionary<string, Delegate> GetActionMap()
        {
            Profiler.BeginSample("ActionCenter.GetActionMap");

            Dictionary<string, Delegate> map = null;
            BindingFlags flags = GetBindingFlagsForActionHandler();

            if (flags != BindingFlags.Default)
            {
                Type actionCenterType = GetType();
                MethodInfo[] methods = actionCenterType.GetMethods(flags);

                foreach (MethodInfo method in methods)
                {
                    object[] attrs = method.GetCustomAttributes(typeof(ActionAttribute), false);

                    if (attrs.Length == 0)
                        continue;

                    ParameterInfo[] parameters = method.GetParameters();

                    if (parameters.Length > s_MaxParameterCount)
                        throw new ParameterCountException(method, "参数数量必须在[0,4]范围内");

                    Type[] typeArguments = AllocTypeArray(parameters.Length);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        typeArguments[i] = parameters[i].ParameterType;
                    }

                    Delegate callback = method.CreateDelegate(GetDelegateType(method, typeArguments), method.IsStatic ? null : this);

                    for (int i = 0; i < attrs.Length; i++)
                    {
                        if (attrs[i] is ActionAttribute attr)
                        {
                            if (map == null)
                            {
                                map = new Dictionary<string, Delegate>(StringComparer.Ordinal);
                            }

                            map.Add(attr.ActionName ?? method.Name, callback);
                        }
                    }
                }
            }

            Profiler.EndSample();
            return map;
        }


        internal static ActionCenter New(Type viewType, UIManager manager)
        {
            Profiler.BeginSample("ActionCenter.New");

            object[] attrs = viewType.GetCustomAttributes(typeof(InjectActionsAttribute), false);

            if (attrs.Length != 1)
                return null;

            Type actionCenterType = (attrs[0] as InjectActionsAttribute).ActionCenterType;

            if (actionCenterType.IsAbstract)
                throw new InvalidTypeInjectionException(actionCenterType, "注入的类型必须是非抽象类型");

            if (!actionCenterType.IsSubclassOf(typeof(ActionCenter)))
                throw new InvalidTypeInjectionException(actionCenterType, "注入的类型必须派生自" + typeof(ActionCenter));

            ActionCenter center = Activator.CreateInstance(actionCenterType) as ActionCenter;
            center.Initialize(manager);

            Profiler.EndSample();
            return center;
        }

        internal static ActionCenter Clone(ActionCenter prototype)
        {
            if (prototype == null)
                return null;

            Profiler.BeginSample("ActionCenter.Clone");

            Type type = prototype.GetType();
            ActionCenter center = Activator.CreateInstance(type) as ActionCenter;

            Dictionary<string, Delegate> map = null;
            IReadOnlyDictionary<string, Delegate> actionMap = prototype.m_ActionMap;

            if (actionMap != null)
            {
                map = new Dictionary<string, Delegate>(actionMap.Count, StringComparer.Ordinal);

                foreach (KeyValuePair<string, Delegate> pair in actionMap)
                {
                    Type delegateType = pair.Value.GetType();
                    MethodInfo method = pair.Value.Method;
                    Delegate handler = method.CreateDelegate(delegateType, center);

                    map.Add(pair.Key, handler);
                }
            }

            center.Initialize(map, prototype.Manager);

            Profiler.EndSample();
            return center;
        }

        private static Type[] AllocTypeArray(int length)
        {
            return length == 0 ? Array.Empty<Type>() : new Type[length];
        }

        private static Type GetDelegateType(MethodInfo method, Type[] typeArguments)
        {
            Type boolType = typeof(bool);
            Type coroutineType = typeof(IEnumerator<bool>);
            Type returnType = method.ReturnType;

            if (returnType == boolType)
            {
                switch (typeArguments.Length)
                {
                    case 0: return typeof(ActionHandler);
                    case 1: return typeof(ActionHandler<>).MakeGenericType(typeArguments);
                    case 2: return typeof(ActionHandler<,>).MakeGenericType(typeArguments);
                    case 3: return typeof(ActionHandler<,,>).MakeGenericType(typeArguments);
                    case 4: return typeof(ActionHandler<,,,>).MakeGenericType(typeArguments);
                    default: return null; // 永远不会发生
                }
            }
            else if (returnType == coroutineType)
            {
                switch (typeArguments.Length)
                {
                    case 0: return typeof(ActionHandlerCoroutine);
                    case 1: return typeof(ActionHandlerCoroutine<>).MakeGenericType(typeArguments);
                    case 2: return typeof(ActionHandlerCoroutine<,>).MakeGenericType(typeArguments);
                    case 3: return typeof(ActionHandlerCoroutine<,,>).MakeGenericType(typeArguments);
                    case 4: return typeof(ActionHandlerCoroutine<,,,>).MakeGenericType(typeArguments);
                    default: return null; // 永远不会发生
                }
            }
            else
            {
                throw new ReturnTypeMismatchException(method, $"返回值的类型必须为{boolType}、{coroutineType}类型中的一个");
            }
        }
    }
}