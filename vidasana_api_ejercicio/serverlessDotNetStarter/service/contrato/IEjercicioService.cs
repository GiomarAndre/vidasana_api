using AwsDotnetCsharp.Entity;
using AwsDotnetCsharp.Providers.Repositories;
using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Providers.Service
{
    public interface IEjercicioService
    {      
        Task<ListaMaestrosResponse> ListaMaestros(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario);
        Task<ListarEjerciciosResponse> ListarEjercicios(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario, string cod_categoria);
        Task<RegistrarEjercicioResponse> RegistrarEjercicio(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario, RegistrarEjercicioRequest request);
        Task<EliminarEjercicioResponse> EliminarEjercicio(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario, int id_ejercicio);
    }
}