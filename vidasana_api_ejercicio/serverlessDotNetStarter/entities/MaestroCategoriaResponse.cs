using System.Collections.Generic;

namespace AwsDotnetCsharp.Entity {
    public class MaestroCategoriaResponse {
        public string cod_categoria { get; set; }
        public string nombre_categoria { get; set; }
        public List<MaestroSubcategoriaResponse> subcategorias { get; set; }
    }
}