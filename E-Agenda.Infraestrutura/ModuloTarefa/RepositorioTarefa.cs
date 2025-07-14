using E_Agenda.Dominio.ModuloTarefa;
using E_Agenda.Infraestrutura.Compartilhado;

namespace E_Agenda.Infraestrutura.ModuloTarefa;
public class RepositorioTarefa : RepositorioBaseEmArquivos<Tarefa>, IRepositorioTarefa
{
    public RepositorioTarefa(ContextoDados contexto) : base(contexto) { }

    public List<Tarefa> ObterTarefasConcluidas()
    {
        return registros.FindAll(t => !t.Concluido);

    }

    public List<Tarefa> ObterTarefasPendentes()
    {
        return registros.FindAll(t => !t.Concluido);
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
