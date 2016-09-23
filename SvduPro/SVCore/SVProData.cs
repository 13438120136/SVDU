﻿using System;

namespace SVCore
{
    public static class SVProData
    {
        //工程名称
        static public String ProName { get; set; }
        //当前工程路径
        static public String ProPath { get; set; }
        //工程完整路径
        static public String FullProPath { get; set; }
        //模板完整路径
        static public String TemplatePath { get; set; }
        //当前配置文件的路径
        static public String ConfigFile { get; set; }
        //当前下装文件生成的目录
        static public String DownLoadFile { get; set; }
        //图标管理路径
        static public String IconPath { get; set; }
        //保存文件日志路径
        static public String LogPath { get; set; }
    }
}
