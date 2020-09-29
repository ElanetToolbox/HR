using HR.Data.Models;

namespace HR.Data.Services
{
	public interface IUserData
	{
        User Get(string username,string password);
	}
}