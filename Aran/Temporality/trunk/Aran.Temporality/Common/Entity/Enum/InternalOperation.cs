using System;

namespace Aran.Temporality.Common.Entity.Enum
{
    [Flags]
    internal enum InternalOperation
    {
        None = 0,

        //if WorkPackage + FeatureTypeId are set
        ReadData            = 1 << 0, //can read data of specified FeatureType
        WriteData           = 1 << 1, //can write data of specified FeatureType

        //if  WorkPackage is set 
        ReadPackage         = 1 << 2, //can read any data of specified Package
        WritePackage        = 1 << 3, //can write any data of specified Package
        CommitPackage       = 1 << 4, //can commit and rollback specified Packag

        //if nothing is set
        CreatePackage       = 1 << 5, //can create any Package 
        ReadStorage         = 1 << 6, //can read any data 
        WriteStorage        = 1 << 7, //can write any data 
        TruncateStorage     = 1 << 8,  //can truncate storage

        ChangeUsers = 1<<9,
        ChangeGroups = 1<<10,
        ChangeLogLevel = 1 << 11
    }
}