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
           var sqlSelecionarTodos=
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

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitorCompromisso = comandoSelecao.ExecuteReader();

           var compromissos = new List<Compromisso>();

            while (leitorCompromisso.Read()) 
            {
                var compromisso = ConverterParaCompromisso(leitorCompromisso);
                compromissos.Add(compromisso);
            }
            conexaoComBanco.Close();

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
            throw new NotImplementedException();
        }

        public bool Excluir(Guid idRegistro)
        {
            throw new NotImplementedException();
        }

        public List<Compromisso> SelecionarRegistro()
        {
            throw new NotImplementedException();
        }

        public Compromisso? SelecionarRegistroPorId(Guid idRegistro)
        {
            throw new NotImplementedException();
        }
    }
}
