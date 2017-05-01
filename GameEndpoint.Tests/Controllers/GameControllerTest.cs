using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEndpoint;
using GameEndpoint.Controllers;
using System.Threading.Tasks;
using GameEndpoint.Model;
using System.Web.Http.Results;

namespace GameEndpoint.Tests.Controllers
{
    [TestClass]
    public class GameControllerTest
    {
        [TestMethod]
        public void Get()
        {
            GameController controller = new GameController();

            var result = controller.Get() as Task<IHttpActionResult>;

            var viewresult = result.Result;

            Assert.AreEqual(viewresult.GetType(), typeof(OkNegotiatedContentResult<IEnumerable<Leaderboard>>));

            IEnumerable<Leaderboard> leaders = (viewresult as OkNegotiatedContentResult<IEnumerable<Leaderboard>>).Content;

            Assert.IsTrue(leaders.Count() > 0);
        }

        [TestMethod]
        public void Post()
        {
            GameController controller = new GameController();

            List<GameResult> gameResults = new List<GameResult>
            {
                new GameResult
                {
                    GameId = 0,
                    PlayerId = 0,
                    Timestamp = DateTimeOffset.Now,
                    Win = 0
                }
            };

            Task<IHttpActionResult> result = controller.Post(gameResults) as Task<IHttpActionResult>;
            IHttpActionResult viewresult = result.Result;
            Assert.AreEqual(viewresult.GetType(), typeof(BadRequestErrorMessageResult));

            string message = (viewresult as BadRequestErrorMessageResult).Message;
            Assert.AreEqual(message, "Game id 0 not exist");

            gameResults = new List<GameResult>
            {
                new GameResult
                {
                    GameId = 1,
                    PlayerId = 1,
                    Timestamp = DateTimeOffset.Now,
                    Win = 10
                }
            };

            result = controller.Post(gameResults) as Task<IHttpActionResult>;
            viewresult = result.Result;
            Assert.AreEqual(viewresult.GetType(), typeof(OkResult));
        }

        [TestMethod]
        public void MyUTC()
        {
            GameController controller = new GameController();

            var result = controller.Get() as Task<IHttpActionResult>;

            var viewresult = result.Result;

            Assert.AreEqual(viewresult.GetType(), typeof(OkNegotiatedContentResult<IEnumerable<Leaderboard>>));

            IEnumerable<Leaderboard> leaders = (viewresult as OkNegotiatedContentResult<IEnumerable<Leaderboard>>).Content;

            TimeZoneInfo local = TimeZoneInfo.Local;
            Assert.AreEqual(leaders.ToList()[0].LastUpdateDate.Offset, local.BaseUtcOffset);
        }
    }
}
