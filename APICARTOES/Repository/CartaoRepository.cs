using APICARTOES.Models;
using System.Net.Http.Headers;
using APICARTOES.Controllers;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace APICARTOES.Repository
{
    public class CartaoRepository : IRepository<Cartao>
    {
        private readonly MySqlDbContext _dbContext;


        public CartaoRepository(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ObterPorNumero(string Cartao)
        {
            bool sucesso = false;
            try
            {
                using (MySqlCommand cmd = _dbContext.GetConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT Numero FROM Cartao WHERE Numero = @Cartao";
                    cmd.Parameters.AddWithValue("@Cartao", Cartao);

                    using (var dr = cmd.ExecuteReader()) // Garante que o DataReader será fechado
                    {
                        if (dr.Read())
                        {
                            sucesso = true;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw; // Aqui você pode adicionar logging para capturar o erro
            }

            return sucesso;
        }

        public bool ObterCartaoValido(String Cartao)
        {
            bool sucesso = false;
            try
            {

                using (MySqlCommand cmd = _dbContext.GetConnection().CreateCommand())
                {
                    cmd.CommandText = @$"SELECT Validade, Numero FROM Cartao WHERE Numero = @Cartao";
                    cmd.Parameters.AddWithValue("Cartao", Cartao);
                    var dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        var validade = dr.GetDateTime("Validade"); // Pega a data corretamente

                        if (validade < DateTime.Now)
                        {
                            sucesso = false;
                        }
                        else
                        {
                            sucesso = true;
                        }
                        dr.Close();
                    }
                   
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
