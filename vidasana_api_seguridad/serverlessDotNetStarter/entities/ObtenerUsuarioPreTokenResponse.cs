using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwsDotnetCsharp.Entity {
    public class ObtenerUsuarioPreTokenResponse  {
        [Key]
        public string id_usuario { get; set; }
        public Nullable<int> id_rol { get; set; }
        public string cod_rol { get; set; }
        public string nombre_rol { get; set; }
        public string username { get; set; }
        public string doc_identidad { get; set; }
        public string nombres { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
    }

    public class ObtenerUsuarioPreTokenHttpResponse {
         public int codigo { get; set; }
        public string descripcion { get; set; }
        public ObtenerUsuarioPreTokenResponse data { get; set; }
    }
}