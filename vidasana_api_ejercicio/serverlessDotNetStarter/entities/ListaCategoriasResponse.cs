using System.Collections.Generic;

namespace AwsDotnetCsharp.Entity {
    public class ListaCategoriasResponse : GlobalResponse {
        public List<DataCategoriasBD> data { get; set; }       
    }
    public class DataCategoriasBD
    {
         public string cod_categoria { get; set; }
        public string nombre_categoria { get; set; }
        public string cod_subcategoria { get; set; }
        public string nombre_subcategoria { get; set; }
    }
}