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


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(
typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        private readonly ISeguridadService _service;
        private readonly ISeguridadRepository _repository;
        public Handler() : this(null)
        {

        }
        internal Handler(ISeguridadRepository repository = null,
        ISeguridadService service = null)
        {
            Startup.ConfigureServices();

            _repository = repository ?? Startup.Services.GetRequiredService<ISeguridadRepository>();
            _service = service ?? Startup.Services.GetRequiredService<ISeguridadService>();
        }

        public async Task<JObject> PreToken(JObject request, ILambdaContext context)
        {
            LogMessage(context, "Datos entrada: " + JsonConvert.SerializeObject(request));
            var datosUsuario = await _service.ObtenerUsuarioPreToken(context, _repository, request["request"]["userAttributes"]["sub"].ToString());
            LogMessage(context, "data usuario" + JsonConvert.SerializeObject(datosUsuario));

            if (datosUsuario.codigo == 200) {
               request["response"]["claimsOverrideDetails"] = JObject.Parse(JsonConvert.SerializeObject(new { claimsToAddOrOverride = new {
                    userName = request["userName"],
                    cod_rol = datosUsuario.data.cod_rol,
                    nombre_rol = datosUsuario.data.nombre_rol,
                    nombre_usuario = datosUsuario.data.nombres + " " + datosUsuario.data.ape_paterno
                }}));
            }
            LogMessage(context, "Datos salida: - " + JsonConvert.SerializeObject(request)); 

            return request;
        }
        public async Task<APIGatewayProxyResponse> DataUsuario(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LogMessage(context, JsonConvert.SerializeObject(request));
            request.QueryStringParameters.TryGetValue("id_usuario", out string id_usuario);
            var responseService = await _service.DataUsuario(context, _repository, id_usuario);

            return CreateResponseObject(responseService, 200);
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
            }; ;
        }

    }



}

