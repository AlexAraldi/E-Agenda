using Microsoft.AspNetCore.Mvc;

namespace E_Agenda.WebApp.Controllers;
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Title = "Página Inicial";
        return View(); 
    }
    [HttpGet("erro")]
    public IActionResult Erro()
    {
        ViewBag.Title = "Erro";
        return View();
    }
}
