namespace Pedidos.Domain.Entities;

public class PedidoCabecera
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal Total { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Estado { get; set; } = "Registrado";

    public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
}
