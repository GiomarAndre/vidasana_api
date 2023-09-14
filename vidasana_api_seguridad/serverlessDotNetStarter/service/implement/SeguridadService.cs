using AwsDotnetCsharp.Providers.Repositories;
using AwsDotnetCsharp.Entity;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace AwsDotnetCsharp.Providers.Service
{
    public class SeguridadService : ISeguridadService
    {

        public async Task<ObtenerUsuarioPreTokenHttpResponse> ObtenerUsuarioPreToken(ILambdaContext contextLambda, ISeguridadRepository _repository, string id_usuario)
        {
            string secreto = await GetSecret();
            return await Task.FromResult(_repository.ObtenerUsuarioPreToken(secreto, contextLambda, id_usuario));
        }
        public async Task<ObtenerUsuarioPreTokenHttpResponse> DataUsuario(ILambdaContext contextLambda, ISeguridadRepository _repository, string id_usuario)
        {
            string secreto = await GetSecret();
            return await Task.FromResult(_repository.ObtenerUsuarioPreToken(secreto, contextLambda, id_usuario));    
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