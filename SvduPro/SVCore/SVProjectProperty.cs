using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SVCore
{
    public class SVProjectProperty
    {
        [CategoryAttribute("站属性")]
        [DisplayName("站ID")]
        [DescriptionAttribute("显示当前站的ID号")]
        [ReadOnlyAttribute(true)]
        public String StationID { get; set; }

        [CategoryAttribute("站属性")]
        [DisplayName("站名称")]
        [DescriptionAttribute("显示当前站的名称")]
        [ReadOnlyAttribute(true)]
        public String StationName { get; set; }

        [CategoryAttribute("站属性")]
        [DisplayName("数据库IP")]
        [DescriptionAttribute("连接数据库的IP地址")]
        [ReadOnlyAttribute(true)]
        public String DataBaseIP { get; set; }

        [CategoryAttribute("站属性")]
        [DisplayName("连接用户")]
        [DescriptionAttribute("连接数据库的用户名称")]
        [ReadOnlyAttribute(true)]
        public String DataBaseName { get; set; }

        [CategoryAttribute("站属性")]
        [DisplayName("工程路径")]
        [DescriptionAttribute("存放当前工程的实际路径")]
        [ReadOnlyAttribute(true)]
        public String StationPath { get; set; }
    }
}
