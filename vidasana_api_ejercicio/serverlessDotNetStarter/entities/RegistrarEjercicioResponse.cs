using System.Collections.Generic;
namespace AwsDotnetCsharp.Entity
{
    public class RegistrarEjercicioResponse : GlobalResponse
    {
        public RegistrarEjercicioDataResponse data { get; set; }
    }
    public class RegistrarEjercicioDataResponse
    {
        public int id_ejercicio { get; set; }
        public string cod_ejercicio { get; set; }
    }
}