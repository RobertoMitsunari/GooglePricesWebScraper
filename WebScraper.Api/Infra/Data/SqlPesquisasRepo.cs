using System;
using System.Collections.Generic;
using System.Linq;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Common.Domain.Model;

namespace WebScraper.Api.Infra.Data
{
    public class SqlPesquisasRepo : IPesquisaRepo
    {
        private readonly ProdutoContext _context;

        public SqlPesquisasRepo(ProdutoContext context)
        {
            _context = context;
        }

        public void CreatePesquisa(Pesquisa Pesquisa)
        {
            if (Pesquisa is null)
            {
                throw new ArgumentNullException(nameof(Pesquisa));
            }

            _context.Pesquisas.Add(Pesquisa);
        }

        public void DeletePesquisa(Pesquisa Pesquisa)
        {
            if (Pesquisa is null)
            {
                throw new ArgumentNullException(nameof(Pesquisa));
            }

            _context.Remove(Pesquisa);
        }

        public Pesquisa GetPesquisaById(int id)
        {
            return _context.Pesquisas.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Pesquisa> GetPesquisas()
        {
            return _context.Pesquisas.ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdatePesquisa(Pesquisa Pesquisa)
        {
            // No SQL agt n precisa implementar nada no update mas esse método é adicionado caso alguma outra forma
            // de persistencia acabe surgindo no projeto, nesse caso esse método vai ser utilizado.
        }
    }
}
