using AwsDotnetCsharp.Entity;
using AwsDotnetCsharp.Providers.Repositories;
using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Providers.Service
{
    public interface IEjercicioService
    {      
        Task<ListaMaestrosResponse> ListaMaestros(ILambdaContext contextLambda, IEjercicioRepository _repository);
        Task<RegistrarEjercicioResponse> RegistrarEjercicio(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario, RegistrarEjercicioRequest request);
    }
}