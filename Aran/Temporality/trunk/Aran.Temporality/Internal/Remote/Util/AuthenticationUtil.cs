#region

using System;
using System.IdentityModel.Selectors;
using System.Security.Authentication;
using Aran.Temporality.Internal.Implementation.Storage;
//using log4net;

#endregion

namespace Aran.Temporality.Internal.Remote.Util
{
    internal class AuthenticationUtil : UserNamePasswordValidator
    {
        //public static readonly ILog Log = LogManager.GetLogger(typeof(AuthenticationUtil));

        /// <summary>
        /// When overridden in a derived class, validates the specified username and password.
        /// </summary>
        /// <param name="userName">The username to validate.</param><param name="password">The password to validate.</param>
        public override void Validate(string userName, string password)
        {
            var i = userName.IndexOf("\\", StringComparison.Ordinal);
            if (i == -1) throw new AuthenticationException("Application is not specified");
            var userId=userName.Substring(0, i);

            var user = StorageService.Login(Convert.ToInt32(userId), password);

         
            //Log.Logger.Log(new LoggingEvent(new LoggingEventData
            //{
            //    Level = Level.Info,
            //    LocationInfo = new LocationInfo(userName, "Login request", "", ((user != null) ? "Correct" : "Incorrect")),
            //    LoggerName = "AuthenticationService",
            //    TimeStamp = DateTime.Now
            //}));


            if (user==null) throw new AuthenticationException("Username/Password does not match");
        }
    }
}