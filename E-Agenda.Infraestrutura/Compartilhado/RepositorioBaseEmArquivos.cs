using E_Agenda.Dominio.Compartilhado;
using Microsoft.Win32;

namespace E_Agenda.Infraestrutura.Compartilhado;
public abstract class RepositorioBaseEmArquivos<T> where T : EntidadeBase<T>
{
    protected ContextoDados Contexto;
    protected List<T> registros = new List<T>();

    protected RepositorioBaseEmArquivos(ContextoDados contexto)
    {
        this.Contexto = contexto;

        registros = ObterRegistros();
    }

    protected abstract List<T> ObterRegistros();

    public void Cadastrar(T novoRegistro)
    {
        registros.Add(novoRegistro);

        Contexto.Salvar();
    }

    public bool Editar(Guid idRegistro, T registroEditado)
    {
        T? registroSelecionado = ObterPorId(idRegistro);  //Cláusula de guarda

        if (registroSelecionado is null)

            return false;
        registroSelecionado.Atualizar(registroEditado);
        Contexto.Salvar();
        return true;
    }

    public bool Excluir(Guid idRegistro)
    {
        T? registroSelecionado = ObterPorId(idRegistro);

        if (registroSelecionado is null)
        {
            return false;
        }
        registros.Remove(registroSelecionado);

        Contexto.Salvar();

        return true;
    }

    public List<T> ObterTodos()
    {
        return registros;
    }

    public T? ObterPorId(Guid idRegistro)
    {
        return registros.Find((x) => x.Id.Equals(idRegistro));
    }

}
