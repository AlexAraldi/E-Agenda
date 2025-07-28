using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Dominio.ModuloContatos;
using E_Agenda.WebApp.Extensions;
using E_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Agenda.WebApp.Models;

public class FormularioCompromissoViewModel
{
    [Required(ErrorMessage = "O campo \"Assunto\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Assunto\" precisa ter no mínimo 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Assunto\" precisa ter no máximo 100 caracteres.")]
    public string Assunto { get; set; }

    [Required(ErrorMessage = "O campo \"Data de Ocorrência\" é obrigatório.")]
    public DateOnly DataOcorrencia { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "O campo \"Hora de Início\" é obrigatório.")]
    public TimeOnly HoraInicio { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "O campo \"Hora de Término\" é obrigatório.")]
    public TimeOnly HoraTermino { get; set; } = TimeOnly.FromDateTime(DateTime.Now).AddHours(1);

    [Required(ErrorMessage = "O campo \"Tipo do Compromisso\" é obrigatório.")]
    public TipoCompromisso Tipo { get; set; }

    [Required(ErrorMessage = "O campo \"Local ou Link\" é obrigatório.")]
    public string LocalOuLink { get; set; } = null;

    public Guid? ContatoId { get; set; } = null;

    public List<SelecionarContatoViewModel> ContatosDisponiveis { get; set; }

    protected FormularioCompromissoViewModel()
    {
        ContatosDisponiveis = new();
    }
}

public class CadastrarCompromissoViewModel : FormularioCompromissoViewModel
{
    public CadastrarCompromissoViewModel() { }

    public CadastrarCompromissoViewModel(List<Contato> contatos) : this()
    {
        foreach (Contato c in contatos)
        {
            var selecionarVM = new SelecionarContatoViewModel(c.Id, c.Nome);
            ContatosDisponiveis.Add(selecionarVM);
        }
    }
}

public class EditarCompromissoViewModel : FormularioCompromissoViewModel
{
    public Guid Id { get; set; }
    public EditarCompromissoViewModel() { }

    public EditarCompromissoViewModel(Guid id, string assunto, DateOnly dataOcorrencia, TimeOnly inicio, TimeOnly termino, TipoCompromisso tipo, string localOuLink, Guid? contatoId, List<Contato> contatos) : this()
    {
        Id = id;
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = inicio;
        HoraTermino = termino;
        Tipo = tipo;
        LocalOuLink = localOuLink;
        ContatoId = contatoId;

        foreach (Contato c in contatos)
        {
            var selecionarVM = new SelecionarContatoViewModel(c.Id, c.Nome);
            ContatosDisponiveis.Add(selecionarVM);
        }
    }
}
public class ExcluirCompromissoViewModel
{
    public Guid Id { get; set; }
    public string Assunto { get; set; }
    public ExcluirCompromissoViewModel(Guid id, string assunto)
    {
        Id = id;
        Assunto = assunto;
    }
}

public class VisualizarCompromissoViewModel
{
    public List<DetalhesCompromissoViewModel> Registros { get; set; }

    public VisualizarCompromissoViewModel(List<Compromisso> registros)
    {
        Registros = new();

        foreach (var c in registros)
            Registros.Add(c.ParaVM());
    }
}

public class DetalhesCompromissoViewModel
{
    public Guid Id { get; set; }
    public string Assunto { get; set; }
    public DateOnly DataOcorrencia { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }
    public string LocalOuLink { get; set; }
    public string? Contato { get; set; } = null;

    public DetalhesCompromissoViewModel(Guid id, string assunto, DateOnly dataOcorrencia, TimeOnly inicio, TimeOnly termino, TipoCompromisso tipo, string localOuLink, Contato? contato)
    {
        Id = id;
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = inicio;
        HoraTermino = termino;
        Tipo = tipo;
        LocalOuLink = localOuLink;
        Contato = contato?.Nome;
    }
}