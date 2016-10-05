using System;
using System.Runtime.InteropServices;

namespace SVCore
{
    static public class SVLimit
    {
        public const Int32 MAX_PAGE_NUMBER = 500;
        public const Int32 PAGE_TITILE_MAXLEN = 128 * 2;
        public const Int32 PAGE_BTN_MAXNUM = 200;
        public const Int32 PAGE_AREA_MAXNUM = 160;
        public const Int32 PAGE_ICON_MAXNUM = 80;
        public const Int32 PAGE_LINE_MAXNUM = 160;
        public const Int32 PAGE_GIF_MAXNUM = 40;
        public const Int32 PAGE_BOOL_MAXNUM = 160;
        public const Int32 PAGE_ANA_MAXNUM = 160;
        public const Int32 PAGE_TCHART_MAXNUM = 40;
        public const Int32 PAGE_TICKGIF_MAXNUM = 10;

        public const Int32 TEXT_MAX_LEN = 64 * 2;
        public const Int32 AREA_MAX_LEN = 64 * 2;
        public const Int32 TREND_LINE_MAX_NUM = 4;
    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 4)]
    public struct btnParaBin
    {
        [FieldOffset(0)]
        public UInt32 pageId;

        [FieldOffset(0)]
        public UInt32 addrOffset;
    }

    public struct RectBin
    {
        public UInt16 sX;
        public UInt16 sY;
        public UInt16 eX;
        public UInt16 eY;
    }

    public struct ButtonBin
    {
        public RectBin rect;
        public UInt32 fontClr;
        public UInt32 bgUpColor;
        public UInt32 bgDownColor;
        public btnParaBin param;
        public UInt32 enableAddrOffset; /*使能变量的地址*/
        public UInt16 id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.TEXT_MAX_LEN)]
        public Byte[] text;
        public Byte type;
        public Byte confirm;
        public Byte font;

        public Byte bgUpFlag;    /*0;使用颜色进行填充，1：使用图片进行填充 针对bgUpColor */
        public Byte bgDownFlag;  /*0;使用颜色进行填充，1：使用图片进行填充 针对bgDownColor */

        public Byte enable;      /*0;不使用使能变量 1：使用使能变量*/
        public Byte varTypeEn;   /*使能变量类型*/
        public Byte varTypeBtn;  /*按钮关联变量类型*/
    }

    public struct AnalogBin
    {
        public RectBin rect;
        public UInt32 addrOffset;
        public UInt32 normalClr;
        public UInt32 overMaxClr;
        public UInt32 overMinClr;

        public UInt32 normalBgClr;
        public UInt32 overMaxBgClr;
        public UInt32 overMinBgClr;

        public UInt32 vinfoInvalid;   //质量位无效的字体颜色
        public UInt32 vinfoInvalidBg; //质量位无效的背景显示颜色

        public Single vMax;
        public Single vMin;

        public UInt16 id;
        public Byte font; /*显示时使用的字体*/
        public Byte nDecimalNum; /*小数点后数据位数*/
        public Byte enExponent; /*是否指数显示*/
        public Byte varType;/*变量类型*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte []rsv;
    }

    public struct BinaryBin
    {
        public RectBin rect;          /*变量在页面上的区域*/
        public UInt32 addrOffset;     /*对应变量的地址*/

        public UInt32 trueClr;
        public UInt32 trueBgClr;

        public UInt32 falseClr;
        public UInt32 falseBgClr;

        public UInt32 vinfoInvalid;   //质量位无效的字体颜色
        public UInt32 vinfoInvalidBg; //质量位无效的背景显示颜色

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.TEXT_MAX_LEN)]
        public Byte[] trueText;   /*真时自定义文本*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.TEXT_MAX_LEN)]
        public Byte[] falseText;   /*假时自定义文本*/

        public UInt16 id;   /*Bool型变量的显示元素Id*/
        public Byte font;   /*显示变量值时采用的字体*/

        /// <summary>
        /// 
        /// BINARY_OPEN_CLOSE,
        /// BINARY_RUN_STOP,
        /// BINARY_ONE_ZERO,
        /// BINARY_YES_NO,
        /// BINARY_TRUE_FALSE,
        /// BINARY_RIGHT_WRONG,
        /// BINARY_ON_OFF,
        /// BINARY_CUSTOM ///自定义类型
        /// </summary>
        public Byte type;   ///类型

        public Byte varType;/*变量类型*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Byte []rsv;
    }

    public struct AreaBin
    {
        public RectBin rect;            /*AREA的区域*/
        public UInt32 bgClr;            /*AREA的背景颜色*/
        public UInt32 fontClr;          /*AREA的文本颜色*/        
        public UInt16 id;               /*AREA的元素Id*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.AREA_MAX_LEN)]
        public Byte[] text;            /*AREA的文本*/
        public Byte font;              /*AREA绘制文本时使用的字体*/
        public Byte align;             /*AREA绘制文本时使用的对齐方式*/
        public Byte transparent;   //是否透明

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public Byte[] rsv;
    }

    public struct TrendChartBin
    {
	    public RectBin rect;             /*趋势图的区域*/
        public UInt32 bgClr;             /*趋势图的背景颜色*/
        public UInt32 scaleClr;          /*趋势图的坐标以及文本颜色*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.TREND_LINE_MAX_NUM)]
        public UInt32[] addrOffset;      /*趋势图对应的变量偏移地址*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.TREND_LINE_MAX_NUM)]
        public UInt32[] lineClr;         /*趋势图的趋势线颜色*/
        public Single yMin;
        public Single yMax;
        public UInt16 maxTime;           //趋势图的最大时间，单位秒，范围60-3600
        public UInt16 id;                /*的元素Id*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.TREND_LINE_MAX_NUM)]
        public Byte[] lineWidth;         /*趋势线的宽度,该位为非0时表示线段有效，但不能超过4像素。*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.TREND_LINE_MAX_NUM)]
        public Byte[] varType;           /*对应变量的类型*/
        //uint8_t     varType[TREND_LINE_MAX_NUM];
        public Byte font;                /*趋势图绘制文本时使用的字体*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Byte[] rsv;
    }

    public struct TickBin
    {
        public RectBin rect;  /*心跳控件的显示区域*/

        /*图标数据在图标库中的偏移值,最多支持8张图片*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public UInt32[] imageOffsetList;

        public UInt16 id;  /*图标元素的Id*/
        public Byte count; //有效图片个数,范围(1-8)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public Byte[] rsv;
    }

    public struct LineBin
    {
        public UInt32 color; /*线段元素的颜色*/
        public UInt16 id;    /*线段元素的Id*/
        public UInt16 x1;    /*线段元素的起点X坐标*/
        public UInt16 y1;    /*线段元素的起点Y坐标*/
        public UInt16 x2;    /*线段元素的终点X坐标*/
        public UInt16 y2;    /*线段元素的终点Y坐标*/
        public Byte width;   /*线段元素的宽度*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public Byte[] rsv; 
    }

    public struct IconBin
    {
        public RectBin rect;
        public UInt32 imageOffset;

        public UInt16 id; /*图标元素的Id*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] rsv; 
    }

    public struct GifBin
    {
        public RectBin rect;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public UInt32[] addOffset;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public UInt32[] imageOffset;
        public UInt32 iamgeOffsetErr;

        public UInt16 id;         /*动态图标元素的Id*/
        public Byte type;         //动态图的类型

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Byte[] varType;    /*对应变量的类型*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] rsv;
    }

    public struct PageBin
    {
        public UInt32 bgClr;   /*页面的背景色*/        
        //public UInt32 fontClr; /*绘制标题时使用的字体颜色*/       

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_AREA_MAXNUM)]
        public AreaBin[] m_area; /*页面包含的所有AREA元素*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_BTN_MAXNUM)]
        public ButtonBin[] m_btn; /*页面包含的所有按钮*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_LINE_MAXNUM)]
        public LineBin[] m_line; /*页面包含的线段元素*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_ANA_MAXNUM)]
        public AnalogBin[] m_analog; /*页面包含的所有ANALOG元素*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_BOOL_MAXNUM)]
        public BinaryBin[] m_binary; /*页面包含的所有BOOL元素*/


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_TCHART_MAXNUM)]
        public TrendChartBin[] m_trendChart;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_ICON_MAXNUM)]
        public IconBin[] m_icon; /*页面包含的所有图标元素*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_GIF_MAXNUM)]
        public GifBin[] m_gif; /*页面包含的所有动态图标元素*/

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_TICKGIF_MAXNUM)]
        public TickBin[] m_tick;        /*心跳控制，1个*/
        
        public UInt16 id;      /*页面的Id*/
        public UInt16 index;   //当前页面在数组中的索引位置
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.PAGE_TITILE_MAXLEN)]
        //public Byte[] title;   /*页面的标题*/
        //public Byte font;      /*绘制标题时使用的字体*/
        //public Byte align;     /*标题的对齐方式*/
        public Byte pointAlign;  //小数点是否对齐

        public Byte btnNum;  /*页面包含的按钮的实际个数*/
        public Byte areaNum; /*页面包含的AREA的实际个数*/
        public Byte iconNum; /*页面包含的图标的实际个数*/
        public Byte lineNum; /*页面包含的线段的实际个数*/
        public Byte gif_num; /*页面包含的动态图片的实际个数*/
        public Byte analog_num; /*页面包含的ANALOG的实际个数*/
        public Byte binaryNum; /*页面包含的BOOL的实际个数*/
        public Byte trendChartNum; /*页面包含的趋势线的实际个数*/
        public Byte tickNum;       //心跳控件个数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] rsv; 
    }

    public struct PageArrayBin
    {
        public static readonly PageArrayBin Empty;

        public UInt32 pageCount;         /*有效页面数量*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public UInt32[] rsv;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SVLimit.MAX_PAGE_NUMBER)]
        public PageBin[] pageArray;
    }

    static public class SVBinData
    {
        static public void structToByteArray(PageArrayBin bin, ref byte[] result)
        {
            int size = Marshal.SizeOf(bin);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            result = new byte[size];

            Marshal.StructureToPtr(bin, buffer, true);
            Marshal.Copy(buffer, result, 0, size);
            Marshal.FreeHGlobal(buffer);
        }

        static public void byteArrayToStruct(byte[] result, ref PageArrayBin bin)
        {
            //实际大小
            int size = result.Length;
            //结构体大小
            int totalSize = Marshal.SizeOf(bin);
            
            IntPtr buffer = Marshal.AllocHGlobal(totalSize);
            Marshal.Copy(result, 0, buffer, size);
            bin = (PageArrayBin)Marshal.PtrToStructure(buffer, typeof(PageArrayBin));
            Marshal.FreeHGlobal(buffer);
        }
    }
}
