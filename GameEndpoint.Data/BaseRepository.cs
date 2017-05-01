using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace GameEndpoint.Data
{
    /// <summary>
    /// Repositório base com dados de conexão e encerramento de conexão.
    /// Essa classe trabalha com SQLite
    /// </summary>
    public abstract class BaseRepository: IDisposable
    {
        /// <summary>
        /// Objeto de conexão
        /// </summary>
        protected IDbConnection connection = null;

        /// <summary>
        /// Cria uma conexão utilizando a string de conexão Connection do web.config ou app.config
        /// </summary>
        /// <returns>Objeto IDbConnection do client SQLite instanciado</returns>
        public static IDbConnection CreateConnection()
        {
            return CreateConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
        }

        /// <summary>
        /// Cria uma conexão utilizando uma string connection específica
        /// </summary>
        /// <param name="_connectionString">String connection para SQLite apenas</param>
        /// <returns>Objeto IDbConnection do client SQLite instanciado</returns>
        public static IDbConnection CreateConnection(string _connectionString)
        {
            return new SQLiteConnection(_connectionString);
        }

        /// <summary>
        /// Construtor padrão instacia uma conexão pela connection string Connection do web.config ou app.config
        /// Abre uma nova conexão.
        /// </summary>
        public BaseRepository()
        {
            this.connection = BaseRepository.CreateConnection();
            this.connection.Open();
        }

        /// <summary>
        /// Construtor aproveitando uma conexão existente, desta forma evita abrir e fechar as conexões de vários Repositories
        /// </summary>
        /// <param name="_connection">Objeto connection instanciado</param>
        public BaseRepository(IDbConnection _connection)
        {
            if (_connection == null)
                throw new Exception("Connection cannot be null.");

            this.connection = _connection;
            if (this.connection.State == ConnectionState.Closed)
                this.connection.Open();
        }

        /// <summary>
        /// Se existir conexão e essa nãop estiver fechada, fecha a conexão.
        /// </summary>
        public void Dispose()
        {
            if (this.connection != null)
            {
                if (this.connection.State != ConnectionState.Closed)
                    this.connection.Close();
                this.connection.Dispose();
            }
        }
    }
}
