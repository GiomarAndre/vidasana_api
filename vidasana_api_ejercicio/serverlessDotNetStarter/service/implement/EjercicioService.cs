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
        public async Task<ListarEjerciciosAppResponse> ListarEjerciciosApp(ILambdaContext contextLambda, IEjercicioRepository _repository, string id_usuario)
        {
            string secreto = await GetSecret();
            var responseService = new ListarEjerciciosAppResponse();
            var lista_all = _repository.ListarEjerciciosApp(secreto, contextLambda);
            if(lista_all.codigo != 200)
            {
                responseService.codigo = lista_all.codigo;
                responseService.descripcion = lista_all.descripcion;
                return responseService;
            }

            var justcategorias = lista_all.data.Select(x => new
            {
                cod_categoria = x.cod_categoria,
                nombre_categoria = x.nombre_categoria
            }).Distinct().ToList();

            var categorias_subcategorias = lista_all.data.Select(x => new
            {
                cod_categoria = x.cod_categoria,
                cod_subcategoria = x.cod_subcategoria,
                nombre_subcategoria = x.nombre_subcategoria
            }).Distinct().ToList();

            var subcategorias_ejercicios = lista_all.data.Select(x => new
            {
                cod_subcategoria = x.cod_subcategoria,
                id_ejercicio = x.id_ejercicio,
                cod_ejercicio = x.cod_ejercicio,
                nombre_ejercicio = x.nombre_ejercicio,
                cantidad_duracion = x.cantidad_duracion,
                unidad_medida_duracion = x.unidad_medida_duracion,
                cantidad_descanso = x.cantidad_descanso,
                unidad_medida_descanso = x.unidad_medida_descanso,
                series = x.series,
                repeticiones = x.repeticiones,
                descripcion = x.descripcion
            }).Distinct().ToList();

            var ejercicios_multimedia = lista_all.data.Where(x => !String.IsNullOrEmpty(x.cod_multimedia_ejercicio)).Select(x => new
            {
                id_ejercicio = x.id_ejercicio,
                cod_multimedia_ejercicio = x.cod_multimedia_ejercicio,
                cod_tipo_multimedia = x.cod_tipo_multimedia,
                nombre_archivo = x.nombre_archivo,
                extension = x.extension,
                url_archivo = x.url_archivo,
                descripcion_multimedia = x.descripcion_multimedia
            }).Distinct().ToList();

            responseService.categorias = justcategorias.Select(cat => new DataAppCategoria
            {
                cod_categoria = cat.cod_categoria,
                nombre_categoria = cat.nombre_categoria,
                subcategorias = categorias_subcategorias
                .Where(all => all.cod_categoria == cat.cod_categoria)
                .Select(sub => new DataAppSubcategoria
                {
                    cod_subcategoria = sub.cod_subcategoria,
                    nombre_subcategoria = sub.nombre_subcategoria,
                    ejercicios =  subcategorias_ejercicios
                    .Where(all => all.cod_subcategoria == sub.cod_subcategoria)
                    .Select(eje => new DataAppEjercicio
                    {
                        id_ejercicio = eje.id_ejercicio,
                        cod_ejercicio = eje.cod_ejercicio,
                        nombre_ejercicio = eje.nombre_ejercicio,
                        cantidad_duracion = eje.cantidad_duracion,
                        unidad_medida_duracion = eje.unidad_medida_duracion,
                        cantidad_descanso = eje.cantidad_descanso,
                        unidad_medida_descanso = eje.unidad_medida_descanso,
                        series = eje.series,
                        repeticiones = eje.repeticiones,
                        descripcion = eje.descripcion,
                        multimedia = ejercicios_multimedia
                        .Where(all => all.id_ejercicio == eje.id_ejercicio && !String.IsNullOrEmpty(all.cod_multimedia_ejercicio))
                        .Select(mul => new DataAppMultimediaEjercicio
                        {
                            cod_multimedia_ejercicio = mul.cod_multimedia_ejercicio,
                            cod_tipo_multimedia = mul.cod_tipo_multimedia,
                            nombre_archivo = mul.nombre_archivo,
                            extension = mul.extension,
                            url_archivo = mul.url_archivo,
                            descripcion_multimedia = mul.descripcion_multimedia
                        }).Distinct().ToList()
                    }).Distinct().ToList()
                }).Distinct().ToList()
            }).Distinct().ToList();


            LogMessage(contextLambda, "justcategorias | " + JsonConvert.SerializeObject(justcategorias));
            LogMessage(contextLambda, "categorias_subcategorias | " +JsonConvert.SerializeObject(categorias_subcategorias));
            LogMessage(contextLambda, "subcategorias_ejercicios | " +JsonConvert.SerializeObject(subcategorias_ejercicios));
            LogMessage(contextLambda, "ejercicios_multimedia | " +JsonConvert.SerializeObject(ejercicios_multimedia));



            responseService.codigo = 200;
            responseService.descripcion = "Ejercicios obtenidos correctamente";

            return await Task.FromResult(responseService);
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