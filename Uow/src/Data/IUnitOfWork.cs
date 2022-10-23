using System;

namespace EFCore.UowRepository.Data
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
    }
}
