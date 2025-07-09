using AspNetCoreGeneratedDocument;
using E_Agenda.Dominio.ModuloTarefa;
using E_Agenda.Infraestrutura.Compartilhado;
using E_Agenda.Infraestrutura.ModuloTarefa;
using E_Agenda.WebApp.Extensions;
using E_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using static E_Agenda.Dominio.ModuloTarefa.Item;

namespace E_Agenda.WebApp.Controllers;

[Route("tarefas")]
public class TarefaController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioTarefa repositorioTarefa;


    //inversao de controle
    public TarefaController(ContextoDados contextoDados, IRepositorioTarefa repositorioTarefa)
    {
        this.contextoDados = contextoDados;
        this.repositorioTarefa = repositorioTarefa;
    }

    public IActionResult Index(string status)
    {
        ViewBag.Title = "Tarefas";
        ViewBag.Header = "Visualizando Tarefas";

        List<Tarefa> tarefas;

        switch (status)
        {
            case "pendente": tarefas = repositorioTarefa.ObterTarefasPendentes(); break;
            case "concluida": tarefas = repositorioTarefa.ObterTarefasConcluidas(); break;
            case "alta": tarefas = repositorioTarefa.ObterTarefasPorPrioridade(Tarefa.Prioridade.Alta); break;
            case "normal": tarefas = repositorioTarefa.ObterTarefasPorPrioridade(Tarefa.Prioridade.Normal); break;
            case "baixa": tarefas = repositorioTarefa.ObterTarefasPorPrioridade(Tarefa.Prioridade.Baixa); break;
            default: tarefas = repositorioTarefa.ObterTodos(); break;
        }

        var VisualizarVM = new VisualizarTarefaViewModel(tarefas);

        return View(VisualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        ViewBag.Title = "Tarefas | Cadastrar";
        ViewBag.Header = "Cadastro de Tarefa";

        var CriarTarefaVM = new CriarTarefaViewModel();

        return View(CriarTarefaVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CriarTarefaViewModel CriarTarefaVM)
    {
        ViewBag.Title = "Tarefas | Cadastrar";
        ViewBag.Header = "Cadastro de Tarefa";

        if (!ModelState.IsValid)
        {
            return View(CriarTarefaVM);
        }

        var tarefa = CriarTarefaVM.ParaEntidade();

        repositorioTarefa.Cadastrar(tarefa);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        ViewBag.Title = "Tarefas | Editar";
        ViewBag.Header = "Edição de Tarefa";

        var registroSelecionado = repositorioTarefa.ObterPorId(id);

        var editarVM = new EditarTarefaViewModel(
            registroSelecionado.Id,
            registroSelecionado.Titulo,
            registroSelecionado.NivelPrioridade
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarTarefaViewModel editarVM)
    {
        ViewBag.Title = "Tarefas | Editar";
        ViewBag.Header = "Edição de Tarefa";

        var registros = repositorioTarefa.ObterTodos();

        if (!ModelState.IsValid)
            return View(editarVM);

        var registroEditado = editarVM.ParaEntidade();

        repositorioTarefa.Editar(id, registroEditado);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        ViewBag.Title = "Tarefas | Excluir";
        ViewBag.Header = "Exclusão de Tarefa";

        var registroSelecionado = repositorioTarefa.ObterPorId(id);

        var excluirVM = new ExcluirTarefaViewModel(registroSelecionado.Id, registroSelecionado.Titulo);

        return View(excluirVM);
    }


    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        ViewBag.Title = "Tarefas | Excluir";
        ViewBag.Header = "Exclusão de Tarefa";

        repositorioTarefa.Excluir(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/tarefas/{id:guid}/gerenciar")]
    public IActionResult Gerenciar(Guid id)
    {
        ViewBag.Title = "Tarefas | Gerenciar";
        ViewBag.Header = "Gerenciamento de Tarefa";

        var tarefaSelecionada = repositorioTarefa.ObterPorId(id);

        var gerenciarItensViewModel = new GerenciarTarefaViewModel(tarefaSelecionada);

        return View(gerenciarItensViewModel);
    }

    [HttpPost, Route("/tarefas/{id:guid}/concluir")]
    public IActionResult Concluir(Guid id)
    {
        var tarefaSelecionada = repositorioTarefa.ObterPorId(id);

        tarefaSelecionada.ConcluirTarefa();

        contextoDados.Salvar();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, Route("/tarefas/{id:guid}/adicionar_item")]
    public IActionResult AdicionarItem(Guid id, string titulo)
    {
        var tarefaSelecionada = repositorioTarefa.ObterPorId(id);

        tarefaSelecionada.AdicionarItem(titulo);

        contextoDados.Salvar();

        var gerenciarVM = new GerenciarTarefaViewModel(tarefaSelecionada);

        return View(nameof(Gerenciar), gerenciarVM);
    }

    [HttpPost, Route("/tarefas/{idTarefa:guid}/alternar_status/{idItem:guid}")]
    public IActionResult AlternarStatus(Guid idTarefa, Guid idItem)
    {
        var tarefaSelecionada = repositorioTarefa.ObterPorId(idTarefa);

        var itemSelecionado = tarefaSelecionada.ObterItem(idItem);

        if (itemSelecionado.status == statusItem.Concluido)
            tarefaSelecionada.ReabrirItem(itemSelecionado);
        else
            tarefaSelecionada.ConcluirItem(itemSelecionado);

        contextoDados.Salvar();

        var gerenciarItensViewModel = new GerenciarTarefaViewModel(tarefaSelecionada);

        return View(nameof(Gerenciar), gerenciarItensViewModel);
    }

    [HttpPost, Route("/tarefas/{idTarefa:guid}/remover_item/{idItem:guid}")]
    public IActionResult RemoverItem(Guid idTarefa, Guid idItem)
    {
        var tarefaSelecionada = repositorioTarefa.ObterPorId(idTarefa);

        var itemSelecionado = tarefaSelecionada.ObterItem(idItem);

        tarefaSelecionada.RemoverItem(itemSelecionado);

        contextoDados.Salvar();

        var gerenciarItensViewModel = new GerenciarTarefaViewModel(tarefaSelecionada);

        return View(nameof(Gerenciar), gerenciarItensViewModel);
    }
}
