using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Interfaces;
using Pedidos.Infrastructure.Data;

namespace Pedidos.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly PedidosDbContext _context;
    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _transaction;

    public PedidoRepository(PedidosDbContext context)
    {
        _context = context;
    }

    public async Task<PedidoCabecera> CrearPedidoAsync(PedidoCabecera pedido)
    {
        _context.PedidoCabecera.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task RegistrarLogAsync(LogAuditoria log)
    {
        _context.LogAuditoria.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }
}
