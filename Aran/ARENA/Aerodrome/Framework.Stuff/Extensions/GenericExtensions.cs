// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.Extensions.GenericExtensions
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Framework.Stuff.Extensions
{
    public static class GenericExtensions
    {
        public static void Copy<TParent, TChild>(TParent parent, TChild child) where TParent : class
                                             where TChild : class
        {
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        var setter = childProperty.GetSetMethod();
                        if (setter is null) continue;
                        childProperty.SetValue(child, parentProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }
        public static T ShallowClone<T>(this T instance)
        {
            object copy;
            Type type = instance.GetType();

            if (instance is ICloneable)
                return (T)((ICloneable)instance).Clone();

            List<MemberInfo> fields = new List<MemberInfo>();
            if (type.GetCustomAttributes(typeof(SerializableAttribute), false).Length == 0)
            {
                Type t = type;
                while (t != typeof(Object))
                {
                    fields.AddRange(t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                    t = t.BaseType;
                }
            }
            else
                fields.AddRange(FormatterServices.GetSerializableMembers(instance.GetType()));

            copy = FormatterServices.GetUninitializedObject(instance.GetType());
            object[] values = FormatterServices.GetObjectData(instance, fields.ToArray());
            FormatterServices.PopulateObjectMembers(copy, fields.ToArray(), values);

            return (T)copy;
        }

        public static T CloneInstance<T>(this T me) where T : class, new()
        {
            return me.CloneInstance<T>(false);
        }

        public static T CloneInstance<T>(this T me, bool isCloningMe) where T : class, new()
        {
            T instance = Activator.CreateInstance<T>();
            me.UpdateInstance<T>(instance, isCloningMe);
            return instance;
        }

        public static void UpdateInstance<T>(this T me, T target) where T : class
        {
            me.UpdateInstance<T>(target, false);
        }

        public static void UpdateInstance<T>(this T me, T target, bool isCloningMe) where T : class
        {
            if (!isCloningMe && (object)me is ICloneable)
            {
                target = ((object)me as ICloneable).Clone() as T;
            }
            else
            {
                foreach (FieldInfo field in me.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    object obj = field.GetValue((object)me);
                    if (obj is ICloneable)
                        obj = (obj as ICloneable).Clone();
                    field.SetValue((object)target, obj);
                }
            }
        }

        public static T CloneByAttributes<T>(this T me) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public static T UpdateByAttributes<T>(this T me, T target) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
