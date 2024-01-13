using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using JC_AjudaMutuaSLZ.Models;

namespace JC_AjudaMutuaSLZ.Controllers
{
    public class CadastroController : Controller
    {
        private readonly ClientesEntities _db = new ClientesEntities();

        //
        // GET: /Cadastro/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cadastro/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clientes clientes)
        {
            //if (IsExistNome(clientes.Nome))
            //    ModelState.AddModelError("Nome", "Login cadastrado, tente outro");

            if (IsExist(clientes.Login))
                ModelState.AddModelError("Login", "Login cadastrado, tente outro");

            //if (IsExistEmail(clientes.Email))
            //    ModelState.AddModelError("Email", "E-mail cadastrado, tente outro");

            if (ModelState.IsValid)
            {
                try
                {
                    var login = Session["Login"] == null ? "valdo" : Session["Login"].ToString();

                    clientes.DataCadastro = DateTime.Now;
                    clientes.Patrocinador = login;
                    clientes.Status = 0; // Status 0 para inativo por não ter sido autorizado ainda pelos pagamentos e comprovantes.

                    _db.Clientes.Add(clientes);
                    _db.SaveChanges();

                    UpLineGeral(clientes.idCliente, login);

                    //if (login == "valdo")
                    //{
                    //    UpLineValdo(clientes.idCliente);
                    //}
                    //else
                    //{
                    //    UpLineGeral(clientes.idCliente, login);
                    //}

                    ViewBag.Salvou = true;

                    return RedirectToAction("Index", "Home");

                }
                catch (DbEntityValidationException e)
                {
                    // retorna o erro se existir algum erro de tipo de dados
                    // no banco de dados, ou no objeto passado para o banco
                    foreach (var error in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entidade do tipo \"{0} \" no estado \"{1} \" tem os seguintes erros de validação:",
                        error.Entry.Entity.GetType().Name, error.Entry.State);
                        foreach (var ve in error.ValidationErrors)
                        {
                            Console.WriteLine("-Propriedade: \"{0}\", Erro: \"{1}\"",
                                                     ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                    throw;
                }
            }

            return View(clientes);
        }
        
        private bool IsExistNome(string nome)
        {
            var query = (from u in _db.Clientes
                         where u.Nome == nome
                         select u).FirstOrDefault();

            if (query == null)
            {
                return false;
            }
            return true;
        }

        //private void UpLineValdo(int id)
        //{
        //    var upLine1 = new UpLine { idCliente = id, idOrigem = 1, Posicao = 1, Status = "PENDENTE" };
        //    _db.UpLine.Add(upLine1);

        //    var upLine2 = new UpLine { idCliente = id, idOrigem = 3, Posicao = 2, Status = "PENDENTE" };
        //    _db.UpLine.Add(upLine2);

        //    var upLine3 = new UpLine { idCliente = id, idOrigem = 4, Posicao = 3, Status = "PENDENTE" };
        //    _db.UpLine.Add(upLine3);

        //    var upLine4 = new UpLine { idCliente = id, idOrigem = 5, Posicao = 4, Status = "PENDENTE" };
        //    _db.UpLine.Add(upLine4);

        //    _db.SaveChanges();
        //}

        private void UpLineGeral(int id, string login)
        {
            var origem1 = (from u in _db.Clientes
                          where u.Login.Equals(login)
                          select new { u.idCliente, u.Login, u.Patrocinador }).FirstOrDefault();

            if (origem1 != null)
            {
                var upLine1 = new UpLine { idCliente = id, idOrigem = origem1.idCliente, Posicao = 1, Status = "PENDENTE" };
                _db.UpLine.Add(upLine1);
            }

            var origem2 = (from u in _db.Clientes
                           where u.Login.Equals(origem1.Patrocinador)
                           select new { u.idCliente, u.Login, u.Patrocinador }).FirstOrDefault();

            if (origem2 != null)
            {
                var upLine2 = new UpLine { idCliente = id, idOrigem = origem2.idCliente, Posicao = 2, Status = "PENDENTE" };
                _db.UpLine.Add(upLine2);
            }

            var origem3 = (from u in _db.Clientes
                           where u.Login.Equals(origem2.Patrocinador)
                           select new { u.idCliente, u.Login, u.Patrocinador }).FirstOrDefault();

            if (origem3 != null)
            {
                var upLine3 = new UpLine { idCliente = id, idOrigem = origem3.idCliente, Posicao = 3, Status = "PENDENTE" };
                _db.UpLine.Add(upLine3);
            }

            var origem4 = (from u in _db.Clientes
                           where u.Login.Equals(origem3.Patrocinador)
                           select new { u.idCliente, u.Login, u.Patrocinador }).FirstOrDefault();

            if (origem4 != null)
            {
                var upLine4 = new UpLine { idCliente = id, idOrigem = origem4.idCliente, Posicao = 4, Status = "PENDENTE" };
                _db.UpLine.Add(upLine4);
            }

            _db.SaveChanges();
        }

        public JsonResult NomeUsuarioDisponivel(Clientes clientes)
        {
            return IsExist(clientes.Login)
                ? Json(true, JsonRequestBehavior.AllowGet)
                : Json(false, JsonRequestBehavior.AllowGet);
        }

        public bool IsExist(string usuario)
        {
            var query = (from u in _db.Clientes
                where u.Login == usuario
                select u).FirstOrDefault();

            if (query == null)
            {
                return false;
            }
            return true;
        }

        public bool IsExistEmail(string email)
        {
            var query = (from u in _db.Clientes
                         where u.Email == email
                         select u).FirstOrDefault();

            if (query == null)
            {
                return false;
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}