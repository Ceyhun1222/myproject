﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChartServices.DataContract;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace ChartServices.Hibernate.Config
{
    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.TableName.ToLower());
        }
    }
}