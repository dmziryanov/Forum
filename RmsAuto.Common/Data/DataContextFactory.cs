using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Configuration;
using RmsAuto.Common.Misc;

namespace RmsAuto.Common.Data
{
	[Obsolete()]
    public static class DataContextFactory
    {
        public readonly static Dictionary<Type, String> _connectionStrings = new Dictionary<Type, string>();
        public readonly static object _locker = new object();

        public static T Create<T>() where T : DataContext, new()
        {
            String cString = null;
            Type dcType = typeof(T);

            lock (_locker)
            {
                _connectionStrings.TryGetValue(dcType, out cString);
                if (cString == null)
                {
                    var attr = dcType.GetCustomAttributes(false).FirstOrDefault(a => a is ConnectionStringAttribute) as ConnectionStringAttribute;
                    if (attr == null)
                    {
                        throw new ArgumentException("Запрошеный контекст не декорирован атрибутом " +
                                                    "RmsAuto.Common.Data.ConnectionStringAttribute.");
                    }
                    try
                    {
                        cString = ConfigurationManager.ConnectionStrings[attr.ConnectionString].ConnectionString;
                        _connectionStrings.Add(dcType, cString);
                    }
                    catch (NullReferenceException)
                    {
                        throw new ConfigurationErrorsException("Задайте в конфигурационном файле необходимую строку подключения.");
                    }
                }

                T ret = new T();
                ret.Connection.ConnectionString = cString;

                return ret;
            }
        }
    }
}
