using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using RmsAuto.Common.Configuration;

namespace RmsAuto.Common.Configuration
{
    public class ReferenceCacheConfiguration : ConfigurationSectionSingleton<ReferenceCacheConfiguration>
    {
        public override string SectionName
        {
            get { return "rmsauto.acctg/referenceCache"; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(ReferenceConfigurationElementCollection))]
        public ReferenceConfigurationElementCollection Settings
        {
            get { return this[""] as ReferenceConfigurationElementCollection; }
        }
    }

    public class ReferenceConfigurationElement : ConfigurationElement
    {
        public ReferenceConfigurationElement() { }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("expirationTimeout", IsRequired = true)]
        public int ExpirationTimeout
        {
            get { return Convert.ToInt32(this["expirationTimeout"]); }
            set { this["expirationTimeout"] = value; }
        }

        [ConfigurationProperty("connectTimeout", IsRequired = false)]
        public int ConnectTimeout
        {
            get { return Convert.ToInt32(this["connectTimeout"]); }
            set { this["connectTimeout"] = value; }
        }

        [ConfigurationProperty("errorCachingTimeout", IsRequired = true)]
        public int ErrorCachingTimeout
        {
            get { return Convert.ToInt32(this["errorCachingTimeout"]); }
            set { this["errorCachingTimeout"] = value; }
        }
    }

    public class ReferenceConfigurationElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ReferenceConfigurationElement this[string referenceName]
        {
            get
            {
                return (ReferenceConfigurationElement)this.BaseGet(referenceName);

            }
            set
            {
                if (this.BaseGet(referenceName) != null)
                {
                    BaseRemove(referenceName);
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ReferenceConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReferenceConfigurationElement)element).Name;
        }
    }

}
