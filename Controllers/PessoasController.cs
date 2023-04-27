using Crud_AspNet_Core7_Dapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Crud_AspNet_Core7_Dapper.Controllers
{
    public class PessoasController : Controller
    {
        private readonly string _connectionString;
        public PessoasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Pessoas
        public async Task<IActionResult> Index()
        {
            using (var sqlConnection = new MySqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM pessoas";

                var pessoas = await sqlConnection.QueryAsync<Pessoa>(sql);
                return View(pessoas);
            }
        }

        // GET: Pessoas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameters = new
            {
                id
            };

            using (var sqlConnection = new MySqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM pessoas WHERE Id = @id";

                var pessoa = await sqlConnection.QuerySingleOrDefaultAsync<Pessoa>(sql, parameters);

                if (pessoa is null)
                {
                    return NotFound();
                }

                return PartialView("_DetailsPessoa", pessoa);
            }
        }

        // GET: Pessoas/Create
        public IActionResult Create()
        {
            return PartialView("_CreatePessoa");
        }

        // POST: Pessoas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pessoa pessoa)
        {
            var parameters = new
            {
                pessoa.Nome,
                pessoa.Sobrenome,
                pessoa.Nascimento,
                pessoa.Telefone
            };

            using (var sqlConnection = new MySqlConnection(_connectionString))
            {
                const string sql = "INSERT INTO pessoas (Nome, Sobrenome, Nascimento, Telefone) VALUES (@Nome, @Sobrenome, @Nascimento, @Telefone); SELECT LAST_INSERT_ID();";

                int id = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);

                TempData["mensagemResultSucces"] = $"O cadastro de {pessoa.Nome} foi realizado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameters = new
            {
                id
            };

            using (var sqlConnection = new MySqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM pessoas WHERE Id = @id";

                var pessoa = await sqlConnection.QuerySingleOrDefaultAsync<Pessoa>(sql, parameters);

                if (pessoa is null)
                {
                    return NotFound();
                }

                return PartialView("_EditPessoa", pessoa);
            }
        }

        // POST: Pessoas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            var parameters = new
            {
                id,
                pessoa.Nome,
                pessoa.Sobrenome,
                pessoa.Nascimento,
                pessoa.Telefone
            };

            if (PessoaExists(pessoa.Id))
            {
                using (var sqlConnection = new MySqlConnection(_connectionString))
                {
                    const string sql = "UPDATE pessoas SET Nome = @Nome, Sobrenome = @Sobrenome, Nascimento = @Nascimento, Telefone = @Telefone WHERE Id = @id";

                    await sqlConnection.ExecuteAsync(sql, parameters);

                    TempData["mensagemResultSucces"] = $"O cadastro de {pessoa.Nome} foi editado com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["mensagemResultError"] = $"Não existe cadastro com o Id {pessoa.Id}.";
                return NotFound();
            }
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameters = new
            {
                id
            };

            using (var sqlConnection = new MySqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM pessoas WHERE Id = @id";

                var pessoa = await sqlConnection.QuerySingleOrDefaultAsync<Pessoa>(sql, parameters);

                if (pessoa is null)
                {
                    return View();
                }

                return PartialView("_DeletePessoa", pessoa);
            }
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                TempData["mensagemResultError"] = $"Cadastro inexistente.";
                return NotFound();
            }

            var parameters = new
            {
                id
            };

            if (PessoaExists(id))
            {
                using (var sqlConnection = new MySqlConnection(_connectionString))
                {
                    const string sql = "DELETE FROM pessoas WHERE Id = @id";

                    await sqlConnection.ExecuteAsync(sql, parameters);

                    TempData["mensagemResultSucces"] = $"O cadastro de id {id} foi removido com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["mensagemResultError"] = $"Cadastro inexistente.";
                return NotFound();
            }
        }

        private bool PessoaExists(int? id)
        {
            var parameters = new
            {
                id
            };

            using (var sqlConnection = new MySqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM pessoas WHERE Id = @id";

                var pessoa = sqlConnection.QuerySingleOrDefaultAsync<Pessoa>(sql, parameters);

                if (pessoa is null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}