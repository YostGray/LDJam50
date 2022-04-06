using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface ICouldInvestage
{
    public void Investage();
}

public enum InvestageType
{
    normal = 1,
    import = 2,
    emergency = 3,
}
