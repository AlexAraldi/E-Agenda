using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Dominio.ModuloContatos;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Agenda.Infraestrutura.SqlServer.ModuloCompromisso
{
    class RepositorioCompromissoSql : IRepositorioCompromisso
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
            var horaInicio = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HoraInicio"]));
            var horaTermino = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HoraTermino"]));
            Contato? contato = null;

            if (!leitorCompromisso["Contato_Id"].Equals(DBNull.Value))
               contato = ConverterParaContato(leitorCompromisso);            

            var compromisso = new Compromisso(
                Convert.ToString(leitorCompromisso["Assunto"]),
                Convert.ToDateTime(leitorCompromisso["Data"]),
                horaInicio,
                horaTermino,
               (TipoCompromisso)Convert.ToInt32(leitorCompromisso["Tipo"]),
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
