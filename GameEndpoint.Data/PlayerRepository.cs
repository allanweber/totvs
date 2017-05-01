using Dapper;
using System.Data;
using System.Linq;

namespace GameEndpoint.Data
{
    public class PlayerRepository: BaseRepository
    {
        /// <summary>
        /// Construtor abrindo uma nova conexão utilizando a string de conexão Connection do web.config ou app.config
        /// </summary>
        public PlayerRepository() : base() { }

        /// <summary>
        /// Construtor aproveitando uma conexão existente, desta forma evita abrir e fechar as conexões de vários Repositories
        /// </summary>
        /// <param name="_connection">Objeto connection instanciado</param>
        public PlayerRepository(IDbConnection _connection) : base(_connection) { }

        /// <summary>
        ///  Verifica se um Id de Player existe na tabela
        /// </summary>
        /// <param name="playerId">Id do player</param>
        /// <returns>True ou False</returns>
        public bool Exists(int playerId)
        {
            string sql = "SELECT 1 FROM Player WHERE Id = @playerId";

            return this.connection.Query<bool>(sql, new { playerId }).FirstOrDefault();
        }
    }
}
