using AwsDotnetCsharp.Entity;
using AwsDotnetCsharp.Providers.Repositories;
using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Providers.Service
{
    public interface ISeguridadService
    {
        Task<ObtenerUsuarioPreTokenHttpResponse> ObtenerUsuarioPreToken(ILambdaContext contextLambda, ISeguridadRepository _repository, string id_usuario);        
        Task<ObtenerUsuarioPreTokenHttpResponse> DataUsuario(ILambdaContext contextLambda, ISeguridadRepository _repository, string id_usuario);
    }
}