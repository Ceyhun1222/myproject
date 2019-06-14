using System.ComponentModel.DataAnnotations;

namespace OmsApi.Entity
{
    public class UserAudit : BaseEntity
    {
        [Required]
        public long UserId { get; private set; }

        [Required]
        public UserAuditEventType AuditEvent { get; set; }

        public string IpAddress { get; private set; }

        public static UserAudit CreateAuditEvent(long userId, UserAuditEventType auditEventType, string ipAddress)
        {
            return new UserAudit { UserId = userId, AuditEvent = auditEventType, IpAddress = ipAddress };
        }
    }

    public enum UserAuditEventType
    {
        Login = 1,
        FailedLogin = 2,
        LogOut = 3
    }
}
