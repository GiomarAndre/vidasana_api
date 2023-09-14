using System.Threading.Tasks;
using AwsDotnetCsharp.Entity;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using System.Data;

namespace AwsDotnetCsharp.Providers.Repositories
{
    public interface ISeguridadRepository
    {
        ObtenerUsuarioPreTokenHttpResponse ObtenerUsuarioPreToken(string secreto,ILambdaContext contextLambda, string id_usuario);
    }
}