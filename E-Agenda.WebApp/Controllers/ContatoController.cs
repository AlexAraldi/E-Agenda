using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Dominio.ModuloContatos;
using E_Agenda.Infraestrutura.Compartilhado;
using E_Agenda.Infraestrutura.ModuloCompromissos;
using E_Agenda.Infraestrutura.ModuloContatos;
using E_Agenda.WebApp.Extensions;
using E_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Agenda.WebApp.Controllers
{
    [Route("contatos")]
    public class ContatoController : Controller
    {
        private readonly ContextoDados contextoDados;
        private readonly IRepositorioContato repositorioContato;
        private readonly IRepositorioCompromisso repositoriocompromisso;

        public ContatoController(ContextoDados contextoDados, IRepositorioContato repositorioContato, IRepositorioCompromisso repositorioCompromisso)
        {
            this.contextoDados = contextoDados;
            this.repositorioContato = repositorioContato;
            this.repositoriocompromisso =  repositorioCompromisso;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Contatos";
            ViewBag.Header = "Visualizando Contatos";

            var registros = repositorioContato.ObterTodos();

            var visualizarVM = new VisualizarContatoViewModel(registros);

            return View(visualizarVM);
        }

        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            ViewBag.Title = "Contatos | Cadastrar";
            ViewBag.Header = "Cadastro de Contato";

            var cadastrarVM = new CadastrarContatoViewModel();

            return View(cadastrarVM);
        }


        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(CadastrarContatoViewModel cadastrarVM)
        {
            ViewBag.Title = "Contatos | Cadastrar";
            ViewBag.Header = "Cadastro de Contato";

            var registros = repositorioContato.ObterTodos();

            foreach (var item in registros)
            {
                if (item.Email.Equals(cadastrarVM.Email))
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este email.");
                    break;
                }
                if (item.Telefone.Equals(cadastrarVM.Telefone))
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este telefone.");
                    break;
                }
            }

            if (!ModelState.IsValid)
                return View(cadastrarVM);

            var entidade = cadastrarVM.ParaEntidade();

            repositorioContato.Cadastrar(entidade);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("editar/{id}")]
        public IActionResult Editar(Guid id)
        {
            ViewBag.Title = "Contatos | Editar";
            ViewBag.Header = "Edição de Contato";

            var registros = repositorioContato.ObterPorId(id);

            var editarVM = new EditarContatoViewModel(
                id,
                registros.Nome,
                registros.Email,
                registros.Telefone,
                registros.Cargo,
                registros.Empresa
            );

            return View(editarVM);
        }

        [HttpPost("editar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Guid id, EditarContatoViewModel editarVM)
        {
            ViewBag.Title = "Contatos | Editar";
            ViewBag.Header = "Edição de Contato";

            var registros = repositorioContato.ObterTodos();

            foreach (var item in registros)
            {
                
                if (!item.Id.Equals(id) && item.Email.Equals(editarVM.Email))
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este email.");
                    break;
                }

                if (!item.Id.Equals(id) && item.Telefone.Equals(editarVM.Telefone))
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este telefone.");
                    break;
                }        
            }

            if (!ModelState.IsValid)
                return View(editarVM);

            var entidadeEditada = editarVM.ParaEntidade();

            repositorioContato.Editar(id, entidadeEditada);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("excluir/{id:guid}")]
        public IActionResult Excluir(Guid id)
        {
            ViewBag.Title = "Contatos | Excluir";
            ViewBag.Header = "Exclusão de Contato";

            var registroSelecionado = repositorioContato.ObterPorId(id);

            var excluirVM = new ExcluirContatoViewModel(registroSelecionado.Id, registroSelecionado.Nome);

            return View(excluirVM);
        }

        [HttpPost("excluir/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirConfirmado(Guid id)
        {
            ViewBag.Title = "Contatos | Excluir";
            ViewBag.Header = "Exclusão de Contato";

            var contato = repositorioContato.ObterPorId(id);

            var compromissos = repositoriocompromisso.ObterTodos();

            foreach (var c in compromissos) 
            {
                if(c.Contato.Id == contato.Id)
                {
                    ModelState.AddModelError("Exclusão", "Não é possível excluir uma contato com compromissos vinculados");

                    var excluirVM = new ExcluirContatoViewModel(contato.Id, contato.Nome);

                    return View("Excluir", excluirVM);
                }
            }

            repositorioContato.Excluir(id);

            return RedirectToAction(nameof(Index));
        }
    }
}