using System.Collections.Generic;
namespace AwsDotnetCsharp.Entity
{
    public class ListarEjerciciosResponse : GlobalResponse
    {
        public List<DataEjercicio> data { get; set; }
    }
    public class DataEjercicio
    {
        public string cod_subcategoria { get; set; }
        public string nombre_subcategoria { get; set; }
        public int id_ejercicio { get; set; }
        public string cod_ejercicio { get; set; }
        public string nombre_ejercicio { get; set; }
        public int cantidad_duracion { get; set; }
        public string unidad_medida_duracion { get; set; }
        public int cantidad_descanso { get; set; }
        public string unidad_medida_descanso { get; set; }
        public int series { get; set; }
        public int repeticiones { get; set; }
        public string descripcion { get; set; }
        public string usuario_creacion { get; set; }
        public string fecha_creacion { get; set; }
        public string usuario_modificacion { get; set; }
        public string fecha_modificacion { get; set; }
    }
}