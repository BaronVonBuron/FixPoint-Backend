﻿using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FixPoint_Backend.Models;

namespace FixPoint_Backend.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerController> _logger;
    
    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerController> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }
    
    public void AddCustomer(Customer customer)
    {
        if (customer == null)
        {
            nullCaseException(customer);
        }
        _logger.LogInformation("Adding a new customer");
        _customerRepository.AddCustomer(customer);
    }
    
    public void UpdateCustomer(Customer customer)
    {
        if (customer == null || customer.GetID() == Guid.Empty)
            nullCaseException(customer);

        _logger.LogInformation("Updating customer details for ID: {Id}", customer.GetID());
        _customerRepository.UpdateCustomer(customer);
    }
    
    public void DeleteCustomer(Customer customer)
    {
        if (customer == null)
        {
            nullCaseException(customer);
        }
        _logger.LogInformation("Deleting a customer");
        _customerRepository.DeleteCustomer(customer);
    }
    
    public Customer GetCustomer(Guid id)
    {
        _logger.LogInformation("Getting a customer by id");
        return _customerRepository.GetCustomer(id);
    }
    
    public List<Customer> GetCustomers()
    {
        _logger.LogInformation("Getting all customers");
        return _customerRepository.GetCustomers();
    }
    
    public void nullCaseException(Customer customer)
    {
        _logger.LogError("Case is null");
        throw new ArgumentNullException(nameof(customer));
    }
}