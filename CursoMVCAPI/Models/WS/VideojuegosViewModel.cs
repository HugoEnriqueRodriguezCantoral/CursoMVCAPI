using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CursoMVCAPI.Models.WS
{
    public class VideojuegosViewModel : SecurityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Precio { get; set; }
        public string AgeRecommend { get; set; }
    }
}