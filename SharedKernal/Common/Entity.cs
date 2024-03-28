using System.Reflection.PortableExecutable;

namespace SharedKernal.Common;

public class Entity<TId>
{
	protected Entity(TId id)
	{
		Id = id; 
		UpdatedDateTime = DateTime.Now;
	}

	

	public TId Id { get; protected set; }
	public DateTime CreateDateTime { get; protected set; }
	public DateTime UpdatedDateTime { get; protected set; }
}
