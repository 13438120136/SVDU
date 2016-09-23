/*
 * 增加数据改动能够正确的保护机制
 */

namespace SVCore
{
    public class SVAutoSave
    {
        //判断当前数据是否保存
        public bool _IsSave { get; private set; }

        public SVAutoSave()
        {
            initSaveStatus();
        }
        
        //初始化为没有保存
        public void initSaveStatus()
        {
            if (_IsSave)
                _IsSave = false;
        }

        //删除的时候保存
        public void saveSelf()
        {
            if (!_IsSave)
            {
                save();
                _IsSave = true;
            }
        }

        //用来继承的子类实现具体的保存功能
        protected virtual void save()
        {
        }
    }
}
