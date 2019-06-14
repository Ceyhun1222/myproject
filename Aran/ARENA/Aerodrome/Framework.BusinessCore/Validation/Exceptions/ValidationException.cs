// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.Validation.Exceptions.ValidationException
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

using System;
using System.Runtime.Serialization;

namespace BusinessCore.Validation.Exceptions
{
  [Serializable]
  public class ValidationException : Exception
  {
    public ValidationException()
    {
    }

    public ValidationException(string message)
      : base(message)
    {
    }

    public ValidationException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected ValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
