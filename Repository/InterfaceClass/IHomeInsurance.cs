using test0000001.Models;

namespace test0000001.Repository.InterfaceClass
{
    public interface IHomeInsurance
    {
        Task<IEnumerable<Home_Insurance>> getAllHomeInsurance();
        Task<Home_Insurance> getHomeInsurance(int id);
        Task<bool> addHomeInsurance(Home_Insurance newHomeInsurance);
        Task<bool> editHomeInsurance(Home_Insurance editHomeInsurance);
        Task<bool> deleteHomeInsurance(int id);
    }
}
