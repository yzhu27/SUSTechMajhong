using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.Web;
using System.Threading.Tasks;

namespace Tests
{
    public class Stomp
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Connect()
        {
			var client = new StompClient(new System.Uri("ws://10.21.63.106:8083/ws/websocket"));
			Task task = client.Connect();
			task.Wait(10000);
			task = client.Subscribe("/topic/public");
			task.Wait(1000);
			for(int i = 0; i<10; i++)
			{
				//task = client.Send();
				task = client.Send("/app/public.sendMessage", "{\"sender\":\"Unity\",\"type\":\"CHAT\",\"content\":\"ko no DIO da!!!\"}");
				task.Wait(1000);
			}
			task = client.DisConnect();
			task.Wait(1000);
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
