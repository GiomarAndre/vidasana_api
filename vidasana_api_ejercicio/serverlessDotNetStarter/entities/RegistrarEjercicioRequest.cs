using System.Collections.Generic;

namespace AwsDotnetCsharp.Entity
{
    public class RegistrarEjercicioRequest
    {
        public string nombre_ejercicio { get; set; }
        public string cod_subcategoria { get; set; }
        public int cantidad_duracion { get; set; }
        public string unidad_medida_duracion { get; set; }
        public int series { get; set; }
        public int repeticiones { get; set; }
        public int cantidad_descanso { get; set; }
        public string unidad_medida_descanso { get; set; }
        public string descripcion { get; set; }
    }
}