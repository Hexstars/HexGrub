using Assignment.Models;

namespace Assignment.Services
{
    public interface IAccountSvc
    {
        //List<Account> GetAllAccount();

        Account GetAccount(int id);

        //int AddAccount(Account account);

        //int EditProduct(int id, Account account);
    }
    public class AccountSvc: IAccountSvc
    {
        protected DataContext _context;

        public AccountSvc(DataContext context)
        {
            _context = context;
        }

        public Account GetAccount(int id) 
        {
            Account account = null;
            account = _context.Accounts.Find(id);
            return account;
        }
    }
}
