using System.ComponentModel;

namespace DAL
{
    public enum AdvType
    {
        [Description("Все")]
        All = 0,
        [Description("Бизнес")]
        Business = 1,
        [Description("Частное")]
        Private = 2,
    }
}