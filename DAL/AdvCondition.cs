using System.ComponentModel;

namespace DAL
{
    public enum AdvCondition
    {
        [Description("���")]
        All = 0,

        [Description("�/�")]
        Used = 1,

        [Description("�����")]
        New = 2
    }
}