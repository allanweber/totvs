
using System;

namespace GameEndpoint.Model
{
    /// <summary>
    /// Pontuação de um jogador em um jogo
    /// </summary>
    public class GameResult
    {
        public int Id { get; set; }

        /// <summary>
        /// Id do jogador
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Id do jogo
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Pontos recebidos
        /// </summary>
        public long Win { get; set; }

        /// <summary>
        /// Data hora e timezone da obtenção dos pontos
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
