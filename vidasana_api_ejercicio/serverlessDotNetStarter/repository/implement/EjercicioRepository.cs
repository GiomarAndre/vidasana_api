using AwsDotnetCsharp.Entity;
using System;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace AwsDotnetCsharp.Providers.Repositories
{
    public class EjercicioRepository : IEjercicioRepository
    { 
        public ListaCategoriasResponse ListaCategorias(string secreto, ILambdaContext contextLambda)
        {
            MySqlConnection connection = new MySqlConnection(secreto);
            MySqlDataReader reader;

            try
            {
                var consulta = $"call VIDASANA_SP_EJERCICIO_LISTA_CATEGORIAS ()";
                MySqlCommand cmd = new MySqlCommand(consulta, connection);
                connection.Open();
                reader = cmd.ExecuteReader();

                var data = new List<DataCategoriasBD>();                
                while (reader.Read())
                {
                    var item = new DataCategoriasBD();
                    item.cod_categoria = reader["cod_categoria"].ToString();
                    item.nombre_categoria = reader["nombre_categoria"].ToString();
                    item.cod_subcategoria = reader["cod_subcategoria"].ToString();
                    item.nombre_subcategoria = reader["nombre_subcategoria"].ToString();
                    data.Add(item);
                }
                
                if (data != null && data.Count > 0)
                {
                    return new ListaCategoriasResponse()
                    {
                        codigo = 200,
                        descripcion = "Categorias obtenidos.",
                        data = data
                    };
                }
                else
                {
                    return new ListaCategoriasResponse()
                    {
                        codigo = 400,
                        descripcion = "No se obtuvo datos de las categorias."
                    };
                }

            }
            catch (Exception ex)
            {
                LogMessage(contextLambda, "Error invocacion bd: " + ex.Message);
                LogMessage(contextLambda, JsonConvert.SerializeObject(ex));
                return new ListaCategoriasResponse()
                {
                    codigo = 500,
                    descripcion = "Error interno al obtener las categorias."
                };
            }
            finally 
            {
                connection.Close();
            }
        }
        public RegistrarEjercicioResponse RegistrarEjercicio(string secreto, ILambdaContext contextLambda, string id_usuario, RegistrarEjercicioRequest request)
        {
            MySqlConnection connection = new MySqlConnection(secreto);
            MySqlDataReader reader;

            try
            {
                var consulta = $"call VIDASANA_SP_EJERCICIO_REGISTRAR " +
                $"('${request.nombre_ejercicio}', " + 
                $"'${request.cod_subcategoria}', " + 
                $"${request.cantidad_duracion}, " + 
                $"'${request.unidad_medida_duracion}', " + 
                $"${request.series}, " + 
                $"${request.repeticiones}, " + 
                $"${request.cantidad_descanso}, " + 
                $"'${request.unidad_medida_descanso}', " + 
                $"'${request.descripcion}', " + 
                $"'${id_usuario}')";
                MySqlCommand cmd = new MySqlCommand(consulta, connection);
                connection.Open();
                reader = cmd.ExecuteReader();

                var data = new RegistrarEjercicioDataResponse();                
                while (reader.Read())
                {
                    data.id_ejercicio = String.IsNullOrEmpty(reader["id_ejercicio"].ToString()) ? 0 : Convert.ToInt32(reader["id_ejercicio"].ToString());
                    data.cod_ejercicio = reader["cod_ejercicio"].ToString();
                }
                
                if (data != null)
                {
                    return new RegistrarEjercicioResponse()
                    {
                        codigo = 200,
                        descripcion = "Categorias obtenidos.",
                        data = data
                    };
                }
                else
                {
                    return new RegistrarEjercicioResponse()
                    {
                        codigo = 400,
                        descripcion = "No se obtuvo respuesta del registro de ejercicio."
                    };
                }

            }
            catch (Exception ex)
            {
                LogMessage(contextLambda, "Error invocacion bd: " + ex.Message);
                LogMessage(contextLambda, JsonConvert.SerializeObject(ex));
                return new RegistrarEjercicioResponse()
                {
                    codigo = 500,
                    descripcion = "Error interno al registrar ejercicio."
                };
            }
            finally 
            {
                connection.Close();
            }
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