using E_Agenda.Dominio.ModuloContatos;
using E_Agenda.Infraestrutura.Compartilhado;

namespace E_Agenda.Infraestrutura.ModuloContatos;

public class RepositorioContato : RepositorioBaseEmArquivos<Contato>, IRepositorioContato
{
    public RepositorioContato(ContextoDados contexto) : base(contexto) { }

    protected override List<Contato> ObterRegistros()
    {
        return Contexto.Contatos;
    }
}