namespace TesteRecuperacao.Models;

public class WithdrawalTool
{
    public int Id { get; set; }
    public int IdTool { get; set;}
    public int IdWithdral { get; set;}
    public int Quantity { get; set;}
    public Tools Tools { get; set; }
}