using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 可选择接口
    /// </summary>
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}
