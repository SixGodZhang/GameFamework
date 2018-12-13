//-----------------------------------------------------------------------
// <filename>GenAdapterConfig</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/11 星期二 17:27:06# </time>
//-----------------------------------------------------------------------

using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace CodeGenerationTools.Generator
{
    public class WhiteTypeList
    {
        /// <summary>
        /// 系统类型
        /// </summary>
        private static List<Type> systemWhiteTypeList = new List<Type>()
        {
            typeof(System.IDisposable),
        };

        //系统类型程序集
        public static string mscorlib_path = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\mscorlib.dll";

        public static List<TypeDefinition> GetSystemWhiteList()
        {
            List<TypeDefinition> types = new List<TypeDefinition>();
            var module = ModuleDefinition.ReadModule(mscorlib_path);

            var all_types = module.GetTypes();

            //find all typedefines in system whiteList
            foreach (var need in systemWhiteTypeList)
            {
                foreach (var item in all_types)
                {
                    if (item.FullName.Equals(need.FullName))
                        types.Add(item);
                }
            }

            return types;
        }
    }
}
