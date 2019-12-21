using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Threading.Tasks;
using Assets.Scripts.Web;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;

namespace Tests
{
    public class StompBasic
    {
        // A Test behaves as an ordinary method
        [Test]
        public void StompBasicSimplePasses()
        {
            // 983584 983585 983139
            HashSet<Tile> tiles = Rule.GetEatableTiles(new Tile(983139), new List<Tile>() { new Tile(983585), new Tile(983584) });
            Assert.IsNull(tiles);
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
