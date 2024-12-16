using FixPoint_Backend.Models;

namespace FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;

public interface ICustomerRepository
{
    void AddCustomer(Customer customer);
    void DeleteCustomer(Customer customer);
    Customer GetCustomer(Guid id);
    List<Customer> GetCustomers();
    void UpdateCustomer(Customer customer);
}