using Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute;
using Aran.Temporality.Internal.Interface;
using System;

namespace Aran.Temporality.Common.Entity
{

    [Serializable]
    public class Notam : INHibernateEntity
    {

        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime CreatedOn { get; set; } = DateTime.Now;

        [StringLength(1)]
        public virtual string Series { get; set; }
        [StringLength(4)]
        public virtual string Number { get; set; }
   
        public virtual int Year { get; set; }
        [StringLength(1)]
        public virtual int Type { get; set; }

        [StringLength(10)]
        public virtual string RefNotam { get; set; }


        #region QLine
        [StringLength(4)]
        public virtual String FIR { get; set; }
        [StringLength(2)]
        public virtual String Code23 { get; set; }
        [StringLength(2)]
        public virtual String Code45 { get; set; }
        [StringLength(2)]
        public virtual String Traffic { get; set; }
        [StringLength(5)]
        public virtual String Purpose { get; set; }
        [StringLength(4)]
        public virtual String Scope { get; set; }
        [StringLength(3)]
        public virtual String Lower { get; set; }
        [StringLength(3)]
        public virtual String Upper { get; set; }
        [StringLength(20)]
        public virtual String Coordinates { get; set; }
        [StringLength(10)]
        public virtual String Radius { get; set; }
        #endregion

        [StringLength(4)]
        public virtual string ICAO { get; set; }


        public virtual DateTime StartValidity { get; set; }
        public virtual DateTime EndValidity { get; set; }
        public virtual bool EndValidityEst { get; set; }

        public virtual string Schedule { get; set; }
        [StringLength(1500)]
        public virtual string ItemE { get; set; }

        [StringLength(20)]
        public virtual string ItemF { get; set; }
        [StringLength(20)]
        public virtual string ItemG { get; set; }

        [StringLength(2000)]
        public virtual string Text { get; set; }

        public virtual int Format { get; set; }

        

    }

    public enum NotamFormat
    {
        Text = 0,
        Xml = 1
    }

    public enum NotamType{
        N = 0,
        C = 1,
        R =2
    }
}
