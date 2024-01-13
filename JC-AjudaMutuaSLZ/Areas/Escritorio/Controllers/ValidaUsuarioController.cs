using JC_AjudaMutuaSLZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JC_AjudaMutuaSLZ.Areas.Escritorio.Controllers
{
    public class ValidaUsuarioController : Controller
    {
        private static readonly ClientesEntities Db = new ClientesEntities();

        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        /*************************************************
         *  VALIDAÇÃO DE USUARIO
         * ***********************************************/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AutenticarUsuario(string usuario, string senha)
        {
            //var entities = new SalaoEntities();

            var query = (from u in Db.Clientes
                         where u.Login == usuario &&
                            u.Senha == senha
                         select u).SingleOrDefault();

            //Usuário não existe ou a senha está incorreta
            if (query == null)
            {
                ViewBag.ErrTitulo = "Acesso Restrito.";
                ViewBag.ErrTituloMensagem = "Acesso Negado";
                ViewBag.ErrMensagem = "Verifique Usuário e Senha se estão corretos.";
                return PartialView("_Mensagem");
                //return Content("<script>alert('Acesso negado. Verifique Usuário e Senha se estão corretos.)</script>");
            }

            //Irá setar um cookie encriptado com o Login do usuário autenticado
            FormsAuthentication.SetAuthCookie(query.Nome, false);

            Session["Usuario"] = query.Nome;
            Session["Login"] = query.Login;
            Session["idUsuario"] = query.idCliente;
            //Session["IdPermissao"] = query.IdPermissao;
            //Session["SessionID"] = Session.SessionID;
            //Session["IdLoja"] = query.IdLoja;

            return RedirectToAction("Index", "Home");
        }

        //[Authorize]
        //public static Usuario GetUsuarioLogado()
        //{
        //    //Pegamos o login do usuário logado
        //    string login = System.Web.HttpContext.Current.User.Identity.Name;

        //    //Naõ existe usuário logado
        //    if (login == "")
        //    {
        //        return null;
        //    }
        //    SalaoEntities entities = new SalaoEntities();

        //    //Buscaos no banco de dados o usuario que está logado
        //    Usuario user = (from u in _db.Usuario
        //        where u.Email == login
        //        select u).SingleOrDefault();

        //    return user;
        //}

        //Deslogando do sistema
        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Session.Abandon();
            ViewBag.Erro = "Usuário desconectado com sucesso";
            return RedirectToAction("Index", "ValidaUsuario");
        }

        public ActionResult RecuperarSenha(string email)
        {

            //var query = _db.Usuario.SingleOrDefault(u => email != null && (email != null && u.Email == email));

            //if (query != null)
            //{
            //    // enviar email para recuperar senha
            //    return Index();
            //}
            //// retorna mensagem de erro, que email nao foi encontrado
            return Index();
        }
    }
}