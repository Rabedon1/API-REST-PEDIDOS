namespace Pedidos.Domain.Entities;

public class LogAuditoria
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Evento { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Nivel { get; set; } = "Info";
    public string? Usuario { get; set; }
}
