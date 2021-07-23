using EbanxAPI.Models;
using EbanxAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EbanxAPI.Controllers
{
    public static class Cache
    {
        private static List<Account> accounts = new List<Account>();

        private static object cacheLock = new object();
        public static List<Account> Accounts
        {
            get
            {
                lock (cacheLock)
                {
                    if (accounts == null)
                    {
                        accounts = new List<Account>();
                    }
                    return accounts;
                }
            }
        }
    }

    public class AccountsController : ApiController
    {
        [HttpGet]

        public IHttpActionResult Balance(string account_id)
        {
            var account = Cache.Accounts.SingleOrDefault(e => e.id == account_id);
            if (account == null)
            {
                return NotFound();
            }
            else { return Ok(account.balance); }
        }

        [HttpPost]

        public IHttpActionResult Event(EventCommand command)
        {
            if (command.type == "deposit")
            {
                var account = Cache.Accounts.SingleOrDefault(e => e.id == command.destination);
                if (account == null)
                {
                    account = new Account(command.destination, command.amount);
                    Cache.Accounts.Add(account);
                    return Ok(new { destination = account });
                }
                else
                {
                    account.balance += command.amount;
                    return Ok(new { destination = account });
                }
            }
            else if (command.type == "withdraw")
            {
                var account = Cache.Accounts.SingleOrDefault(e => e.id == command.origin);
                if (account == null)
                {
                    account = new Account(command.origin, command.amount);
                    Cache.Accounts.Add(account);
                    return Ok(new { origin = account });
                }
                else
                {
                    account.balance -= command.amount;
                    return Ok(new { origin = account });
                }
            }
            else if (command.type == "transfer")
            {
                var origin = Cache.Accounts.SingleOrDefault(e => e.id == command.origin);
                var destination = Cache.Accounts.SingleOrDefault(e => e.id == command.destination);
                if (origin == null)
                {
                    return NotFound();
                }
                else
                {
                    origin.balance -= command.amount;
                    if (destination == null)
                    {
                        destination = new Account(command.destination, command.amount);
                        Cache.Accounts.Add(destination);
                        return Ok(new { destination = destination, origin = origin });
                    }
                    else
                    {
                        destination.balance += command.amount;

                        return Ok(new { destination = destination, origin = origin });
                    }
                }
            }
            else return BadRequest(); 

        }

        [HttpPost]

        public IHttpActionResult Reset()
        {
            Cache.Accounts.Clear();
            return Ok();
        }
    }
}
