using E_Agenda.Dominio.ModuloTarefa;
using E_Agenda.Infraestrutura.Compartilhado;

namespace E_Agenda.Infraestrutura.ModuloTarefa;
public class RepositorioTarefa : RepositorioBaseEmArquivos<Tarefa>, IRepositorioTarefa
{
    public RepositorioTarefa(ContextoDados contexto) : base(contexto) { }

    public List<Tarefa> ObterTarefasConcluidas()
    {
        return ObterRegistros().Where(t => t.StatusTarefa == Tarefa.Status.Concluida).ToList();
    }

    public List<Tarefa> ObterTarefasPendentes()
    {
        return ObterRegistros().Where(t => t.StatusTarefa == Tarefa.Status.Pendente).ToList();
    }

    public List<Tarefa> ObterTarefasPorPrioridade(Tarefa.Prioridade prioridade)
    {
        return ObterRegistros().Where(t => t.NivelPrioridade == prioridade).ToList();
    }

    protected override List<Tarefa> ObterRegistros()
    {
        return Contexto.Tarefas;
    }
}
