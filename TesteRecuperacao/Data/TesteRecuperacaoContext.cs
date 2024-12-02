using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesteRecuperacao.Models;

namespace TesteRecuperacao.Data
{
    public class TesteRecuperacaoContext : DbContext
    {
        public TesteRecuperacaoContext (DbContextOptions<TesteRecuperacaoContext> options)
            : base(options)
        {
        }

        public DbSet<TesteRecuperacao.Models.Tools> Tools { get; set; } = default!;
    }
}
