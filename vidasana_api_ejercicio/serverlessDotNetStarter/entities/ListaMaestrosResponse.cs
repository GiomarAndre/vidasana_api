using System.Collections.Generic;
namespace AwsDotnetCsharp.Entity
{
    public class ListaMaestrosResponse : GlobalResponse
    {
        public List<MaestroCategoriaResponse> categorias { get; set; }
    }
}