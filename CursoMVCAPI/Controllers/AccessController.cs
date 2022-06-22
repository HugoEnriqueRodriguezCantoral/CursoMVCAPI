using CursoMVCAPI.Models.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CursoMVCAPI.Models;

namespace CursoMVCAPI.Controllers
{
    public class AccessController : ApiController
    {
        [HttpGet]
        public Reply HelloWorld()
        {
            Reply oR=new Reply();
            oR.Resultado = 1;
            oR.Message = "Hi World";
            return oR;
        }
        [HttpPost]
        public Reply Login([FromBody] AccessViewModel model)
        {
            var oR = new Reply();
            oR.Resultado = 0;
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    var lst = db.user.Where(d=>d.email == model.email && d.password==model.password && d.idEstatus==1).ToList();
                    if (lst.Count > 0)
                    {
                        oR.Resultado = 1;
                        oR.Data= Guid.NewGuid().ToString(); 

                        user oUser = lst.First();
                        oUser.token = (string)oR.Data;
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        oR.Message = "Datos Incorrectos";
                    }
                }

            }
            catch (Exception ex)
            {
                oR.Resultado=0;
                oR.Message = "Ocurrio un error!!, estamos corrigiendolo :)";
            }
            return oR;
        }
    }
}
