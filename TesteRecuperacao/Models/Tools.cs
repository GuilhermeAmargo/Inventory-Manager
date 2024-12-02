using System.ComponentModel.DataAnnotations;

namespace TesteRecuperacao.Models;

public class Tools
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }
    public required string Category { get; set; }
}