using E_Agenda.Dominio.ModuloDespesas;
using E_Agenda.Infraestrutura.Compartilhado;


namespace E_Agenda.Infraestrutura.ModuloDespesas;
public class RepositorioDespesa : RepositorioBaseEmArquivos<Despesa>, IRepositorioDespesa
{
    public RepositorioDespesa(ContextoDados contexto) : base(contexto) { }

    protected override List<Despesa> ObterRegistros()
    {
        return Contexto.Despesas;
    }
}
