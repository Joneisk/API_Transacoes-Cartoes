namespace APICARTOES.Repository
{
    public interface IRepository<T> where T : class
    {
        bool ObterPorNumero(string Cartao);
        bool ObterCartaoValido(string Cartao);
        
    }
}
    