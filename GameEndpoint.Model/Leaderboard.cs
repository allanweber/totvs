using System;

namespace GameEndpoint.Model
{
    /// <summary>
    /// Dados do líder em pontuação
    /// </summary>
    public class Leaderboard
    {
        /// <summary>
        /// Id do jogador
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Total de pontos em todos os jogos
        /// </summary>
        public long Balance { get; set; }

        /// <summary>
        /// Última atualização de pontuação
        /// </summary>
        public DateTimeOffset LastUpdateDate { get; set; }
    }
}
