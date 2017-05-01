using Dapper;
using GameEndpoint.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GameEndpoint.Data
{
    public class GameResultRepository : BaseRepository
    {
        /// <summary>
        /// Construtor abrindo uma nova conexão utilizando a string de conexão Connection do web.config ou app.config
        /// </summary>
        public GameResultRepository() : base() { }

        /// <summary>
        /// Construtor aproveitando uma conexão existente, desta forma evita abrir e fechar as conexões de vários Repositories
        /// </summary>
        /// <param name="_connection">Objeto connection instanciado</param>
        public GameResultRepository(IDbConnection _connection) : base(_connection) { }

        /// <summary>
        /// Salva em banco a pontuação de um jogador em um jogo
        /// </summary>
        /// <param name="gameResult">Dados da pontuação de um jogador em um jogo</param>
        public void Save(GameResult gameResult)
        {
            //Verifica se o Id do jogo existe
            if (!new GameRepository(this.connection).Exists(gameResult.GameId))
                throw new Exception(string.Format("Game id {0} not exist", gameResult.GameId));

            //Verifica se o Id do player existe
            if (!new PlayerRepository(this.connection).Exists(gameResult.PlayerId))
                throw new Exception(string.Format("Player id {0} not exist", gameResult.GameId));

            //O valor da data deve ser informado
            if (gameResult.Timestamp == DateTime.MinValue)
                throw new Exception("Timestamp need to be informed.");

            //Seleciona o id do GameResult caso exista para um mesmo GameId e PlayerId, mantendo o unicidade do indice.
            gameResult.Id = this.Select(gameResult.GameId, gameResult.PlayerId);

            //Se o id existe então atualiza a linha
            if (gameResult.Id > 0)
                this.update(gameResult);
            //Se não, insere um registro novo
            else
                this.insert(gameResult);
        }

        /// <summary>
        /// Seleciona os 100 melhores pontuadores
        /// </summary>
        /// <returns>Lista de dados do líder em pontuação</returns>
        public IEnumerable<Leaderboard> GetTop100()
        {
            string sql = @"SELECT PlayerId, SUM(Win) Win, Timestamp 
                            FROM GameResult 
                            GROUP BY PlayerId
                            ORDER BY Win DESC
                            ,DATETIME(Timestamp) DESC 
                            LIMIT 100";

            return this.connection.Query<dynamic>(sql).Select
                                                        (result => new Leaderboard
                                                        {
                                                            PlayerId = int.Parse(result.PlayerId.ToString()),
                                                            Balance = long.Parse(result.Win.ToString()),
                                                            LastUpdateDate = DateTimeOffset.Parse(result.Timestamp.ToString()).ToLocalTime()
                                                        });
        }

        /// <summary>
        /// Atualiza os dados de uma linha de GameResult
        /// </summary>
        /// <param name="gameResult">Pontuação de um jogador em um jogo</param>
        private void update(GameResult gameResult)
        {
            string sql = "UPDATE GameResult SET Win = Win + @Win, Timestamp = @Timestamp WHERE Id = @Id";

            this.connection.Execute(sql, new { gameResult.Win, gameResult.Timestamp, gameResult.Id });
        }

        /// <summary>
        /// Adiciona uma linha de GameResult
        /// </summary>
        /// <param name="gameResult">Pontuação de um jogador em um jogo</param>
        private void insert(GameResult gameResult)
        {
            string sql = @"INSERT INTO GameResult(PlayerId, GameId, Win, Timestamp)
                        VALUES (@PlayerId, @GameId, @Win, @Timestamp)";

            this.connection.Execute(sql,
                new { gameResult.PlayerId, gameResult.GameId, gameResult.Win, gameResult.Timestamp });
        }

        /// <summary>
        /// Seleciona uma linha para manter o indice único de GameId e Player Id
        /// </summary>
        /// <param name="gameId">Id do game</param>
        /// <param name="playerId">Id do player</param>
        /// <returns>Id de GameResult caso exista ou 0 (zero)</returns>
        public int Select(int gameId, int playerId)
        {
            string sql = "SELECT ID FROM GameResult WHERE GameId = @gameId AND PlayerId = @playerId";

            return this.connection.Query<int>(sql, new { gameId, playerId }).FirstOrDefault();
        }
    }
}
