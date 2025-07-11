using System.ComponentModel.DataAnnotations;
using E_Agenda.Dominio.Compartilhado;

namespace E_Agenda.Dominio.ModuloTarefa;

public class Tarefa : EntidadeBase<Tarefa>
{
    public string Titulo { get; set; }
    public Prioridade NivelPrioridade { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataConclusao { get; set; }
    public Status StatusTarefa { get; set; }
    public bool Concluido { get; set; } //Adicionado 33:49 v1 a82
    public List<Item> Itens { get; set; }
    public decimal PercentualConcluido 
    {
        get 
        {
            if (Itens.Count == 0)
                return 0;

            int itensConcluidos = Itens.Count(item => item.status == Item.statusItem.Concluido);

            return(decimal)itensConcluidos / Itens.Count * 100;
        }
    }

    public Tarefa() 
    {
        Itens = new List<Item>();
    }

    public Tarefa(string titulo, Prioridade nivelPrioridade): this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        NivelPrioridade = nivelPrioridade;
        DataCriacao = DateTime.Now;
        StatusTarefa = Status.Pendente;
    }

    public override void Atualizar(Tarefa registroEditado)
    {
        Titulo = registroEditado.Titulo;
        NivelPrioridade = registroEditado.NivelPrioridade;
    }

    public void AdicionarItem(string titulo)
    {
        var item = new Item(titulo);

        Itens.Add(item);
    }

    public void RemoverItem(Item item)
    {
        Itens.Remove(item);
    }

    public void ConcluirItem(Item item)
    {
        item.Concluir();
    }

    public Item? ObterItem(Guid idItem)
    {
        foreach (var i in Itens)
        {
            if (idItem.Equals(i.Id))
                return i;
        }

        return null;
    }

    public void ReabrirItem(Item item)
    {
        item.Reabrir();
    }

    public void ConcluirTarefa()
    {
        StatusTarefa = Status.Concluida;
        DataConclusao = DateTime.Now;

        foreach (var item in Itens)
        {
            item.Concluir();
        }
    }

    public enum Prioridade
    {
        [Display(Name = "Baixa")]
        Baixa,
        [Display(Name = "Normal")]
        Normal,
        [Display(Name = "Alta")]
        Alta
    }

    public enum Status
    {
        Pendente,
        Concluida
    }
}