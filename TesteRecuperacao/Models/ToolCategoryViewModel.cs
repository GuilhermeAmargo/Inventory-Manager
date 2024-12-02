using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TesteRecuperacao.Models;

public class ToolCategoryViewModel
{
    public List<Tools>? Tools { get; set; }
    public SelectList? Categories { get; set; }
    public string? ToolCategory { get; set; }
    public string? SearchString { get; set; }
}