
using SharedKernal.Persistence;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork(ApplicationDbContext db): IUnitOfWork
{
	public async Task Commit()
	{
		await db.SaveChangesAsync();
	}
}