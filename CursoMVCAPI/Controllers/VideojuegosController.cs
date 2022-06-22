using CursoMVCAPI.Models;
using CursoMVCAPI.Models.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CursoMVCAPI.Controllers
{
    public class VideojuegosController : BaseController
    {
        [HttpGet]
        public Reply Get([FromBody] SecurityViewModel model)
        {
            Reply oR = new Reply();
            oR.Resultado = 0;
            if (!Verify(model.token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    List<ListVideojuegosViewModel> lst = List(db);
                    oR.Resultado = 1;
                    oR.Data = lst;
                }

            }
            catch (Exception ex)
            {
                oR.Message = "Ocurrio un error en el servidor, intenta más tarde " + ex;
            }
            return oR;
        }
        [HttpPost]
        public Reply Add([FromBody] VideojuegosViewModel model)
        {
            Reply oR = new Reply();
            oR.Resultado = 0;
            if (!Verify(model.token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            //validaciones
            if (Validate(model))
            {
                oR.Message = error;
            }
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    videojuego oVideo = new videojuego();
                    oVideo.name = model.Name;
                    oVideo.precio = model.Precio;
                    oVideo.ageRecom = model.AgeRecommend;

                    db.videojuego.Add(oVideo);
                    db.SaveChanges();
                    List<ListVideojuegosViewModel> lst = List(db);
                    oR.Resultado = 1;
                    oR.Data = lst;
                }
            }
            catch (Exception ex)
            {
                oR.Message = "Ocurrio un error en el servidor, intenta más tarde " + ex;
            }
            return oR;
        }
        [HttpPut]
        public Reply Edit([FromBody] VideojuegosViewModel model)
        {
            Reply oR = new Reply();
            oR.Resultado = 0;
            if (!Verify(model.token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            //validaciones
            if (!Validate(model))
            {
                oR.Message = error;
            }
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    videojuego oVideo = db.videojuego.Find(model.Id);
                    oVideo.name = model.Name;
                    oVideo.precio = model.Precio;
                    oVideo.ageRecom = model.AgeRecommend;

                    db.Entry(oVideo).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListVideojuegosViewModel> lst = List(db);
                    oR.Resultado = 1;
                    oR.Data = lst;
                }
            }
            catch (Exception ex)
            {
                oR.Message = "Ocurrio un error en el servidor, intenta más tarde " + ex;
            }
            return oR;
        }
        [HttpDelete]
        public Reply Delete([FromBody] VideojuegosViewModel model)
        {
            Reply oR = new Reply();
            oR.Resultado = 0;
            if (!Verify(model.token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            //validaciones
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    videojuego oAnimal = db.videojuego.Find(model.Id);                   
                    db.videojuego.Remove(oAnimal);
                    db.SaveChanges();

                    List<ListVideojuegosViewModel> lst = List(db);
                    oR.Resultado = 1;
                    oR.Data = lst;
                }
            }
            catch (Exception ex)
            {
                oR.Message = "Ocurrio un error en el servidor, intenta más tarde " + ex;
            }
            return oR;
        }
        private bool Validate(VideojuegosViewModel model)
        {
            if (model.Name == "")
            {
                error = "El nombre es obligatorio";
                return false;
            }
            return true;
        }
        private List<ListVideojuegosViewModel> List(cursomvcapiEntities db)
        {
            List<ListVideojuegosViewModel> lst = (from d in db.videojuego
                                              select new ListVideojuegosViewModel
                                              {
                                                  Name = d.name,
                                                  Precio = d.precio,
                                                  AgeRecommend = d.ageRecom
                                              }).ToList();
            return lst;
        }
    }
}
