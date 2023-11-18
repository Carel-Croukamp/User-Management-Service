using UserManagement_Model;

namespace User_Management_Web.Services
{
    public class UserService
    {
        private readonly List<UserDto> _users = new List<UserDto>();
        private int _userIdCounter = 1;

        public List<UserDto> GetUsers()
        {
            return _users;
        }

        public UserDto? GetUserById(int id)
        {
            return _users.Find(user => user.UserId == id);
        }
        public void AddUser(UserDto user)
        {
            user.UserId = _userIdCounter++;
            _users.Add(user);
        }
        public void AddUsers(List<UserDto> users)
        {
            _users.AddRange(users);
        }
        public void UpdateUser(UserDto updatedUser)
        {
            var existingUser = _users.Find(user => user.UserId == updatedUser.UserId);
            if (existingUser != null)
            {
                existingUser.UserName = updatedUser.UserName;
                existingUser.UserLastName = updatedUser.UserLastName;
                existingUser.Email = updatedUser.Email;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.IsActive = updatedUser.IsActive;
            }
        }

        public void DeleteUser(int id)
        {
            _users.RemoveAll(user => user.UserId == id);
        }



    }
}
