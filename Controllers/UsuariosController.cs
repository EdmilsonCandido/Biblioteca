using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult ListaDeUsuarios()
        {
          //  Autenticacao.CheckLogin(this);
           // Autenticacao.verificaSeUsuarioAdmin(this);

            return View(new UsuarioService().Listar());
        }

        public IActionResult editarUsuario(int Id)
        {
            Usuario u = new UsuarioService().Listar(Id);

            return View(u);
        }
        
        [HttpPost]
        public IActionResult editarUsuario(Usuario userEditado)
        {
            UsuarioService us = new UsuarioService();
            us.editarUsuario(userEditado);

            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult RegistrarUsuarios()
        {
           // Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioAdmin(this);
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuarios(Usuario novoUser)
        {
            //Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioAdmin(this);

            novoUser.senha = Criptografo.TextoCriptografado(novoUser.senha);

            UsuarioService us = new UsuarioService();
            us.incluirUsuario(novoUser);

            return RedirectToAction("cadastroRealizado");
        }
        
        public IActionResult ExcluirUsuario(int Id)
        {
            return View(new UsuarioService().Listar(Id));

        }

        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao, int Id)
        {
            if(decisao=="EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do Usuario"+new UsuarioService().Listar(Id).nome+"Realizado com sucesso";
                new UsuarioService().excluirUsuario(Id);
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão cancelada";
                return View ("ListaDeUsuarios", new UsuarioService().Listar());
            }
            
        }
        public IActionResult cadastroRealizado()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioAdmin(this);

            return View();
        }

        public IActionResult NeedAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }
        
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}