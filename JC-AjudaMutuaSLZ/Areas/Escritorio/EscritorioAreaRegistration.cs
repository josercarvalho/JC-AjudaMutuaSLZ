﻿using System.Web.Mvc;

namespace JC_AjudaMutuaSLZ.Areas.Escritorio
{
    public class EscritorioAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Escritorio";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Escritorio_default",
                "Escritorio/{controller}/{action}/{id}",
                //new { action = "Index", id = UrlParameter.Optional }
                new { controller = "Validausuario", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JC_AjudaMutuaSLZ.Areas.Escritorio.Controllers" }
            );
        }
    }
}