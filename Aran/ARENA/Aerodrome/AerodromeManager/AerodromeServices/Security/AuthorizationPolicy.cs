using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security;
using System.Security.Principal;
using AerodromeServices.DataContract;
using AerodromeServices.Helpers;
using AerodromeServices.Repositories;

namespace AerodromeServices.Security
{
    public class AuthorizationPolicy : IAuthorizationPolicy
    {
        Guid _id = Guid.NewGuid();

        // this method gets called after the authentication stage
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {            
            IIdentity client = GetClientIdentity(evaluationContext);

            RepositoryContext dbContext = new RepositoryContext();
           
            var currentUser = dbContext.Repository<User>()
                .GetAll()
                .FirstOrDefault(u => u.UserName == client.Name);

            if (currentUser == null)
                throw new SecurityException("User not found");

            if (WcfOperationContext.Current != null)
            {
                WcfOperationContext.Current.Items["user"] = currentUser;
            }

            return true;
        }

        private IIdentity GetClientIdentity(EvaluationContext evaluationContext)
        {
            if (!evaluationContext.Properties.TryGetValue("Identities", out var obj))
                throw new Exception("No Identity found");

            if (!(obj is IList<IIdentity> identities) || identities.Count <= 0)
                throw new Exception("No Identity found");

            return identities[0];
        }

        public ClaimSet Issuer => ClaimSet.System;

        public string Id => _id.ToString();
    }
}
