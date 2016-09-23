/*
 * 定义接口，统一操作控件元素的行为。
 * 所有控件都需要实现该接口
 */
using System;

namespace SVCore
{
    public interface SVInterfacePanel
    {
        //将当前属性值设置到控件中，对控件进行的刷新
        void refreshPropertyToPanel();

        //获取当前属性对象
        object property();

        //读配置
        void loadXML(SVXml element, Boolean isCreate = false);

        //写配置
        void saveXML(SVXml element);
    }
}
