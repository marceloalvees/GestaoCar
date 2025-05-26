using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Client client, CancellationToken cancellationToken);
        Task UpdateAsync(Client client, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken);
        Task<Client> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken);
    }
}
