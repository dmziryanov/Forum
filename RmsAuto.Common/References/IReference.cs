using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.References
{
    public interface IReference
    {
        string Name { get; }
        string Description { get; }
        IEnumerable Items { get; }
        object this[object key] { get; } 
    }
}
