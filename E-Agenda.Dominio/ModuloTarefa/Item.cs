using System.ComponentModel.DataAnnotations;

namespace E_Agenda.Dominio.ModuloTarefa;

public class Item   //value object
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public statusItem status { get; set; }

    public Item() {}
    public Item(string titulo) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        status = statusItem.Pendente;
    }

    public void Concluir()
    {
        status = statusItem.Concluido;
    }

    public void Reabrir()
    {
        status = statusItem.Pendente;
    }

    public enum statusItem
    {
        [Display(Name = "Pendente")]
        Pendente,

        [Display(Name = "Concluído")]
        Concluido
    }
}