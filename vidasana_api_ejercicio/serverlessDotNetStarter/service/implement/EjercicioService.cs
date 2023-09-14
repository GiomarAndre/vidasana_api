using AwsDotnetCsharp.Providers.Repositories;
using AwsDotnetCsharp.Entity;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Linq;

namespace AwsDotnetCsharp.Providers.Service
{
    public class EjercicioService : IEjercicioService
    {
        public async Task<ListaMaestrosResponse> ListaMaestros(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario)
        {
            string secreto = await GetSecret();
            var responseService = new ListaMaestrosResponse();
            var listacategorias = _repository.ListaCategorias(secreto, contextLambda);
            if(listacategorias.codigo != 200)
            {
                responseService.codigo = listacategorias.codigo;
                responseService.descripcion = listacategorias.descripcion;
                return responseService;
            }

            var justcategorias = listacategorias.data.Select(x => new
            {
                cod_categoria = x.cod_categoria,
                nombre_categoria = x.nombre_categoria
            }).Distinct().ToList();

            responseService.categorias = justcategorias.Select(cat => new MaestroCategoriaResponse
            {
                cod_categoria = cat.cod_categoria,
                nombre_categoria = cat.nombre_categoria,
                subcategorias = listacategorias.data
                .Where(all => all.cod_categoria == cat.cod_categoria)
                .Select(sub => new MaestroSubcategoriaResponse
                {
                    cod_subcategoria = sub.cod_subcategoria,
                    nombre_subcategoria = sub.nombre_subcategoria
                }).Distinct().ToList()
            }).Distinct().ToList();

            responseService.codigo = 200;
            responseService.descripcion = "Maestros obtenidos correctamente";

            return await Task.FromResult(responseService);
        }
        public async Task<ListarEjerciciosResponse> ListarEjercicios(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario, string cod_categoria)
        {
            string secreto = await GetSecret();
             return await Task.FromResult(_repository.ListarEjercicios(secreto, contextLambda, id_usuario, cod_categoria));
        }
        public async Task<RegistrarEjercicioResponse> RegistrarEjercicio(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario, RegistrarEjercicioRequest request)
        {
             string secreto = await GetSecret();
             return await Task.FromResult(_repository.RegistrarEjercicio(secreto, contextLambda, id_usuario, request));
        }
        public async Task<EliminarEjercicioResponse> EliminarEjercicio(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario, int id_ejercicio)
        {
             string secreto = await GetSecret();
             return await Task.FromResult(_repository.EliminarEjercicio(secreto, contextLambda, id_usuario, id_ejercicio));
        }
        static async Task<string> GetSecret()
        {
            string secretName = Environment.GetEnvironmentVariable("SECRET_NAME_BD");
            string region = Environment.GetEnvironmentVariable("SECRET_NAME_BD_REGION");

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
            };

            GetSecretValueResponse response;

            try
            {
                response = await client.GetSecretValueAsync(request);
            }
            catch (Exception e)
            {
                // For a list of the exceptions thrown, see
                // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
                throw e;
            }

            var secret = JsonConvert.DeserializeObject<SecretStringResponse>(response.SecretString);

            return secret.connection_vidasana;
            // Your code goes here
        }
        void LogMessage(ILambdaContext ctx, string msg)
        {
            ctx.Logger.LogLine(
                string.Format("{0}:{1} - {2}",
                    ctx.AwsRequestId,
                    ctx.FunctionName,
                    msg));
        }
    }

}