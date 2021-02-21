using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace Biblioteca.Controllers
{
    
    public class EmprestimoController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            LivroService LivroService = new LivroService();
            EmprestimoService EmprestimoService = new EmprestimoService();

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = LivroService.ListarDisponiveis();
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            if(!string.IsNullOrEmpty(viewModel.Emprestimo.NomeUsuario))
            {
                    EmprestimoService EmprestimoService = new EmprestimoService();
                
                if(viewModel.Emprestimo.Id == 0)
                {
                    EmprestimoService.Inserir(viewModel.Emprestimo);
                }
                else
                {
                    EmprestimoService.Atualizar(viewModel.Emprestimo);
                }
                return RedirectToAction("Listagem");
            }
            else
            {
                ViewData["mensagem"] = "Por Favor, preencha todos os campos";

                LivroService LivroService = new LivroService();
                EmprestimoService EmprestimoService = new EmprestimoService();

                CadEmprestimoViewModel cadModel = new  CadEmprestimoViewModel();

                cadModel.Livros = LivroService.ListarDisponiveis();

                return View(cadModel);
            }  
        }
       

        public IActionResult Listagem(string tipoFiltro, string filtro)
        {
            FiltrosEmprestimos objFiltro = null;
            if(!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosEmprestimos();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            EmprestimoService EmprestimoService = new EmprestimoService();
            return View(EmprestimoService.ListarTodos(objFiltro));
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            LivroService LivroService = new LivroService();
            EmprestimoService em = new EmprestimoService();
            Emprestimo e = em.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = LivroService.ListarTodos();
            cadModel.Livros.Add(LivroService.ObterPorId(e.LivroId));
            cadModel.Emprestimo = e;
            
            return View(cadModel);
        }
    }
}