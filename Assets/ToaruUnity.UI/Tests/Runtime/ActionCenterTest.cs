//using NUnit.Framework;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using ToaruUnity.UI;
//using UnityEngine.TestTools;

//namespace Tests
//{
//    public sealed class ActionCenterTest
//    {
//        [InjectActionCenter(typeof(TestActionCenter))]
//        public sealed class TestView : AbstractView { }

//        [InjectActionCenter(typeof(int))]
//        public sealed class InvalidTestView1 : AbstractView { }

//        [InjectActionCenter(typeof(InvalidActionCenter))]
//        public sealed class InvalidTestView2 : AbstractView { }

//        public abstract class InvalidActionCenter : ActionCenter { }


//        public sealed class TestState : IActionState
//        {
//            public string Name;
//            public int Age;
//            public int Id;
//        }

//        public sealed class TestActionCenter : ActionCenter
//        {
//            protected override IActionState CreateState()
//            {
//                return new TestState();
//            }

//            protected override void ResetState(ref IActionState state)
//            {
//                state = new TestState();
//            }

//            [HandleAction(0)]
//            public bool HandleAction0()
//            {
//                TestState state = GetState<TestState>();

//                Assert.NotNull(state);
//                Assert.AreEqual(state.Name, default(string));
//                Assert.AreEqual(state.Age, default(int));
//                Assert.AreEqual(state.Id, default(int));

//                state.Name = "Test Name";
//                state.Age = 1;
//                state.Id = 1;

//                return true;
//            }

//            [HandleAction(1)]
//            public bool HandleAction1()
//            {
//                TestState state = GetState<TestState>();

//                Assert.NotNull(state);
//                Assert.AreEqual(state.Name, "Test Name");
//                Assert.AreEqual(state.Age, 1);
//                Assert.AreEqual(state.Id, 1);

//                state.Name = "Null";
//                state.Age = 2;
//                state.Id = 2;

//                return false;
//            }


//            [HandleAction(10)]
//            public IEnumerator<bool> HandleAction5()
//            {
//                TestState state = GetState<TestState>();
//                yield return false;

//                Assert.NotNull(state);
//                yield return false;

//                Assert.AreEqual(state.Name, default(string));
//                yield return false;

//                Assert.AreEqual(state.Age, default(int));
//                yield return false;

//                Assert.AreEqual(state.Id, default(int));
//                yield return false;

//                state.Name = "Test Name";
//                yield return false;

//                state.Age = 1;
//                yield return false;

//                state.Id = 1;
//                yield return false;

//                yield return true;
//            }

//            [HandleAction(11)]
//            public IEnumerator<bool> HandleAction11()
//            {
//                TestState state = GetState<TestState>();
//                yield return false;

//                Assert.NotNull(state);
//                yield return false;

//                Assert.AreEqual(state.Name, "Test Name");
//                yield return false;

//                Assert.AreEqual(state.Age, 1);
//                yield return false;

//                Assert.AreEqual(state.Id, 1);
//                yield return false;

//                state.Name = "Null";
//                yield return false;

//                state.Age = 2;
//                yield return false;

//                state.Id = 2;
//                yield return false;

//                yield return false;
//            }
//        }

//        private UIManager m_UIManager;

//        [SetUp]
//        public void SetUp()
//        {
//            //m_UIManager = new UIManager(new AddressableUIFactory());
//        }

//        [Test]
//        public void GetActionCenterType()
//        {
//            //Type type = ActionCenter.GetActionCenterType(typeof(TestView));

//            //Assert.NotNull(type);
//            //Assert.AreEqual(typeof(TestActionCenter), type);


//            //Assert.Throws(typeof(ActionCenter.InvalidTypeInjectionException), () =>
//            //{
//            //    ActionCenter.GetActionCenterType(typeof(InvalidTestView1));
//            //});

//            //Assert.Throws(typeof(ActionCenter.InvalidTypeInjectionException), () =>
//            //{
//            //    ActionCenter.GetActionCenterType(typeof(InvalidTestView2));
//            //});
//        }

//        [Test]
//        public void ActionCenterNew()
//        {
//            ActionCenter center = ActionCenter.New(typeof(TestActionCenter), m_UIManager);

//            Assert.NotNull(center);
//            Assert.IsInstanceOf<TestActionCenter>(center);
//            //Assert.AreEqual(1, center.ActionCount);
//            Assert.Zero(center.ExecutingCoroutineCount);
//        }

//        [Test]
//        public void ActionCenterClone()
//        {
//            ActionCenter prototype = ActionCenter.New(typeof(TestActionCenter), m_UIManager);
//            ActionCenter center = ActionCenter.Clone(prototype);

//            Assert.NotNull(center);
//            Assert.AreEqual(prototype.GetType(), center.GetType());
//            Assert.AreEqual(prototype.ActionCount, center.ActionCount);
//            Assert.Zero(center.ExecutingCoroutineCount);
//        }


//        [Test]
//        public void ActionCenterDispatchWithZeroArgs()
//        {
//            ActionCenter center = ActionCenter.New(typeof(TestActionCenter), m_UIManager);

//            center.RegisterStateChangeHandler(s =>
//            {
//                TestState state = s as TestState;

//                Assert.NotNull(state);
//                Assert.AreEqual(state.Name, "Test Name");
//                Assert.AreEqual(state.Age, 1);
//                Assert.AreEqual(state.Id, 1);
//            });

//            center.Dispatch(0);
//            center.Dispatch(1);
//        }

//        [UnityTest]
//        public IEnumerator ActionCenterDispatchCoroutineWithZeroArgs()
//        {
//            ActionCenter center = ActionCenter.New(typeof(TestActionCenter), m_UIManager);

//            center.RegisterStateChangeHandler(s =>
//            {
//                TestState state = s as TestState;

//                Assert.NotNull(state);
//                Assert.AreEqual(state.Name, "Test Name");
//                Assert.AreEqual(state.Age, 1);
//                Assert.AreEqual(state.Id, 1);
//            });

//            center.Dispatch(10);
            
//            while (center.ExecutingCoroutineCount > 0)
//            {
//                center.UpdateCoroutines();
//                yield return null;
//            }

//            center.Dispatch(11);

//            while (center.ExecutingCoroutineCount > 0)
//            {
//                center.UpdateCoroutines();
//                yield return null;
//            }

//            yield return null;
//        }
//    }
//}
