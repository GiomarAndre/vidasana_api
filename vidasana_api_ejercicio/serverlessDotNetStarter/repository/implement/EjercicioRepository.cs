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
                    var item = new DataCategoriasBD
                    {
                        cod_categoria = reader["cod_categoria"].ToString(),
                        nombre_categoria = reader["nombre_categoria"].ToString(),
                        cod_subcategoria = reader["cod_subcategoria"].ToString(),
                        nombre_subcategoria = reader["nombre_subcategoria"].ToString()
                    };
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
        public ListarEjerciciosResponse ListarEjercicios(string secreto, ILambdaContext contextLambda, string id_usuario, string cod_categoria)
        {
            MySqlConnection connection = new MySqlConnection(secreto);
            MySqlDataReader reader;

            try
            {
                var consulta = $"call VIDASANA_SP_EJERCICIO_LISTA_EJERCICIOS ('${cod_categoria}')";
                MySqlCommand cmd = new MySqlCommand(consulta, connection);
                connection.Open();
                reader = cmd.ExecuteReader();

                var data = new List<DataEjercicio>();                
                while (reader.Read())
                {
                    var item = new DataEjercicio
                    {
                        cod_subcategoria = reader["cod_subcategoria"].ToString(),
                        nombre_subcategoria = reader["nombre_subcategoria"].ToString(),
                        id_ejercicio = String.IsNullOrEmpty(reader["id_ejercicio"].ToString()) ? 0 : Convert.ToInt32(reader["id_ejercicio"].ToString()),
                        cod_ejercicio = reader["cod_ejercicio"].ToString(),
                        nombre_ejercicio = reader["nombre_ejercicio"].ToString(),
                        cantidad_duracion = String.IsNullOrEmpty(reader["cantidad_duracion"].ToString()) ? 0 : Convert.ToInt32(reader["cantidad_duracion"].ToString()),
                        unidad_medida_duracion = reader["unidad_medida_duracion"].ToString(),
                        cantidad_descanso = String.IsNullOrEmpty(reader["cantidad_descanso"].ToString()) ? 0 : Convert.ToInt32(reader["cantidad_descanso"].ToString()),
                        unidad_medida_descanso = reader["unidad_medida_descanso"].ToString(),
                        series = String.IsNullOrEmpty(reader["series"].ToString()) ? 0 : Convert.ToInt32(reader["series"].ToString()),
                        repeticiones = String.IsNullOrEmpty(reader["repeticiones"].ToString()) ? 0 : Convert.ToInt32(reader["repeticiones"].ToString()),
                        descripcion = reader["descripcion"].ToString(),
                        estado = String.IsNullOrEmpty(reader["activo"].ToString()) ? false : Convert.ToBoolean(reader["activo"].ToString()),
                        usuario_creacion = reader["usuario_creacion"].ToString(),
                        fecha_creacion = reader["fecha_creacion"].ToString(),
                        usuario_modificacion = reader["usuario_modificacion"].ToString(),
                        fecha_modificacion = reader["fecha_modificacion"].ToString()
                    };
                    data.Add(item);
                }
                
                if (data != null && data.Count > 0)
                {
                    return new ListarEjerciciosResponse()
                    {
                        codigo = 200,
                        descripcion = "Ejercicios obtenidos.",
                        data = data
                    };
                }
                else
                {
                    return new ListarEjerciciosResponse()
                    {
                        codigo = 400,
                        descripcion = "No se obtuvo respuesta del listado de ejercicios."
                    };
                }

            }
            catch (Exception ex)
            {
                LogMessage(contextLambda, "Error invocacion bd: " + ex.Message);
                LogMessage(contextLambda, JsonConvert.SerializeObject(ex));
                return new ListarEjerciciosResponse()
                {
                    codigo = 500,
                    descripcion = "Error interno al liistar ejercicios."
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
                var dcodigo = 200;
                var ddescripcion = "";                
                while (reader.Read())
                {
                    dcodigo = String.IsNullOrEmpty(reader["codigo"].ToString()) ? 400 : Convert.ToInt32(reader["codigo"].ToString());
                    ddescripcion = reader["descripcion"].ToString();
                    data.id_ejercicio = String.IsNullOrEmpty(reader["id_ejercicio"].ToString()) ? 0 : Convert.ToInt32(reader["id_ejercicio"].ToString());
                    data.cod_ejercicio = reader["cod_ejercicio"].ToString();
                }
                
                if (data != null)
                {
                    return new RegistrarEjercicioResponse()
                    {
                        codigo = dcodigo,
                        descripcion = ddescripcion,
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
        public EliminarEjercicioResponse EliminarEjercicio(string secreto, ILambdaContext contextLambda, string id_usuario, int id_ejercicio)
        {
            MySqlConnection connection = new MySqlConnection(secreto);
            MySqlDataReader reader;

            try
            {
                var consulta = $"call VIDASANA_SP_EJERCICIO_ELIMINAR (${id_ejercicio},'${id_usuario}')";
                MySqlCommand cmd = new MySqlCommand(consulta, connection);
                connection.Open();
                reader = cmd.ExecuteReader();

                var data = new EliminarEjercicioResponse();                
                while (reader.Read())
                {
                    data.codigo = String.IsNullOrEmpty(reader["codigo"].ToString()) ? 400 : Convert.ToInt32(reader["codigo"].ToString());
                    data.descripcion = reader["descripcion"].ToString();
                }
                
                if (data != null)
                {
                    return new EliminarEjercicioResponse()
                    {
                        codigo = data.codigo,
                        descripcion = data.descripcion
                    };
                }
                else
                {
                    return new EliminarEjercicioResponse()
                    {
                        codigo = 400,
                        descripcion = "No se obtuvo respuesta al eliminar ejercicio."
                    };
                }

            }
            catch (Exception ex)
            {
                LogMessage(contextLambda, "Error invocacion bd: " + ex.Message);
                LogMessage(contextLambda, JsonConvert.SerializeObject(ex));
                return new EliminarEjercicioResponse()
                {
                    codigo = 500,
                    descripcion = "Error interno al eliminar ejercicio."
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