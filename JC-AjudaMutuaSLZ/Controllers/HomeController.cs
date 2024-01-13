using System.Linq;
using System.Web.Mvc;
using JC_AjudaMutuaSLZ.Models;

namespace JC_AjudaMutuaSLZ.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClientesEntities _db = new ClientesEntities();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var login = @HttpContext.ApplicationInstance.Request.QueryString.ToString();

            var clientes = (from c in _db.Clientes
                            where c.Login == login
                            select c).SingleOrDefault();

            if (clientes != null)
            {
                Session["Login"] = clientes.Nome;
                Session["User"] = clientes.Login;
            }
            else
            {
                Session["Login"] = null;
                Session["User"] = null;
            }

            ViewBag.Salvou = false;

            return View();
        }

        public ActionResult ComoFunciona()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Construcao()
        {
            ViewBag.Message = "Breve em funcionamento.";

            return View();
        }

        public ActionResult Contato()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contato(string nome, string email, string mensagem)
        {
            var envia = new Email();
            var texto = mensagem;
            envia.EnviarEmail("Console Aplication", texto, true, "", new[] { email }, "andreluizjm@hotmail.com");

            return RedirectToAction("Index");
        }

        public ActionResult Suporte()
        {
            ViewBag.Message = "JCarvalho Tecnologia em Informação";

            return View();
        }
    }
}