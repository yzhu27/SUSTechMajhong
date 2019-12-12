using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.Web;
using System.Threading.Tasks;

namespace Tests
{
	[TestFixture]
    public class Stomp
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Connect()
        {

			Task t = ConectTest();
			t.Wait(10000);
        }

		async Task ConectTest()
		{
			var client = new Web(new System.Uri("ws://10.21.63.106:8083/ws/websocket"));
			await client.test();
			//await client.Connect();
			//client.Subscribe("/topic/echo");
			//for (int i = 0; i < 10; i++)
			//{
			//	client.Send("/app/echo", "{\"sender\":\"Unity\",\"type\":\"CHAT\",\"content\":\"ko no DIO da!!!\"}");
			//}
			//await client.DisConnect();
		}

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator StompWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
