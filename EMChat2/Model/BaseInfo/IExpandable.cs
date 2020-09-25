using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    public interface IExpandable
    {
        bool IsExpanded { get; set; }
    }
}
