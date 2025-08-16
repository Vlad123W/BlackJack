using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class GraphicInterface : IGraphicInterface
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;
        
        private readonly string _menuString = "1.Hit 2.Stand 0.Exit";
        
        public string MenuString
        {
            get
            {
                if(IsDoubleNeeded && !IsSplitNeeded)
                {
                    string temp = _menuString;
                    return temp.Insert(temp.IndexOf('0') - 1, " 3.Double");
                }
                else if(!IsDoubleNeeded && IsSplitNeeded)
                {
                    string temp = _menuString;
                    return temp.Insert(temp.IndexOf('0') - 1, " 3.Split");
                }
                else if(IsSplitNeeded && IsDoubleNeeded)
                {
                    string temp = _menuString;
                    return temp.Insert(temp.IndexOf('0') - 1, " 3.Double 4.Split");
                }

                return _menuString;
            }
        }

        public string WinMessage { get; set; } = string.Empty;

        private bool _isSplitNeeded = false;
        public bool IsSplitNeeded { get => _isSplitNeeded; set => _isSplitNeeded = value; }

        private bool _isDoubleNeeded = false;
        public bool IsDoubleNeeded
        {
            get { return _isDoubleNeeded; }
            set { _isDoubleNeeded = value; }
        }

        public GraphicInterface(IPlayer player, IDealer dealer,  params bool[] p)
        {
            _player = player;
            _dealer = dealer;
            _dealer.Hand.PairCards[1].IsHidden = p[0];
        }

        public void Print()
        {
            Console.WriteLine("\n=========TABLE==========\n");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Your score: " + _player.Hand.GetScore());
            _player.Hand.Show();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");

            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Dealer:");
            _dealer.Hand.Show();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");

            Console.WriteLine(WinMessage);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Money: {_player.Money}");
            Console.WriteLine(MenuString);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
        }
    }
}
