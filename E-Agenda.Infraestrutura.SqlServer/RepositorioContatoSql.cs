using E_Agenda.Dominio.ModuloContatos;
using Microsoft.Data.SqlClient;

namespace E_Agenda.Infraestrutura.SqlServer
{
    public class RepositorioContatoSql : IRepositorioContato
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDb;Integrated Security=True";
        public void Cadastrar(Contato novoRegistro)
        {
            var sqlInserir =
                @"INSERT INTO [TBCONTATO] 
                    (
                        [ID],
                        [NOME], 
                        [EMAIL], 
                        [TELEFONE], 
                        [EMPRESA], 
                        [CARGO]
                    ) 
                    VALUES 
                    (
                        @ID,
                        @NOME, 
                        @EMAIL, 
                        @TELEFONE, 
                        @EMPRESA, 
                        @CARGO
                    );";

            SqlConnection conexaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosContato(novoRegistro, comandoInsercao);
            conexaoComBanco.Open();
            comandoInsercao.ExecuteNonQuery();
            conexaoComBanco.Close();


        }

        public bool Editar(Guid idRegistro, Contato registroEditado)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(Guid idRegistro)
        {
            throw new NotImplementedException();
        }

        public Contato? SelecionarRegistroPorId(Guid idRegistro)
        {
            throw new NotImplementedException();
        }

        public List<Contato> SelecionarRegistro()
        {
            // configura o comando a ser executado
            var sqlSelecionarTodos =
                @"SELECT
                        [ID],
                        [NOME],
                        [EMAIL],
                        [TELEFONE],
                        [EMPRESA],
                        [CARGO]
                    FROM
                        [TBCONTATO]";
            SqlConnection conexaoComBanco = new SqlConnection(connectionString);

            conexaoComBanco.Open();

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            //executar o comando e ler a resposta
            SqlDataReader leitor = comandoSelecao.ExecuteReader();

            var contatos = new List<Contato>();

            while (leitor.Read())
            {
                var contato = ConverterParaContato(leitor);
                contatos.Add(contato);
            }

            conexaoComBanco.Close();

            return contatos;
        }

        private Contato ConverterParaContato(SqlDataReader leitor)
        {
            var contato = new Contato(
                    Convert.ToString(leitor["NOME"])!,
                     Convert.ToString(leitor["EMAIL"])!,
                     Convert.ToString(leitor["TELEFONE"])!,
                     Convert.ToString(leitor["EMPRESA"])!,
                     Convert.ToString(leitor["CARGO"])
                    );

            contato.Id = Guid.Parse(leitor["ID"].ToString()!);

            return contato;

        }
        private void ConfigurarParametrosContato(Contato contato, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", contato.Id);
            comando.Parameters.AddWithValue("NOME", contato.Nome);
            comando.Parameters.AddWithValue("EMAIL", contato.Email);
            comando.Parameters.AddWithValue("TELEFONE", contato.Telefone);
            comando.Parameters.AddWithValue("EMPRESA", contato.Empresa);
            comando.Parameters.AddWithValue("CARGO", contato.Cargo);
        }
    }
}