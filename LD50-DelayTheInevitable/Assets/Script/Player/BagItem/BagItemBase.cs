using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 背包物品的基类
/// </summary>
public abstract class BagItemBase
{
    public string des 
    { get
        {
            switch (MultLanguageUtility.GetLanguageTag())
            {
                case eMultLanguageTag.ZH:
                    return des_CN;
                case eMultLanguageTag.EN:
                    return des_CN;
                default:
                    return des_CN;
            }
        } 
    }
    protected abstract string des_CN { get; }
    protected abstract string des_EN { get; }
    public abstract string resName { get; }
    public int num { set; get; }
}

//public enum eInterActTag
//{
        
//}
