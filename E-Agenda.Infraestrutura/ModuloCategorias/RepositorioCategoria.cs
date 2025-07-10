using E_Agenda.Dominio.ModuloCategorias;
using E_Agenda.Infraestrutura.Compartilhado;

namespace E_Agenda.Infraestrutura.ModuloCategorias;
public class RepositorioCategoria : RepositorioBaseEmArquivos<Categoria>, IRepositorioCategoria
{
    public RepositorioCategoria(ContextoDados contexto) : base(contexto) { }

    protected override List<Categoria> ObterRegistros()
    {
        return Contexto.Categorias;
    }
}
