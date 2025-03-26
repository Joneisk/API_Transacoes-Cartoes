using APICARTOES.Models;
using MySql.Data.MySqlClient;

namespace APICARTOES.Repository
{
    public class TransacaoRepository
    {
        private readonly MySqlDbContext _dbContext;


        public TransacaoRepository(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Cadastrar(Transacao transacao)
        {
            bool sucesso = false;
            try
            {   
                using (MySqlCommand cmd = _dbContext.GetConnection().CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Transacao (Valor, Cartao, CVV, Parcelas, Situacao)
                                VALUES (@Valor, @Cartao, @CVV, @Parcelas, @Situacao)";

                    cmd.Parameters.AddWithValue("@Valor", transacao.Valor);
                    cmd.Parameters.AddWithValue("@Cartao", transacao.Cartao);
                    cmd.Parameters.AddWithValue("@CVV", transacao.CVV);
                    cmd.Parameters.AddWithValue("@Parcelas", transacao.Parcelas > 0 ? transacao.Parcelas : 1);
                    cmd.Parameters.AddWithValue("@Situacao", transacao.Situacao);

                    // Executa a inserção
                    int linhaAfetadas = cmd.ExecuteNonQuery();
                    transacao.TransacaoId = (int)cmd.LastInsertedId;
                    sucesso = true;
                }
            }
            catch (MySqlException ex)
            {
                throw; 
            }

            return sucesso;
        }


        public int VerificaSituacao(int transacaoId)
        {
            int valor;
            try
            {
                using (MySqlCommand cmd = _dbContext.GetConnection().CreateCommand())
                {
                    cmd.CommandText = $@"Select Situacao from Transacao where TransacaoId = @TransacaoId";
                    cmd.Parameters.AddWithValue("@TransacaoId", transacaoId);
                    var dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        valor = dr.GetInt32("Situacao");
                        dr.Close();
                        return valor;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Alterar(int TransacaoId)
        {
            bool sucesso = false;
            try
            {
                using (MySqlCommand cmd = _dbContext.GetConnection().CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Transacao set Situacao = @Situacao 
                                                Where TransacaoId = @TransacaoId";




                    cmd.Parameters.AddWithValue("@TransacaoId", TransacaoId);
                    cmd.Parameters.AddWithValue("@Situacao", 2);

                    // Executa a inserção
                    int linhaAfetadas = cmd.ExecuteNonQuery();
                    sucesso = true;
                }
            }
            catch (MySqlException ex)
            {
                throw; 
            }

            return sucesso;
        }

        public bool Alterar2(int TransacaoId)
        {
            bool sucesso = false;
            try
            {
                using (MySqlCommand cmd = _dbContext.GetConnection().CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Transacao set Situacao = @Situacao 
                                                Where TransacaoId = @TransacaoId";




                    cmd.Parameters.AddWithValue("@TransacaoId", TransacaoId);
                    cmd.Parameters.AddWithValue("@Situacao", 3);

                    // Executa a inserção
                    int linhaAfetadas = cmd.ExecuteNonQuery();
                    sucesso = true;
                }
            }
            catch (MySqlException ex)
            {
                throw; 
            }

            return sucesso;
        }

    }
}
