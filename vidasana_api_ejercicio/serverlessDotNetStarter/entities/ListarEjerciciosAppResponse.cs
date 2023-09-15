using System.Collections.Generic;
namespace AwsDotnetCsharp.Entity
{
    public class ListarEjerciciosAppResponse : GlobalResponse
    {
        public List<DataAppCategoria> categorias { get; set; }
    }
    public class DataAppCategoria
    {
        public string cod_categoria { get; set; }
        public string nombre_categoria { get; set; }
        public List<DataAppSubcategoria> subcategorias { get; set; }
    }
    public class DataAppSubcategoria
    {
        public string cod_subcategoria { get; set; }
        public string nombre_subcategoria { get; set; }
        public List<DataAppEjercicio> ejercicios { get; set; }
    }
    public class DataAppEjercicio
    {
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
        public List<DataAppMultimediaEjercicio> multimedia { get; set; }
    }
    public class DataAppMultimediaEjercicio
    {
        public string cod_multimedia_ejercicio { get; set; }
        public string cod_tipo_multimedia { get; set; }
        public string nombre_archivo { get; set; }
        public string extension { get; set; }
        public string url_archivo { get; set; }
        public string descripcion_multimedia { get; set; }
    }    
}