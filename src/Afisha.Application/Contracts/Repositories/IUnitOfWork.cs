using Afisha.Domain.Entities;
using System;

namespace Afisha.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        public void Save();
    }
}
