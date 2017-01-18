using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RmsAuto.Common.Configuration
{
    public abstract class ConfigurationSectionBase : ConfigurationSection
    {
        public abstract string SectionName { get; }
    }

    /// <summary>
    /// Класс-обертка для аспнетдванольных конфигурационных секций
    /// PS: Аспнетдванольные секции легко подменяются и наследуются
    /// </summary>
    /// <typeparam name="T">Генерик тип - собственно тип секции-наследника</typeparam>
    public abstract class ConfigurationSectionSingleton<T> : ConfigurationSectionBase
        where T : ConfigurationSectionBase, new()
    {
        #region Public methods & properties

        /// <summary>
        /// Зарефрешить секцию
        /// </summary>
        public static void RefreshSection()
        {
            CurrentSections.Remove(typeof(T));
            Get();
        }

        /// <summary>
        /// Синглтон-проперть для получения секции
        /// </summary>
        public static T Current
        {
            get
            {
                lock (_locker)
                {
                    return Get();
                }
            }
        }

        #endregion

        #region Private methods

		private readonly static object _locker = new object();

        private static Dictionary<Type, T> CurrentSections = new Dictionary<Type, T>();

        private static T Get()
        {
            T currentSection = null;
            if (!CurrentSections.TryGetValue(typeof(T), out currentSection))
            {
                currentSection = ConfigurationManager.GetSection(new T().SectionName) as T;
                CurrentSections.Add(typeof(T), currentSection);
            }
            return currentSection;
        }

        #endregion

        protected V TryGet<V>(string keyValue, V defValue)
        {
            try
            {
                var val = this[keyValue];
                V retVal = (V)Convert.ChangeType(val, typeof(V));
                return retVal.Equals(default(V)) ? defValue : retVal;
            }
            catch (InvalidCastException)
            {
                return defValue;
            }
            catch (FormatException)
            {
                return defValue;
            }
            catch (NullReferenceException)
            {
                return default(V);
            }
        }
        protected V TryGet<V>(string keyValue)
        {
            return TryGet<V>(keyValue, default(V));
        }

        protected void Set(string name, object val)
        {
            this[name] = val;
        }
    }
}
