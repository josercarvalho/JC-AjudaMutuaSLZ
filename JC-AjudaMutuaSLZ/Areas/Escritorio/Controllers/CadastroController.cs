using JC_AjudaMutuaSLZ.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace JC_AjudaMutuaSLZ.Areas.Escritorio.Controllers
{
    public class CadastroController : Controller
    {
        private readonly ClientesEntities _db = new ClientesEntities();

        //
        // GET: /Escritorio/Cadastro/

        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["idUsuario"]);

            return RedirectToAction("Edit", id);
        }

        //
        // GET: /Escritorio/Cadastro/Details/5

        public ActionResult Details(int id = 0)
        {
            Clientes clientes = _db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        //
        // GET: /Escritorio/Cadastro/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Escritorio/Cadastro/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                _db.Clientes.Add(clientes);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(clientes);
        }

        public ActionResult UploadPropertyImage()
        {
            // Business logic....
            return Content("<script language='javascript' type='text/javascript'>alert('Salvo com Sucesso...!');</script>");
        }

        //
        // GET: /Escritorio/Cadastro/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Clientes clientes = _db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        //
        // POST: /Escritorio/Cadastro/Edit/5

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clientes clientes)
        {
            if (ModelState.IsValid)
            {

                //_db.Entry(clientes).State = EntityState.Modified;
                //DbPropertyValues dbPropertyValues = _db.Entry(clientes).GetDatabaseValues();
                //_db.Entry(clientes).Property(b => b.Descricao).OriginalValue = dbPropertyValues["Descricao"].ToString();
                //_db.SaveChanges();

                _db.Entry(clientes).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientes);
        }

        //
        // GET: /Escritorio/Cadastro/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Clientes clientes = _db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        //
        // POST: /Escritorio/Cadastro/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clientes clientes = _db.Clientes.Find(id);
            _db.Clientes.Remove(clientes);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LiberaCadastro()
        {
            int id = Convert.ToInt32(Session["idUsuario"]);
            //var cliente = _db.Clientes.Find(id);

            var arquivo = (from u in _db.Arquivo
                           where u.IdOrigem == id && u.Status == "PENDENTE"
                           select u);

            ViewBag.Acesso = "http://www.ajudamutuaslz.com/?" + Session["Login"];
            ViewBag.Comprovante = "/uploads/";
            ViewBag.Libera = arquivo.Count();

            ViewBag.Arquivo = arquivo;

            return View(arquivo);
        }

        [Authorize]
        public ActionResult LiberaPagamento(int idCliente, int idOrigem)
        {

            //var idCliente = form["idCliente"];
            //var idOrigem = form["idOrigem"];

            var upLine = (_db.UpLine.Where(
                u => u.idCliente == idCliente && u.idOrigem == idOrigem)).FirstOrDefault();
            if (upLine != null)
            {
                upLine.Status = "COMPROVADO";
                _db.Entry(upLine).State = EntityState.Modified;

                var comprovante = (_db.Arquivo.Where(
                    u => u.IdCliente == idCliente && u.IdOrigem == idOrigem)).FirstOrDefault();

                if (comprovante != null)
                {
                    comprovante.Status = "COMPROVADO";
                    _db.Entry(comprovante).State = EntityState.Modified;
                }
            }

            _db.SaveChanges();

            var ativa = _db.UpLine.Where(u => u.idCliente == idCliente && u.Status == "COMPROVADO");

            if (ativa.Count() == 4)
            {
                Clientes clientes = _db.Clientes.Find(idCliente);
                clientes.Status = 1;
                _db.Entry(clientes).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return RedirectToAction("LiberaCadastro");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}