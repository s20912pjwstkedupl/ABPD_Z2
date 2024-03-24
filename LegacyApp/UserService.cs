using System;

namespace LegacyApp
{
    public interface IUserManagement
    {
        bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId);
    }
    
    public class UserService : IUserManagement
    {
        private readonly ClientRepository _clientRepository;
        private readonly UserCreditService _userCreditService;
        private readonly UserValidationService _userValidationService;
        public UserService()
        {
            _clientRepository = new ClientRepository();
            _userCreditService = new UserCreditService();
            _userValidationService = new UserValidationService();
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!_userValidationService.AssertFirstName(firstName) || 
                !_userValidationService.AssertLastName(lastName) ||
                !_userValidationService.AssertEmail(email) ||
                !_userValidationService.AssertDateOfBirth(dateOfBirth))
            {
                return false;
            }
            
            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            UserCreditLimitDto creditLimitDto = _userCreditService.GetCreditLimitForUserAndType(lastName, dateOfBirth, client.Type);

            user.CreditLimit = creditLimitDto.CreditLimitValue;
            user.HasCreditLimit = creditLimitDto.HasCreditLimit;

            if (!_userValidationService.AssertUserCreditLimit(user.HasCreditLimit, user.CreditLimit))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            
            return true;
        }
        
        
    }
}
