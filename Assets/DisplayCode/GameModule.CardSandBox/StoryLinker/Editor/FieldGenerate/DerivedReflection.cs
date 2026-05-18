using System;
using System.Collections.Generic;
using System.Reflection;

public class DerivedReflection
    {
        public static Type[] GetDerivedTypes(Type baseType)
        {
            if (baseType == null)
            {
                throw new ArgumentException("必须传入一个有效的基类类型");
            }

            List<Type> derivedTypes = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    // 修复：使用 else if 避免重复添加
                    if (baseType.IsInterface)
                    {
                        if (baseType.IsAssignableFrom(type) && !type.IsInterface)
                        {
                            derivedTypes.Add(type);
                        }
                    }
                    else if (baseType.IsAssignableFrom(type) && type != baseType)
                    {
                        derivedTypes.Add(type);
                    }
                }
            }

            return derivedTypes.ToArray();
        }
    }
