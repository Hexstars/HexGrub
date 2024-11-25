using Assignment.Models;

namespace Assignment.Services
{
    public interface IRoleSvc
    {
        List<Role> GetAllRole();
        Role GetRole(int id);

        int AddRole(Role role);

        int EditRole(int id, Role role);
    }
    public class RoleSvc : IRoleSvc
    {
        protected DataContext _context;

        public RoleSvc(DataContext context)
        {
            _context = context;
        }
        public List<Role> GetAllRole()
        {
            List<Role> list = new List<Role>();
            list = _context.Roles.ToList();
            return list;
        }
        public Role GetRole(int id) 
        {
            Role role = null;
            role = _context.Roles.Find(id); //Chỉ tìm bằng khóa chính

            //product = _context.Products.Where(p => p.id == id).FirstOrDefault();
            return role;
        }

        public int AddRole(Role role) 
        {
            int ret = 0;
            try
            {
                _context.Add(role);
                _context.SaveChanges();
                ret = role.RoleId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public int EditRole(int id, Role role) 
        {
            int ret = 0;
            try
            {
                Role _role = null;
                _role = _context.Roles.Find(id); //cách này chỉ dùng cho Khóa chính

                _role.RoleName = role.RoleName;

                _context.Update(_role);
                _context.SaveChanges();
                ret = role.RoleId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
    }
}
