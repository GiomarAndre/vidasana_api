using System;
using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using AwsDotnetCsharp.Providers.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AwsDotnetCsharp.Providers.Service;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using AwsDotnetCsharp.Entity;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(
typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        private readonly IEjercicioService _service;
        private readonly IEjercicioRepository _repository;
        public Handler() : this(null)
        {

        }
        internal Handler(IEjercicioRepository repository = null,
        IEjercicioService service = null)
        {
            Startup.ConfigureServices();

            _repository = repository ?? Startup.Services.GetRequiredService<IEjercicioRepository>();
            _service = service ?? Startup.Services.GetRequiredService<IEjercicioService>();
        }

        public async Task<APIGatewayProxyResponse> MaestroEjercicios(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LogMessage(context, JsonConvert.SerializeObject(request));
            var AuthorizerObject = JObject.Parse(JsonConvert.SerializeObject(request.RequestContext.Authorizer));
            
            var dataResponse = await _service.ListaMaestros(context, _repository, AuthorizerObject["claims"]["userName"].ToString());
            return CreateResponseObject(dataResponse, dataResponse.codigo);
        }
        public async Task<APIGatewayProxyResponse> ListarEjercicios(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LogMessage(context, JsonConvert.SerializeObject(request));
            var AuthorizerObject = JObject.Parse(JsonConvert.SerializeObject(request.RequestContext.Authorizer));
            request.QueryStringParameters.TryGetValue("cod_categoria", out string cod_categoria);

            var dataResponse = await _service.ListarEjercicios(context, _repository, AuthorizerObject["claims"]["userName"].ToString(), cod_categoria);
            return CreateResponseObject(dataResponse, dataResponse.codigo);
        }
        public async Task<APIGatewayProxyResponse> RegistrarEjercicio(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LogMessage(context, JsonConvert.SerializeObject(request));
            RegistrarEjercicioRequest dataRequest = JsonConvert.DeserializeObject<RegistrarEjercicioRequest>(request.Body);
            var AuthorizerObject = JObject.Parse(JsonConvert.SerializeObject(request.RequestContext.Authorizer));
            
            var dataResponse = await _service.RegistrarEjercicio(context, _repository, AuthorizerObject["claims"]["userName"].ToString(), dataRequest);
            return CreateResponseObject(dataResponse, dataResponse.codigo);
        }
        public async Task<APIGatewayProxyResponse> EliminarEjercicio(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LogMessage(context, JsonConvert.SerializeObject(request));
            var AuthorizerObject = JObject.Parse(JsonConvert.SerializeObject(request.RequestContext.Authorizer));
            request.QueryStringParameters.TryGetValue("id_ejercicio", out string str_id_ejercicio);
            var id_ejercicio = Convert.ToInt32(str_id_ejercicio);
            var dataResponse = await _service.EliminarEjercicio(context, _repository, AuthorizerObject["claims"]["userName"].ToString(), id_ejercicio);
            return CreateResponseObject(dataResponse, dataResponse.codigo);
        }
        public async Task<APIGatewayProxyResponse> ListarEjerciciosApp(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LogMessage(context, JsonConvert.SerializeObject(request));
            var dataResponse = await _service.ListarEjerciciosApp(context, _repository,"vidasanaadm");
            return CreateResponseObject(dataResponse, dataResponse.codigo);
        }
        void LogMessage(ILambdaContext ctx, string msg)
        {
            ctx.Logger.LogLine(
                string.Format("{0}:{1} - {2}",
                    ctx.AwsRequestId,
                    ctx.FunctionName,
                    msg));
        }
        APIGatewayProxyResponse CreateResponse(IDictionary<string, string> result, int statusCodeResponse)
        {
            int statusCode = (result != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            if (statusCodeResponse > 0)
            {
                statusCode = statusCodeResponse;
            }

            string body = (result != null) ?
                JsonConvert.SerializeObject(result) : string.Empty;

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };

            return response;
        }
        
        APIGatewayProxyResponse CreateResponseObject(Object result, int statusCodeResponse)
        {
            int statusCode = (result != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            if (statusCodeResponse > 0)
            {
                statusCode = statusCodeResponse;
            }

            string body = (result != null) ?
                JsonConvert.SerializeObject(result) : string.Empty;

            return new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            }; 
        }
        APIGatewayProxyResponse CreateResponseObject(Object result)
        {
            int statusCode = (result != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            string body = (result != null) ?
                JsonConvert.SerializeObject(result) : string.Empty;

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };

            return response;
        }
    }



}

