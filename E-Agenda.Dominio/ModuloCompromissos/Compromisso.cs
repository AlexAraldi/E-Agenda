using E_Agenda.Dominio.Compartilhado;
using E_Agenda.Dominio.ModuloContatos;

namespace E_Agenda.Dominio.ModuloCompromissos;
public class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; }
    public DateOnly Data { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }
    public string Local { get; set; }
    public string Link { get; set; }
    public Contato? Contato { get; set; } = null;

    public Compromisso() { }
    public Compromisso(string assunto, DateOnly dataOcorrencia, TimeOnly inicio, TimeOnly termino, bool ehRemoto, string local, string link, Contato? contato) : this()
    {
        Id = Guid.NewGuid();
        Assunto = assunto;
        Data = dataOcorrencia;
        HoraInicio = inicio;
        HoraTermino = termino;
        Tipo = ehRemoto ? TipoCompromisso.Remoto : TipoCompromisso.Presencial;
        Local = local;
        Link = link;
        Contato = contato;
    }

    public override void Atualizar(Compromisso registroEditado)
    {
        Assunto = registroEditado.Assunto;
        Data = registroEditado.Data;
        HoraInicio = registroEditado.HoraInicio;
        HoraTermino = registroEditado.HoraTermino;
        Tipo = registroEditado.Tipo;
        Local = registroEditado.Local;
        Link = registroEditado.Link;
        Contato = registroEditado.Contato;
    }
}

public enum TipoCompromisso
{
    Presencial,
    Remoto
}
