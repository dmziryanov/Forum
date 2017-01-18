using System.ComponentModel;

namespace DAL
{
    public enum AdvCondition
    {
        [Description("Все")]
        All = 0,

        [Description("Б/У")]
        Used = 1,

        [Description("Новый")]
        New = 2
    }
}