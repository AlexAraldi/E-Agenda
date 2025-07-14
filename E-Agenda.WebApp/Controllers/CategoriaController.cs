using E_Agenda.Dominio.ModuloCategorias;
using E_Agenda.Infraestrutura.Compartilhado;
using E_Agenda.Infraestrutura.ModuloCategorias;
using E_Agenda.WebApp.ActionFilters;
using E_Agenda.WebApp.Extensions;
using E_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Agenda.WebApp.Controllers;

[Route("categorias")]
[ValidarModelo]
public class CategoriaController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioCategoria repositorioCategoria;

    public CategoriaController(ContextoDados contextoDados, IRepositorioCategoria repositorioCategoria )
    {
        this.contextoDados = contextoDados;
        this.repositorioCategoria = repositorioCategoria;
    }
    public IActionResult Index()
    {
        ViewBag.Title = "Categorias";
        ViewBag.Header = "Visualizando Categorias";

        var registros = repositorioCategoria.ObterTodos();

        var visualizarVM = new VisualizarCategoriaViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        ViewBag.Title = "Categorias | Cadastrar";
        ViewBag.Header = "Cadastro de Categoria";

        var cadastrarVM = new CadastrarCategoriaViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarCategoriaViewModel cadastrarVM)
    {
        ViewBag.Title = "Categorias | Cadastrar";
        ViewBag.Header = "Cadastro de Categoria";

        var registros = repositorioCategoria.ObterTodos();

        foreach (var item in registros)
        {
            if (item.Titulo.Equals(cadastrarVM.Titulo))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma categoria registrada com este título.");
                return View(cadastrarVM);

            }
        }
        var entidade = cadastrarVM.ParaEntidade();

        repositorioCategoria.Cadastrar(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public ActionResult Editar(Guid id)
    {
        ViewBag.Title = "Categorias | Editar";
        ViewBag.Header = "Edição de Categoria";

        var registroSelecionado = repositorioCategoria.ObterPorId(id);

        var editarVM = new EditarCategoriaViewModel(id, registroSelecionado.Titulo);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarCategoriaViewModel editarVM)
    {
        ViewBag.Title = "Categorias | Editar";
        ViewBag.Header = "Edição de Categoria";

        var registros = repositorioCategoria.ObterTodos();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Titulo.Equals(editarVM.Titulo))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma categoria registrada com este título.");
                return View(editarVM);
            }
        }

        var entidadeEditada = editarVM.ParaEntidade();

        repositorioCategoria.Editar(id, entidadeEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        ViewBag.Title = "Categorias | Excluir";
        ViewBag.Header = "Exclusão de Categoria";

        var registroSelecionado = repositorioCategoria.ObterPorId(id);

        var excluirVM = new ExcluirCategoriaViewModel(registroSelecionado.Id, registroSelecionado.Titulo);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        ViewBag.Title = "Categorias | Excluir";
        ViewBag.Header = "Exclusão de Categoria";

        var categoriaSelecionada = repositorioCategoria.ObterPorId(id);

        if (categoriaSelecionada.Despesas.Count > 0)
        {
            ModelState.AddModelError("CadastroUnico", "Não é possível excluir uma categoria com despesas vinculadas");

            var excluirVM = new ExcluirCategoriaViewModel(categoriaSelecionada.Id, categoriaSelecionada.Titulo);

            return View("Excluir", excluirVM);
        }

        repositorioCategoria.Excluir(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        ViewBag.Title = "Categorias | Detalhes";
        ViewBag.Header = "Detalhes da Categoria";

        var registroSelecionado = repositorioCategoria.ObterPorId(id);

        var detalhesVM = new DetalhesCategoriaViewModel(id, registroSelecionado.Titulo, registroSelecionado.Despesas);

        return View(detalhesVM);
    }
}
