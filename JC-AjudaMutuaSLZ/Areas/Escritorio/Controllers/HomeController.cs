using JC_AjudaMutuaSLZ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JC_AjudaMutuaSLZ.Areas.Escritorio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClientesEntities _db = new ClientesEntities();
        //
        // GET: /Escritorio/Home/

        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["idUsuario"]);

            var cliente = _db.Clientes.Find(id);

            ViewBag.Cliente = cliente.Nome;
            ViewBag.Status = cliente.Status;
            ViewBag.Acesso = "http://www.ajudamutuaslz.com/?" + cliente.Login;

            ViewBag.UpLine = (from u in _db.UpLine
                              where u.idCliente == id
                              select u);

            return View();
        }

        public ActionResult Construcao()
        {
            ViewBag.Message = "Breve em funcionamento.";

            return View();
        }

        public ActionResult Divulgar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviarArquivo(Picture picture, FormCollection form)
        {
            if (Request.Files.Count != 1)
            {
                //return View();
                return RedirectToAction("Index");
            }

            var post = Request.Files[0];

            if (post == null)
            {
                //return View();
                return RedirectToAction("Index");
            }

            try
            {
                var obj = new Arquivo();
                var idcliente = Convert.ToInt32(Session["idUsuario"]);
                var login = form["Login"];
                //var login = Session["Login"] == null ? "valdo" : Session["Login"].ToString();

                var origem = (from u in _db.Clientes
                              where u.Login.Equals(login)
                              select new { u.idCliente, u.Login, u.Patrocinador }).FirstOrDefault();

                var file = new FileInfo(post.FileName);
                var filename = Path.GetFileName(picture.File.FileName);
                if (filename != null)
                {
                    var path = Path.Combine(Server.MapPath("~/uploads"), filename);
                    obj.IdCliente = idcliente;
                    if (origem != null) obj.IdOrigem = origem.idCliente;
                    obj.Nome = file.Name;
                    obj.AnexoExtensao = file.Extension;
                    obj.AnexoTipo = post.ContentType;
                    obj.DataEnvio = DateTime.Now;
                    obj.Status = "PENDENTE";
                    picture.File.SaveAs(path);
                }

                //using (var reader = new BinaryReader(post.InputStream))
                //    obj.AnexoBytes = reader.ReadBytes(post.ContentLength);
                obj.AnexoBytes = null;

                _db.Arquivo.Add(obj);
                _db.SaveChanges();

                var upLine = (from u in _db.UpLine
                              where u.idCliente == idcliente && u.idOrigem == origem.idCliente
                              select u).FirstOrDefault();

                if (upLine != null)
                {
                    upLine.Status = "ENVIADO";
                    _db.Entry(upLine).State = EntityState.Modified;
                }

                _db.SaveChanges();

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

            return RedirectToAction("Index");
        }

        public ActionResult MinhaRede()
        {
            var login = (string)Session["Login"];
            IQueryable<Clientes> clientes = _db.Clientes.Include(c => c.UpLine).Where(c => c.Patrocinador == login);

            ViewBag.Acesso = "http://www.ajudamutuaslz.com/?" + Session["Login"];

            return View(clientes);
        }

        public ActionResult UpLine()
        {
            int id = Convert.ToInt32(Session["idUsuario"]);

            var cliente = _db.Clientes.Find(id);

            ViewBag.Cliente = cliente.Nome;
            ViewBag.Status = cliente.Status;
            ViewBag.Acesso = "http://www.ajudamutuaslz.com/?" + cliente.Login;

            ViewBag.UpLine = (from u in _db.UpLine
                              where u.idCliente == id
                              select u);
            return View();
        }
    }
}