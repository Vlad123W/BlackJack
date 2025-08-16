using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Interfaces
{
    public interface IGraphicInterface
    {
        string WinMessage { get; set; }
        string MenuString { get; }
        void Print();
    }
}
