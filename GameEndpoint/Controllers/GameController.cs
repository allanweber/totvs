using GameEndpoint.Business;
using GameEndpoint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GameEndpoint.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        /// <summary>
        /// Busca os pontos dos 100 melhores jogadores
        /// </summary>
        /// <returns>Em caso de sucesso, além do status 200 OK retorna um objeto do tipo OkNegotiatedContentResult<IEnumerable<Leaderboard>>
        /// Em caso de erro retorna um objeto do tipo BadRequestErrorMessageResult com a mensagem de erro</returns>
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return await Task.Run(() => Ok(new GameResultPersist().GetBest()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Posta os dados de pontuações de um ou mais jogadores em um ou mais jogos
        /// </summary>
        /// <param name="games">Coleção de pontuação de um jogador em um jogo</param>
        /// <returns>Em caso de sucesso retorna status 200 OK.
        /// Em caso de erro retorna um objeto do tipo BadRequestErrorMessageResult com a mensagem de erro<</returns>
        public async Task<IHttpActionResult> Post(IEnumerable<GameResult> games)
        {
            try
            {
                await Task.Run(() => new GameResultPersist().Submit(games.ToArray()));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
