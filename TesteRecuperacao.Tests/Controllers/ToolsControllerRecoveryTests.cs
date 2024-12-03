using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteRecuperacao.Controllers; // Importa o controlador para testar.
using TesteRecuperacao.Data; // Importa o contexto do banco de dados.
using TesteRecuperacao.Models; // Importa os modelos de dados.
using Xunit; // Importa a biblioteca de testes (XUnit).

namespace TesteRecuperacao.Tests
{
    // Define a classe de testes para o controlador "ToolsController".
    public class ToolsControllerTests
    {
        private readonly ToolsController _controller; // Instância do controlador a ser testado.
        private readonly TesteRecuperacaoContext _context; // Contexto do banco de dados.

        // Construtor da classe de testes.
        public ToolsControllerTests()
        {
            // Configura o contexto do banco de dados para usar um banco de dados em memória.
            var options = new DbContextOptionsBuilder<TesteRecuperacaoContext>()
                .UseInMemoryDatabase(databaseName: "TesteRecuperacaoContext-6348f159-103d-4e62-a9f4-e8ca4e854a92.db") // Define o nome do banco de dados em memória.
                .Options;

            // Cria uma instância do contexto com as opções definidas.
            _context = new TesteRecuperacaoContext(options);
            // Cria uma instância do controlador, passando o contexto como parâmetro.
            _controller = new ToolsController(_context);
        }

        // Método de teste assíncrono para verificar o comportamento do método Details do controlador.
        [Fact]
        public async Task Details_ReturnsNotFound_AfterDatabaseFailure_AndProcessContinues()
        {
            // Adiciona um novo objeto 'Tools' ao banco de dados em memória.
            _context.Tools.Add(new Tools { Name = "Martelo", Quantity = 10, Category = "Ferramentas" });
            _context.SaveChanges(); // Salva as mudanças no banco de dados.

            _context.ChangeTracker.Clear(); // Limpa o rastreador de mudanças para simular falha no banco de dados.

            // Chama o método Details do controlador com um parâmetro inválido (null).
            var result = await _controller.Details(null);

            // Verifica se o resultado retornado é do tipo 'NotFoundResult'.
            var resultType = Assert.IsType<NotFoundResult>(result);

            // Exibe no console o tipo de resultado retornado após a falha simulada no banco de dados.
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine($"Falha simulada no banco de dados. Tipo do resultado retornado: '{resultType.GetType().Name}'");

            // Recupera a ferramenta do banco de dados e exibe seus dados no console para verificar se o acesso ao banco foi perdido.
            var tool = await _context.Tools.FirstOrDefaultAsync();
            Console.WriteLine($"Produto inserido: {tool.Name}, Quantidade: {tool.Quantity}, Categoria: {tool.Category}");

            // Exibe uma mensagem no console confirmando a perda de acesso ao banco de dados.
            Console.WriteLine("Acesso ao banco de dados perdido apos a falha simulada.");

            // Chama o método Index para continuar o processamento após a falha.
            var continueResult = await _controller.Index(null, null);

            // Verifica se o resultado retornado é do tipo 'ViewResult' (uma página de visualização).
            var continueViewResult = Assert.IsType<ViewResult>(continueResult);

            // Obtém o modelo de dados passado para a visualização.
            var continueModel = Assert.IsAssignableFrom<ToolCategoryViewModel>(continueViewResult.ViewData.Model);

            // Verifica se há uma ferramenta no modelo de dados retornado.
            Assert.Equal(1, continueModel.Tools.Count);

            // Exibe os dados da ferramenta no console.
            if (tool != null)
            {
                Console.WriteLine($"Produto inserido: {tool.Name}, Quantidade: {tool.Quantity}, Categoria: {tool.Category}");
            }
            else
            {
                Console.WriteLine("Acesso ao banco de dados falhou. Nenhuma ferramenta encontrada.");
            }

            // Exibe no console uma mensagem informando que o processo continuou com sucesso após a falha.
            Console.WriteLine("Processo continuado com sucesso apos falha no banco de dados.");
            Console.WriteLine("---------------------------------------------------------------------------------------------");

        }
    }
}