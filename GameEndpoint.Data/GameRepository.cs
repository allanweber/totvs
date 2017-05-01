using Dapper;
using GameEndpoint.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GameEndpoint.Data
{
    /// <summary>
    /// Repositório para acesso aos dados do jogos
    /// </summary>
    public class GameRepository: BaseRepository
    {
        /// <summary>
        /// Construtor abrindo uma nova conexão utilizando a string de conexão Connection do web.config ou app.config
        /// </summary>
        public GameRepository() : base() { }

        /// <summary>
        /// Construtor aproveitando uma conexão existente, desta forma evita abrir e fechar as conexões de vários Repositories
        /// </summary>
        /// <param name="_connection">Objeto connection instanciado</param>
        public GameRepository(IDbConnection _connection) : base(_connection) { }

        /// <summary>
        /// Verifica se um Id de Game existe na tabela
        /// </summary>
        /// <param name="gameId">Id do game</param>
        /// <returns>True ou False</returns>
        public bool Exists(int gameId)
        {
            string sql = "SELECT 1 FROM Game WHERE Id = @gameId";

            return this.connection.Query<bool>(sql, new { gameId }).FirstOrDefault();
        }
    }
}
