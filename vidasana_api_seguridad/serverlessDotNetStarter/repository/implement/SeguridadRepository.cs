using AwsDotnetCsharp.Entity;
using System;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace AwsDotnetCsharp.Providers.Repositories
{
    public class SeguridadRepository : ISeguridadRepository
    { 
        public ObtenerUsuarioPreTokenHttpResponse ObtenerUsuarioPreToken(string secreto, ILambdaContext contextLambda, string id_usuario)
        {
            MySqlConnection connection = new MySqlConnection(secreto);
            MySqlDataReader reader;

            try
            {
                var consulta = $"call VIDASANA_SP_SEGURIDAD_OBTENER_USUARIO_PRETOKEN ('" + id_usuario + "')";
                MySqlCommand cmd = new MySqlCommand(consulta, connection);
                connection.Open();
                reader = cmd.ExecuteReader();

                var data = new ObtenerUsuarioPreTokenResponse();
                while (reader.Read())
                {
                    data.id_usuario = reader["id_usuario"].ToString();
                    data.id_rol = Convert.ToInt32(reader["id_rol"]);
                    data.cod_rol = reader["cod_rol"].ToString();
                    data.nombre_rol = reader["nombre_rol"].ToString();
                    data.username = reader["username"].ToString();
                    data.doc_identidad = reader["doc_identidad"].ToString();
                    data.nombres = reader["nombres"].ToString();
                    data.ape_paterno = reader["ape_paterno"].ToString();
                    data.ape_materno = reader["ape_materno"].ToString();
                }
                
                if (data != null)
                {
                    return new ObtenerUsuarioPreTokenHttpResponse()
                    {
                        codigo = 200,
                        descripcion = "Datos obtenidos.",
                        data = data
                    };
                }
                else
                {
                    return new ObtenerUsuarioPreTokenHttpResponse()
                    {
                        codigo = 400,
                        descripcion = "No se obtuvo datos del usuario."
                    };
                }

            }
            catch (Exception ex)
            {
                LogMessage(contextLambda, "Error invocacion bd: " + ex.Message);
                LogMessage(contextLambda, JsonConvert.SerializeObject(ex));
                return new ObtenerUsuarioPreTokenHttpResponse()
                {
                    codigo = 500,
                    descripcion = "Error interno al obtener los datos del usuario."
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