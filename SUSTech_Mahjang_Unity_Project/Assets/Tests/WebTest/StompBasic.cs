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
			
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator StompBasicWithEnumeratorPasses()
        {
            
            yield return null;
        }
    }
}
