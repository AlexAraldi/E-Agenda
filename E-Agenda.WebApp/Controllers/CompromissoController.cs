using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Dominio.ModuloContatos;
using E_Agenda.Infraestrutura.Compartilhado;
using E_Agenda.Infraestrutura.ModuloCompromissos;
using E_Agenda.Infraestrutura.ModuloContatos;
using E_Agenda.WebApp.ActionFilters;
using E_Agenda.WebApp.Extensions;
using E_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Agenda.WebApp.Controllers;

[Route("compromissos")]
[ValidarModelo]

public class CompromissoController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioCompromisso repositorioCompromisso;
    private readonly IRepositorioContato repositorioContato;

    public CompromissoController(ContextoDados contextoDados, IRepositorioCompromisso repositorioCompromisso, IRepositorioContato repositorioContato)
    {
        this.contextoDados = contextoDados;
        this.repositorioCompromisso = repositorioCompromisso;
        this.repositorioContato = repositorioContato;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Title = "Compromissos";
        ViewBag.Header = "Visualizando Compromissos";

        var registros = repositorioCompromisso.SelecionarRegistro();

        var visualizarVM = new VisualizarCompromissoViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        ViewBag.Title = "Compromissos | Cadastrar";
        ViewBag.Header = "Cadastro de Compromisso";

        var contatos = repositorioContato.SelecionarRegistro();
        var cadastrarVM = new CadastrarCompromissoViewModel(contatos);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarCompromissoViewModel cadastrarVM)
    {
        ViewBag.Title = "Compromissos | Cadastrar";
        ViewBag.Header = "Cadastro de Compromisso";

        var registros = repositorioCompromisso.SelecionarRegistro();
        var contatos = repositorioContato.SelecionarRegistro();

        if (cadastrarVM.HoraTermino <= cadastrarVM.HoraInicio)
            ModelState.AddModelError("HorarioInvalido", "O horário de término deve ser após o horário de início.");

        if (registros.Any(c => c.DataOcorrencia == cadastrarVM.DataOcorrencia && c.HoraInicio < cadastrarVM.HoraTermino && c.HoraTermino > cadastrarVM.HoraInicio))
            ModelState.AddModelError("CadastroUnico", "Já existe um compromisso no horário selecionado");

        if (!ModelState.IsValid)
        {
            cadastrarVM.ContatosDisponiveis.Clear();
            foreach (var c in contatos)
                cadastrarVM.ContatosDisponiveis.Add(new SelecionarContatoViewModel(c.Id, c.Nome));
            return View(cadastrarVM);
        }

        var entidade = cadastrarVM.ParaEntidade(contatos);

        repositorioCompromisso.Cadastrar(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        ViewBag.Title = "Compromissos | Editar";
        ViewBag.Header = "Edição de Compromisso";

        var registroSelecionado = repositorioCompromisso.SelecionarRegistroPorId(id);

        var contatos = repositorioContato.SelecionarRegistro();

        var editarVM = new EditarCompromissoViewModel(registroSelecionado.Id, registroSelecionado.Assunto, registroSelecionado.DataOcorrencia, registroSelecionado.HoraInicio, registroSelecionado.HoraTermino, registroSelecionado.Tipo, registroSelecionado.LocalOuLink, registroSelecionado.Contato?.Id, contatos);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarCompromissoViewModel editarVM)
    {
        ViewBag.Title = "Compromissos | Editar";
        ViewBag.Header = "Edição de Compromisso";

        var registros = repositorioCompromisso.SelecionarRegistro();
        var contatos = repositorioContato.SelecionarRegistro();

        if (editarVM.HoraTermino <= editarVM.HoraInicio)
            ModelState.AddModelError("HorarioInvalido", "O horário de término deve ser após o horário de início.");

        if (registros.Any(c => c.DataOcorrencia == editarVM.DataOcorrencia && c.HoraInicio < editarVM.HoraTermino && c.HoraTermino > editarVM.HoraInicio && !c.Id.Equals(id)))
            ModelState.AddModelError("CadastroUnico", "Já existe um compromisso no horário selecionado");

        if (!ModelState.IsValid)
        {
            editarVM.ContatosDisponiveis.Clear();
            foreach (var c in contatos)
                editarVM.ContatosDisponiveis.Add(new SelecionarContatoViewModel(c.Id, c.Nome));
            return View(editarVM);
        }

        var entidadeEditada = editarVM.ParaEntidade(contatos);

        repositorioCompromisso.Editar(id, entidadeEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        ViewBag.Title = "Compromissos | Excluir";
        ViewBag.Header = "Exclusão de Compromisso";

        var registroSelecionado = repositorioCompromisso.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirCompromissoViewModel(registroSelecionado.Id, registroSelecionado.Assunto);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        ViewBag.Title = "Compromissos | Excluir";
        ViewBag.Header = "Exclusão de Compromisso";

        repositorioCompromisso.Excluir(id);

        return RedirectToAction(nameof(Index));
    }
}
