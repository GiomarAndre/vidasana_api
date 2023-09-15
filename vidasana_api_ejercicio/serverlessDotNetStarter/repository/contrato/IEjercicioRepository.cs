using AwsDotnetCsharp.Entity;
using Amazon.Lambda.Core;

namespace AwsDotnetCsharp.Providers.Repositories
{
    public interface IEjercicioRepository
    {
        ListaCategoriasResponse ListaCategorias(string secreto, ILambdaContext contextLambda);
        ListarEjerciciosResponse ListarEjercicios(string secreto, ILambdaContext contextLambda, string id_usuario, string cod_categoria);
        RegistrarEjercicioResponse RegistrarEjercicio(string secreto, ILambdaContext contextLambda, string id_usuario, RegistrarEjercicioRequest request);
        EliminarEjercicioResponse EliminarEjercicio(string secreto, ILambdaContext contextLambda, string id_usuario, int id_ejercicio);
        ListarEjerciciosAppAllResponse ListarEjerciciosApp(string secreto, ILambdaContext contextLambda);
    }
}