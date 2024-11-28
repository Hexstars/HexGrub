using Assignment.Helpers;
using Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Services
{
    public interface IAccountSvc
    {
        Account Login(string email, string password);
        List<Account> GetAllAccount();

        AccountEditModel GetAccount(int id);

        int AddAccount(Account account);

        int EditAccount(int id, Account account);
    }
    public class AccountSvc: IAccountSvc
    {
        protected DataContext _context;
        protected IEncryptionHelper _encryptionHelper;

        public AccountSvc(DataContext context, IEncryptionHelper encryptionHelper)
        {
            _context = context;
            _encryptionHelper = encryptionHelper;
        }
        public Account Login(string email, string password)
        {
            var u = _context.Accounts.Where(
                a => a.Email.Equals(email)).Include(u => u.Role).FirstOrDefault();
            if (u == null)
            {
                return null;
            }


            bool validPass = _encryptionHelper.VerifyPassword(password, u.Password);

            if (u != null && validPass)
            {
                return u;
            }
            return null;
        }
        public List<Account> GetAllAccount()
        {
            List<Account> list = new List<Account>();
            list = _context.Accounts.Include(p => p.Role).ToList();
            return list;
        }

        public AccountEditModel GetAccount(int id) 
        {
            Account account = null;
            account = _context.Accounts.Find(id);
            AccountEditModel model = new AccountEditModel
            {
                AccountId = account.AccountId,
                Email = account.Email,
                FullName = account.FullName,
                Phone = account.Phone,
                Address = account.Address,
                RoleId = account.RoleId,
            };
            return model;
        }

        public int AddAccount(Account account)
        {
            int ret = 0;
            try
            {
                account.Password = _encryptionHelper.EncryptPassword(account.Password);

                _context.Add(account);
                _context.SaveChanges();

                // tạo cart
                var cart = new Cart
                {
                    AccountId = account.AccountId  // set Id
                };

                // Add cart
                _context.Carts.Add(cart);
                _context.SaveChanges();  //lưu
                ret = account.AccountId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        
        public int EditAccount(int id, Account account) 
        {
            int ret = 0;
            try
            {
                Account _account = null;
                _account = _context.Accounts.Find(id); //cách này chỉ dùng cho Khóa chính

                _account.Email = account.Email;
                if (_account.Password != null) 
                {
                    account.Password = _encryptionHelper.EncryptPassword(_account.Password);
                    _account.Password = account.Password;
                    _account.ConfirmPassword = account.ConfirmPassword;
                }
                _account.FullName = account.FullName;
                _account.Phone = account.Phone;
                _account.Address = account.Address;
                _account.RoleId = account.RoleId;

                _context.Update(_account);
                _context.SaveChanges();
                ret = account.AccountId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
    }
}
