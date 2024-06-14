using Questao5.Infrastructure.Models;
using static Questao5.Infrastructure.Services.ContaBancariaService;
using System.Transactions;
using Microsoft.AspNetCore.Authentication;
using Questao5.Infrastructure.Sqlite;
using Microsoft.Data.Sqlite;


namespace Questao5.Infrastructure.Services
{
    public class ContaBancariaService
    {
        public interface IContaBancariaService
        {
            Task<string> ProcessarMovimentacaoAsync(Movimentacao movimentacao);
        }

        public class ContaService : IContaBancariaService
        {
            private readonly string _connectionString;

            public ContaService(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            public async Task<string> ProcessarMovimentacaoAsync(Movimentacao movimentacao)
            {
                using var contexto = new SqliteConnection(_connectionString);
                await contexto.OpenAsync();

                // Verifica se a conta existe e está ativa
                var selectconta = new SqliteCommand("SELECT ativo FROM contacorrente WHERE idcontacorrente = @IdContaCorrente", contexto);
                selectconta.Parameters.AddWithValue("@IdContaCorrente", movimentacao.IdContaCorrente);

                var reader = await selectconta.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return "INVALID_ACCOUNT";

                await reader.ReadAsync();
                bool isActive = reader.GetBoolean(0);
                reader.Close();

                if (!isActive)
                    return "INACTIVE_ACCOUNT";

                if (movimentacao.Valor <= 0)
                    return "INVALID_VALUE";

                if (movimentacao.TipoMovimento != 'C' && movimentacao.TipoMovimento != 'D')
                    return "INVALID_TYPE";

                // Insere o movimento
                var query = new SqliteCommand("INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)", contexto);
                query.Parameters.AddWithValue("@IdMovimento", Guid.NewGuid().ToString());
                query.Parameters.AddWithValue("@IdContaCorrente", movimentacao.IdContaCorrente);
                query.Parameters.AddWithValue("@DataMovimento", DateTime.Now.ToString("dd/MM/yyyy"));
                query.Parameters.AddWithValue("@TipoMovimento", movimentacao.TipoMovimento);
                query.Parameters.AddWithValue("@Valor", movimentacao.Valor);

                await query.ExecuteNonQueryAsync();

                return "Movimentação foi um sucesso.";
            }
        }
    }
}
