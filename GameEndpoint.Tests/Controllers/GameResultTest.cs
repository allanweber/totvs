
using GameEndpoint.Business;
using GameEndpoint.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEndpoint.Tests.Controllers
{
    [TestClass]
    public class GameResultTest
    {
        [TestMethod]
        public void GroupBy()
        {
            List<GameResult> results = new List<GameResult>
            {
                new GameResult { GameId = 1, PlayerId = 1, Timestamp = DateTime.Now, Win = 10 },
                new GameResult { GameId = 1, PlayerId = 1, Timestamp = DateTime.Now.AddDays(1), Win = 10 },
                new GameResult { GameId = 1, PlayerId = 1, Timestamp = DateTime.Now, Win = 10 },

                new GameResult { GameId = 2, PlayerId = 1, Timestamp = DateTime.Now, Win = 10 },
                new GameResult { GameId = 2, PlayerId = 1, Timestamp = DateTime.Now, Win = 10 },
                new GameResult { GameId = 2, PlayerId = 1, Timestamp = DateTime.Now.AddDays(2), Win = 10 },
                new GameResult { GameId = 2, PlayerId = 1, Timestamp = DateTime.Now, Win = 10 },

                new GameResult { GameId = 2, PlayerId = 2, Timestamp = DateTime.Now.AddDays(3), Win = 10 },
                new GameResult { GameId = 2, PlayerId = 2, Timestamp = DateTime.Now, Win = 10 },

                new GameResult { GameId = 1, PlayerId = 2, Timestamp = DateTime.Now, Win = 10 },
                new GameResult { GameId = 1, PlayerId = 2, Timestamp = DateTime.Now, Win = 10 },
                new GameResult { GameId = 1, PlayerId = 2, Timestamp = DateTime.Now.AddDays(4), Win = 10 },
            };

            GameResultPersist persist = new GameResultPersist();

            GameResult[] groupList = persist.GroupBy(results.ToArray());

            var game1_pLayer1 = groupList.Where(g => g.GameId == 1 && g.PlayerId == 1);
            var game2_pLayer1 = groupList.Where(g => g.GameId == 2 && g.PlayerId == 1);
            var game1_pLayer2 = groupList.Where(g => g.GameId == 1 && g.PlayerId == 2);
            var game2_pLayer2 = groupList.Where(g => g.GameId == 2 && g.PlayerId == 2);

            Assert.AreEqual(4, groupList.Count());
            Assert.AreEqual(1, game1_pLayer1.Count());
            Assert.AreEqual(1, game2_pLayer1.Count());
            Assert.AreEqual(1, game1_pLayer2.Count());
            Assert.AreEqual(1, game2_pLayer2.Count());

            Assert.AreEqual(30, game1_pLayer1.FirstOrDefault().Win);
            Assert.AreEqual(40, game2_pLayer1.FirstOrDefault().Win);
            Assert.AreEqual(20, game2_pLayer2.FirstOrDefault().Win);
            Assert.AreEqual(30, game1_pLayer2.FirstOrDefault().Win);

            Assert.AreEqual(DateTime.Now.AddDays(1).Day, game1_pLayer1.FirstOrDefault().Timestamp.Day);
            Assert.AreEqual(DateTime.Now.AddDays(2).Day, game2_pLayer1.FirstOrDefault().Timestamp.Day);
            Assert.AreEqual(DateTime.Now.AddDays(3).Day, game2_pLayer2.FirstOrDefault().Timestamp.Day);
            Assert.AreEqual(DateTime.Now.AddDays(4).Day, game1_pLayer2.FirstOrDefault().Timestamp.Day);
        }
    }
}

