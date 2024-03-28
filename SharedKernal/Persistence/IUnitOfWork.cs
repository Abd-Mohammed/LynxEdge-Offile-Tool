

namespace SharedKernal.Persistence;

public interface IUnitOfWork
{
	Task Commit();
}