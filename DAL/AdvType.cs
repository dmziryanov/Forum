using System.ComponentModel;

namespace DAL
{
    public enum AdvType
    {
        [Description("���")]
        All = 0,
        [Description("������")]
        Business = 1,
        [Description("�������")]
        Private = 2,
    }
}