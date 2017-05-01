using GameEndpoint.Data;
using GameEndpoint.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GameEndpoint.Business
{
    /// <summary>
    /// Classe responsável por persistir os resultados de jogos do jogadores
    /// </summary>
    public class GameResultPersist : IDisposable
    {
        private IDbConnection connection;
        private GameResultRepository gameResultRepository;

        /// <summary>
        /// Construtor padrão.
        /// Criará uma nova instância da conexão e do Repositório GameResultRepository
        /// </summary>
        public GameResultPersist()
        {
            this.connection = BaseRepository.CreateConnection();
            this.gameResultRepository = new GameResultRepository(this.connection);
        }

        /// <summary>
        /// Grava um objeto de resultado de jogo
        /// </summary>
        /// <param name="gameResult">Pontuação de um jogador em um jogo</param>
        public void Submit(GameResult gameResult)
        {
            gameResultRepository.Save(gameResult);
        }

        /// <summary>
        /// Grava varios objetos de resultados de jogos
        /// </summary>
        /// <param name="gamesResult">Coleção de Pontuação de um jogador em um jogo</param>
        public void Submit(GameResult[] gamesResult)
        {
            //Agrupa antes de gravar
            GameResult[] groupList = this.GroupBy(gamesResult);

            for (int i = 0; i < groupList.Count(); i++)
            {
                this.Submit(groupList[i]);
            }
        }

        /// <summary>
        /// Agrupa os resultados de um mesmo Jogador e jogo, 
        /// assim executando apenas uma requisição à banco para gravar esses dados.
        /// Ao grupar as informações, soma os pontos obtidos e utiliza a maior data da coleção
        /// </summary>
        /// <param name="gamesResult">Coleção de Pontuação de um jogador em um jogo</param>
        /// <returns>Coleção agrupada com a soma dos pontos de um jogador em um jogo</returns>
        public GameResult[] GroupBy(GameResult[] gamesResult)
        {
            GameResult[] groupList = gamesResult.GroupBy(g => new { g.GameId, g.PlayerId })
                        .Select(g => new GameResult
                        {
                            GameId = g.First().GameId,
                            PlayerId = g.First().PlayerId,
                            Timestamp = g.OrderByDescending(date => date.Timestamp).First().Timestamp,
                            Id = g.First().Id,
                            Win = g.Sum(w => w.Win)
                        }).ToArray();

            return groupList;
        }

        /// <summary>
        /// Retorna a lista dos 100 jogadores com mais pontos
        /// </summary>
        /// <returns>Coleção com dados dos líderes em pontuação</returns>
        public IEnumerable<Leaderboard> GetBest()
        {
            using (GameResultRepository gameResultRepository = new GameResultRepository())
            {
                return gameResultRepository.GetTop100();
            }
        }

        public void Dispose()
        {
            if (this.gameResultRepository != null)
            {
                this.gameResultRepository.Dispose();
            }
        }
    }
}
