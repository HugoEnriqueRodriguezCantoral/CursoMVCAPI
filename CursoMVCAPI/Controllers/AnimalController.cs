using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CursoMVCAPI.Models.WS;
using CursoMVCAPI.Models;

namespace CursoMVCAPI.Controllers
{
    public class AnimalController : BaseController
    {
        [HttpPost]
        public Reply Get([FromBody]SecurityViewModel model) 
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
                    List<ListAnimalsViewModel> lst = List(db);
                    oR.Resultado = 1;
                    oR.Data = lst;
                }

            }catch (Exception ex)
            {
                oR.Message = "Ocurrio un error en el servidor, intenta más tarde "+ex;
            }
            return oR;
        }

        [HttpPost]
        public Reply Add([FromBody]AnimalViewModel model)
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
                    animal oAnimal = new animal();
                    oAnimal.idState = 1;
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;

                    db.animal.Add(oAnimal);
                    db.SaveChanges();
                    List<ListAnimalsViewModel> lst = List(db);
                    oR.Resultado = 1;
                    oR.Data = lst;
                }
            }catch (Exception ex)
            {
                oR.Message = "Ocurrio un error en el servidor, intenta más tarde " + ex;
            }
            return oR;
        }

        [HttpPut]
        public Reply Edit([FromBody] AnimalViewModel model)
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
                    animal oAnimal = db.animal.Find(model.Id);
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;

                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
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
        public Reply Delete([FromBody] AnimalViewModel model)
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
                    animal oAnimal = db.animal.Find(model.Id);
                    oAnimal.idState = 2;

                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
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
        #region HELPERS
        private bool Validate(AnimalViewModel model)
        {
            if(model.Name == "")
            {
                error = "El nombre es obligatorio";
                return false;
            }
            return true;
        }
        private List<ListAnimalsViewModel> List(cursomvcapiEntities db)
        {
            List<ListAnimalsViewModel> lst = (from d in db.animal
                                              where d.idState == 1
                                              select new ListAnimalsViewModel
                                              {
                                                  Name = d.name,
                                                  Patas = d.patas
                                              }).ToList();
            return lst;
        }
        #endregion
    }
}
