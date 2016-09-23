/*
 * 生成编译下装的二进制文件的接口
 * 所有需要生成对应二进制文件内容对象都要实现该接口
 * */
namespace SVCore
{
    public interface SVInterfaceBuild
    {
        //产生数据
        void buildControlToBin(ref PageArrayBin n, ref SVSerialize serialize);
    }
}
