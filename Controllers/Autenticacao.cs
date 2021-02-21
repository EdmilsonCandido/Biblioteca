using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using System.Linq;
using System.Collections.Generic;


namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
            public static void CheckLogin(Controller controller)
            {   
                if(string.IsNullOrEmpty(controller.HttpContext.Session.GetString("Login")))
                {
                    controller.Request.HttpContext.Response.Redirect("/Home/Login");
                }
            }
            public static bool verificaLoginSenha(string Login, string senha, Controller controller)
            {
                using (BibliotecaContext bc = new BibliotecaContext())
                {
                    verificaSeUsuarioAdminExiste(bc);

                    senha = Criptografo.TextoCriptografado(senha);

                    IQueryable<Usuario> UsuarioEncontrado = bc.Usuarios.Where(u => u.login==Login && u.senha==senha);
                   List<Usuario>listaUsuarioEncontrado = UsuarioEncontrado.ToList();

                    if(listaUsuarioEncontrado.Count==0)
                    {
                        return false;
                    }        
                    else
                    {
                        controller.HttpContext.Session.SetString("Login",listaUsuarioEncontrado[0].login);
                        controller.HttpContext.Session.SetString("Nome",listaUsuarioEncontrado[0].nome);
                        controller.HttpContext.Session.SetInt32("Tipo",listaUsuarioEncontrado[0].tipo);
                        return true;
                    }

                }
            }
            public static void  verificaSeUsuarioAdminExiste(BibliotecaContext bc)
            {
                IQueryable<Usuario> userEncontrado = bc.Usuarios.Where(u => u.login=="admin");

                if(userEncontrado.ToList().Count==0)
                {
                    Usuario admin = new Usuario();
                    admin.login = "admin";
                    admin.senha = Criptografo.TextoCriptografado("123");
                    admin.tipo = Usuario.ADMIN;
                    admin.nome = "Administrador";

                    bc.Usuarios.Add(admin);
                    bc.SaveChanges();
                }
            }
            public static void verificaSeUsuarioAdmin(Controller controller)
            {
                if(!(controller.HttpContext.Session.GetInt32("tipo")==Usuario.PADRAO))
                {
                    controller.Request.HttpContext.Response.Redirect("/Usuarios/ListaDeUsuarios");
                }
                else
                {
                   controller.Request.HttpContext.Response.Redirect("/Usuarios/NeedAdmin"); 
                }
            }

    }
}
