using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Data;
using WebApi.Dto.Autor;
using WebApi.Models;

namespace WebApi.Services.Autor
{
    public class AutorService : IAutorInterface
    {
        private readonly AppDbContext _context;
        public AutorService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<AutorModel>> BuscarAutorPorId(int idAutor)
        {
            ResponseModel<AutorModel> resposta = new ResponseModel<AutorModel>();

            try
            {
                var autor = await _context.Autores.FirstOrDefaultAsync(autorBanco => autorBanco.Id == idAutor);
                if (autor == null)
                {
                    resposta.Mensagem = "Autor não encontrado";
                    return resposta;
                }

                resposta.Dados = autor;
                resposta.Mensagem = "Autor encontrado com sucesso";
                return resposta;
            }


            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = ex.Message;
                return resposta;
            }

        }

        public async Task<ResponseModel<AutorModel>> BuscarAutorPorIdLivro(int idLivro)
        {
            ResponseModel<AutorModel> resposta = new ResponseModel<AutorModel>();

            try
            {
                var livro = await _context.Livros
                    .Include(a => a.Autor)
                    .FirstOrDefaultAsync(livroBanco => livroBanco.Id == idLivro);
                if (livro == null)
                {
                    resposta.Mensagem = "Autor não encontrado";
                    return resposta;
                }

                resposta.Dados = livro.Autor;
                resposta.Mensagem = "Autor encontrado com sucesso";
                return resposta;
            }

            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = ex.Message;
                return resposta;

            }
        }

        public async Task<ResponseModel<List<AutorModel>>> CriarAutor(AutorCriacaoDto autorCriacaoDto)
        {
            ResponseModel<List<AutorModel>> resposta = new ResponseModel<List<AutorModel>>();

            try
            {
                var autor = new AutorModel()
                {
                    Nome = autorCriacaoDto.Nome,
                    Sobrenome = autorCriacaoDto.Sobrenome

                };
                _context.Add(autor);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Autores.ToListAsync();
                resposta.Mensagem = "Autor criado com sucesso";

                return resposta;



            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = ex.Message;
                return resposta;
            }

        }

        public async Task<ResponseModel<List<AutorModel>>> ListarAutores()
        {
            ResponseModel<List<AutorModel>>resposta = new ResponseModel<List<AutorModel>>();
            try
            {
                var autores = await _context.Autores.ToListAsync();

                resposta.Dados = autores;
                resposta.Mensagem = "Autores listados com sucesso";

                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = ex.Message;
                return resposta;
            }
        }
    }
}
