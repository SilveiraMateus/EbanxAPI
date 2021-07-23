using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbanxAPI.Models
{
    public class Account
    {
        public string id { get; set; }
        public decimal balance { get; set; }

        public Account( string id, decimal balance)
        {
            this.id = id;
            this.balance = balance;

        }

              
    }
}