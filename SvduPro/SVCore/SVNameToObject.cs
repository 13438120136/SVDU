/*
 * 应用反射机制，根据配置文件的名称来创建对象实例。
 * 
 * */

using System;
using System.Collections.Generic;

namespace SVCore
{
    public static class SVNameToObject
    {
        static private Dictionary<String, Type> _nameTypeDict;

        static SVNameToObject()
        {
            _nameTypeDict = new Dictionary<String, Type>();
        }

        //添加映射关系
        public static void addInstance(Type value)
        {
            String name = value.Name;

            if (!_nameTypeDict.ContainsKey(name))
                _nameTypeDict.Add(name, value);
        }

        //通过名称创建对象
        public static object getInstance(String name)
        {
            if (!_nameTypeDict.ContainsKey(name))
                return null;

            Type objType = _nameTypeDict[name];
            object resultObj = Activator.CreateInstance(objType);
            return resultObj;
        }
        
        public static object createInstance(String name)
        {
            Type type = Type.GetType(name);
            if (type == null)
                return null;

            object resultObj = Activator.CreateInstance(type);
            return resultObj;
        }
    }
}
