using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 过程数据
/// </summary>
public class Process
{
    public int totalCostTime = 0;//完成花费的总秒数 为负数说明不需要计时
    public int passedTime = 0;//已经经过的时间

    public Dictionary<eMaterialType, float> costPreSecond = new Dictionary<eMaterialType, float>();//每秒消耗数量 不需要计时则为一次性
    public Dictionary<eMaterialType, float> outputPreSecond = new Dictionary<eMaterialType, float>();//每秒产出数量 不需要计时则为一次性

    public Action canNotProcessAction;
}
