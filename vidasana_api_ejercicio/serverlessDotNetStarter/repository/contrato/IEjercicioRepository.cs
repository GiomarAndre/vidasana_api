using AwsDotnetCsharp.Entity;
using Amazon.Lambda.Core;

namespace AwsDotnetCsharp.Providers.Repositories
{
    public interface IEjercicioRepository
    {
        ListaCategoriasResponse ListaCategorias(string secreto, ILambdaContext contextLambda);
        RegistrarEjercicioResponse RegistrarEjercicio(string secreto, ILambdaContext contextLambda, string id_usuario, RegistrarEjercicioRequest request);
    }
}