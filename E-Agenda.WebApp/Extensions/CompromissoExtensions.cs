using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Dominio.ModuloContatos;
using E_Agenda.WebApp.Models;

namespace E_Agenda.WebApp.Extensions;

public static class CompromissoExtensions
{
    public static DetalhesCompromissoViewModel ParaVM(this Compromisso c)
    {
        return new DetalhesCompromissoViewModel(
            c.Id,
            c.Assunto,
            c.Data,
            c.HoraInicio,
            c.HoraTermino,
            c.Tipo,
            c.Local,
            c.Contato);
    }

    public static Compromisso ParaEntidade(this FormularioCompromissoViewModel formVm, List<Contato> contatos)
    {
        // Procura pelo primeiro registro na lista que satisfaça a condição. Se não encontrar nenhum, retorna null.
        Contato? contatoSelecionado = contatos.FirstOrDefault(c => c.Id == formVm.ContatoId);

        // Determina Local e Link com base no tipo de compromisso
        string local = formVm.Tipo == TipoCompromisso.Presencial ? formVm.LocalOuLink : "";
        string link = formVm.Tipo == TipoCompromisso.Remoto ? formVm.LocalOuLink : "";

        return new Compromisso(
            formVm.Assunto,
            formVm.DataOcorrencia,
            formVm.HoraInicio,
            formVm.HoraTermino,
            formVm.Tipo == TipoCompromisso.Remoto,
            local,
            link,
            contatoSelecionado
        );
    }
}
