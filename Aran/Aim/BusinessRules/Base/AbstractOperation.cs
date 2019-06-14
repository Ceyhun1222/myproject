using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    [DataContract]
    [KnownType(typeof(OperationGroup))]
    [KnownType(typeof(Operation))]
    public abstract class AbstractOperation
    {
        [DataMember]
        public AbstractOperationType OperType { get; protected set; }
    }
}
