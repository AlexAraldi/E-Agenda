using System.Data;
using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Dominio.ModuloContatos;
using Microsoft.Data.SqlClient;

namespace E_Agenda.Infraestrutura.SqlServer.ModuloCompromisso
{
    public class RepositorioCompromissoSql : IRepositorioCompromisso
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDb;Integrated Security=True";

        public void Cadastrar(Compromisso novoRegistro)
        {
            var sqlInserir =
                @"INSERT INTO [TBCompromisso] 
                    (
                        [Id],
                        [Assunto], 
                        [Data], 
                        [HoraInicio], 
                        [HoraTermino], 
                        [Tipo], 
                        [Local], 
                        [Link], 
                        [Contato_Id]
                    ) 
                    VALUES 
                    (
                        @ID,
                        @ASSUNTO, 
                        @DATA, 
                        @HORAINICIO, 
                        @HORATERMINO, 
                        @TIPO, 
                        @LOCAL, 
                        @LINK, 
                        @CONTATO_ID
                    );";

            SqlConnection conexaoComBanco = new SqlConnection(connectionString);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosCompromisso(novoRegistro, comandoInsercao);

            conexaoComBanco.Open();

            comandoInsercao.ExecuteNonQuery();

            conexaoComBanco.Close();

        }
        private Contato ConverterParaContato(SqlDataReader leitor)
        {
            var contato = new Contato(
                     Convert.ToString(leitor["Nome"])!,
                     Convert.ToString(leitor["Email"])!,
                     Convert.ToString(leitor["Telefone"])!,
                     Convert.ToString(leitor["Empresa"])!,
                     Convert.ToString(leitor["Cargo"])
                    );

            contato.Id = Guid.Parse(leitor["Id"].ToString()!);
            return contato;
        }
        public bool Editar(Guid idRegistro, Compromisso registroEditado)
        {
            var sqlEditar =
                @"UPDATE [TBCompromisso]
                    SET
                        [Assunto] = @ASSUNTO,
                        [Data] = @DATA,
                        [HoraInicio] = @HORAINICIO,
                        [HoraTermino] = @HORATERMINO,
                        [Tipo] = @TIPO,
                        [Local] = @LOCAL,
                        [Link] = @LINK,
                        [Contato_Id] = @CONTATO_ID
                    WHERE
                        [Id] = @ID";
            SqlConnection conexaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);
            registroEditado.Id = idRegistro;
            ConfigurarParametrosCompromisso(registroEditado, comandoEdicao);
            // abre a conexão com o banco de dados
            conexaoComBanco.Open();
            // executa o comando e obtém o número de registros afetados
            var registrosAfetados = comandoEdicao.ExecuteNonQuery();
            // fecha a conexão com o banco de dados
            conexaoComBanco.Close();

            return registrosAfetados > 0; // Retorna true se pelo menos um registro foi afetado
        }
        public bool Excluir(Guid idRegistro)
        {
            var sqlExcluir =
                 @"DELETE FROM [TBCompromisso]
                    WHERE
                        [Id] = @ID";
            SqlConnection conexaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);
            comandoExclusao.Parameters.AddWithValue("ID", idRegistro);
            conexaoComBanco.Open();
            var registrosAfetados = comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
            return registrosAfetados > 0; // Retorna true se pelo menos um registro foi afetado
        }
        public List<Compromisso> SelecionarRegistro()
        {
            var sqlSelecionarRegistro =
               @"SELECT
                    COMP.[Id], 
	                COMP.[Assunto], 
	                COMP.[Data],
	                COMP.[HoraInicio], 
	                COMP.[HoraTermino],
	                COMP.[Tipo],
	                COMP.[Local],
	                COMP.[Link],
	                COMP.[Contato_Id],
	                CT.[Nome],
	                CT.[Email],
	                CT.[Telefone],
	                CT.[Cargo],
	                CT.[Empresa]
               FROM
                    [TBCompromisso] as COMP LEFT JOIN
                    [TBContato] as CT
               ON
                    CT.[Id] = COMP.Contato_Id;";


            SqlConnection conexaoComBanco = new SqlConnection(connectionString);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarRegistro, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();

            var compromissos = new List<Compromisso>();

            while (leitorCompromisso.Read())
            {
                var compromisso = ConverterParaCompromisso(leitorCompromisso);
                compromissos.Add(compromisso);
            }
            conexaoComBanco.Close();
            return compromissos;
        }
        private Compromisso ConverterParaCompromisso(SqlDataReader leitorCompromisso)
        {
            var horaInicio = TimeOnly.FromTimeSpan(TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HoraInicio"])));
            var horaTermino = TimeOnly.FromTimeSpan(TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HoraTermino"])));
            Contato? contato = null;

            if (!leitorCompromisso["Contato_Id"].Equals(DBNull.Value))
                contato = ConverterParaContato(leitorCompromisso);

            var tipo = (TipoCompromisso)Convert.ToInt32(leitorCompromisso["Tipo"]);
            var compromisso = new Compromisso(
                Convert.ToString(leitorCompromisso["Assunto"]),
                DateOnly.FromDateTime(Convert.ToDateTime(leitorCompromisso["Data"])),
                horaInicio,
                horaTermino,
                tipo == TipoCompromisso.Remoto, // Passa bool baseado no tipo
                Convert.ToString(leitorCompromisso["Local"]),
                Convert.ToString(leitorCompromisso["Link"]),
                contato);

            compromisso.Id = Guid.Parse(leitorCompromisso["Id"].ToString()!);
            return compromisso;
        }
        public Compromisso? SelecionarRegistroPorId(Guid idRegistro)
        {
           var sqlSelecionarPorId =
                @"SELECT
                    COMP.[Id], 
                    COMP.[Assunto], 
                    COMP.[Data],
                    COMP.[HoraInicio], 
                    COMP.[HoraTermino],
                    COMP.[Tipo],
                    COMP.[Local],
                    COMP.[Link],
                    COMP.[Contato_Id],
                    CT.[Nome],
                    CT.[Email],
                    CT.[Telefone],
                    CT.[Cargo],
                    CT.[Empresa]
               FROM
                    [TBCompromisso] as COMP LEFT JOIN
                    [TBContato] as CT
               ON
                    CT.[Id] = COMP.Contato_Id
               WHERE
                    COMP.Id = @ID;";
            SqlConnection conexaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);
            comandoSelecao.Parameters.AddWithValue("ID", idRegistro);
            conexaoComBanco.Open();
            SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();
            Compromisso? compromisso = null;
            if (leitorCompromisso.Read())
                compromisso = ConverterParaCompromisso(leitorCompromisso);
            conexaoComBanco.Close();
            return compromisso;

        }
        private void ConfigurarParametrosCompromisso(Compromisso comprimisso, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", comprimisso.Id);
            comando.Parameters.AddWithValue("ASSUNTO", comprimisso.Assunto);
            comando.Parameters.AddWithValue("DATA", comprimisso.Data);
            comando.Parameters.AddWithValue("HORAINICIO", comprimisso.HoraInicio.Ticks);
            comando.Parameters.AddWithValue("HORATERMINO", comprimisso.HoraTermino.Ticks);
            comando.Parameters.AddWithValue("TIPO", (int)comprimisso.Tipo);
            comando.Parameters.AddWithValue("LOCAL", comprimisso.Local ?? (object)DBNull.Value); // aplicado DBNull pois pode ser Null
            comando.Parameters.AddWithValue("LINK", comprimisso.Link ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("CONTATO_ID", comprimisso.Contato?.Id ?? (object)DBNull.Value);

        }
    }
}
