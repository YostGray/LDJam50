using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ICouldInvestage
{
    public void Investage(PlayerController playerController);
}

public enum InvestageType
{
    normal = 1,
    import = 2,
    emergency = 3,
}
