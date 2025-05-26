using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;
        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<Client> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Clients.FindAsync(new object[] { id }, cancellationToken).AsTask();
        }
        public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Clients.ToListAsync(cancellationToken);
        }
        public async Task AddAsync(Client client, CancellationToken cancellationToken)
        {
            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Client client, CancellationToken cancellationToken)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var client = await GetByIdAsync(id, cancellationToken);
            if (client == null)
                throw new ArgumentException("Client not found", nameof(id));
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public Task<Client> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            return _context.Clients.FirstOrDefaultAsync(c => c.CPF == cpf, cancellationToken);
        }
        public Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            return _context.Clients.AnyAsync(c => c.CPF == cpf, cancellationToken);
        }
    }
}
