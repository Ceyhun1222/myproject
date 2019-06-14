using System;
using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel;
using AerodromeServices.DataContract;
using AerodromeServices.Logging;
using AerodromeServices.Repositories;

namespace AerodromeServices.Security
{
    public class CustomUserNameValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            LogManager.Configure("ChartManagerService.log", "ChartManagerService_Errorlogs.log", LogLevel.Info);
            // validate arguments
            if (string.IsNullOrEmpty(userName))
            {
                LogManager.GetLogger(GetType().Name).Error(new FaultException("UserName is empty"), "UserName is empty");
                throw new FaultException("UserName is empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                LogManager.GetLogger(GetType().Name).Error(new FaultException("Password is empty"), "Password is empty");
                throw new FaultException("Password is empty");
            }

            RepositoryContext dbContext = new RepositoryContext();

            var currentUser = dbContext.Repository<User>()
                .GetAll()
                .FirstOrDefault(u => u.UserName == userName && u.Password == password);

            if (currentUser == null)
            {
                LogManager.GetLogger(GetType().Name).Error(new Exception("User is not found"), "User is not found");
                throw new Exception("User is not found");
            }
            

        }
    }

}
