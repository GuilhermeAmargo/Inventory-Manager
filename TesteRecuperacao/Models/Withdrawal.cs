namespace TesteRecuperacao.Models;

public class Withdrawal
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public List<WithdrawalTool> WithdrawalTools { get; set; }
}