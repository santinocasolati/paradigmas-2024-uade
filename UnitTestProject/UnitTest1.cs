using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddVector()
        {
            Vector2 vector = new Vector2(2, 2);
            Vector2 addVector = new Vector2(2, 2);
            vector.Add(addVector);

            Vector2 expectedResult = new Vector2(4, 4);
            Assert.AreEqual(vector, expectedResult);
        }

        [TestMethod]
        public void TestClearPool()
        {
            ObjectPool<string> testPool = new ObjectPool<string>(() => "");
            
            for (int i = 0; i < 5; i++)
            {
                testPool.ReleaseObject("");
            }

            testPool.ClearPool();
            Assert.AreEqual(testPool.Length, 0);
        }

        [TestMethod]
        public void TestMove()
        {
            Transform t = new Transform();
            t.Move(new Vector2(2, 2));
            t.Move(new Vector2(2, 2));

            Vector2 expectedResult = new Vector2(4, 4);

            Assert.AreEqual(t.position, expectedResult);
        }
    }
}
