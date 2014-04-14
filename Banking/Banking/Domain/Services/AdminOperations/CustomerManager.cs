﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banking.Application.DAL;
using Banking.Domain.Entities;

namespace Banking.Domain.Services.AdminOperations
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public IEnumerable<ICustomer> FindCustomerByFirstName(string firstName)
        {
            var customers = customerRepository.GetCustomersByFirstName(firstName);
            return customers;
        }

        public IEnumerable<ICustomer> FindCustomerByUsername(string userName)
        {
            var customers = customerRepository.GetCustomersByUserName(userName);
            return customers;
        }

        public IEnumerable<ICustomer> FindCustomerByAccountNumber(string accountNumber)
        {
            var customers = customerRepository.GetCustomersByAccountNumber(accountNumber);
            return customers;
        }
    }
}