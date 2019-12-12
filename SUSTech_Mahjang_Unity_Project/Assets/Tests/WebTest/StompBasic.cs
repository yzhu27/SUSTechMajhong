using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Threading.Tasks;
using Assets.Scripts.Web;

namespace Tests
{
    public class StompBasic
    {
        // A Test behaves as an ordinary method
        [Test]
        public void StompBasicSimplePasses()
        {
			Web w = new Web(new System.Uri("ws://10.21.63.106:8083/ws/websocket"));
			Task t = w.test();
			t.Wait(10000);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator StompBasicWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
