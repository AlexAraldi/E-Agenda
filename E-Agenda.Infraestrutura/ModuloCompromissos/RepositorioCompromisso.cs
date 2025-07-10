using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Infraestrutura.Compartilhado;

namespace E_Agenda.Infraestrutura.ModuloCompromissos;
public class RepositorioCompromisso : RepositorioBaseEmArquivos<Compromisso>, IRepositorioCompromisso
{
    public RepositorioCompromisso(ContextoDados contexto) : base(contexto) { }
    protected override List<Compromisso> ObterRegistros()
    {
        return Contexto.Compromissos;
    }
}
