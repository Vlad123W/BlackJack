using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;

namespace BlackJack.Implementation
{
    public class GameLogic : IGameLogic
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;
        private IActions _actions;

        private bool IsGameJustStarted = true;

        private GraphicInterface? graphInter;
        
        private readonly Delegate _hit;
        private readonly Delegate _stand;
        private readonly Delegate _double;
        private readonly Delegate _split;

        private readonly Dictionary<char, Delegate>? acts = [];

        private Stack<IPlayer>? splitHands;
        private List<Card>? cards = [];

        public GameLogic(IPlayer player, IDealer dealer, IActions actions)
        {
            _player = player;
            _dealer = dealer;
            _actions = actions;

            _hit = actions.Hit;
            _stand = actions.Stand;
            _double = actions.Double;
            _split = actions.Split;

            _actions.Hitted += Actions_Hitted;
        }
        private void Actions_Hitted()
              =>  graphInter?.Print();
        public void BeginGame()
        {
            Initialize();

            acts.Add('1', _hit);
            acts.Add('2', _stand);
            acts.Add('3', _double);
            acts.Add('4', _split);

            while(true)
            {
                MainCycle();
                
                if (IDealer.EndTheGame) break;
                
                Initialize();
               
                IsGameJustStarted = true;
            }
        }

        private void Initialize()
        {
            if (_player.Hand.PairCards.Count != 0) _player.Hand.PairCards.Clear();
            if (_dealer.Hand.PairCards.Count != 0) _dealer.Hand.PairCards.Clear();
            if (cards.Count != 0) _dealer.Refresh();

            cards = _dealer.Shuffle();

            for (int i = 0; i < 2; i++)
            {
                _player.Hand.PairCards.Add(cards[i]);
                _dealer.Hand.PairCards.Add(cards[i + 2]);
            }

            graphInter = new(_player, _dealer, [true]) { IsDoubleNeeded = true };
        }

        private void MainCycle()
        {
            char oper = ' ';
            
            while (true)
            {
                if(IsGameJustStarted)
                {
                    Console.Write("Make a bet -> ");
                    _player.Bet = decimal.Parse(Console.ReadLine()!);

                    if (_player.Hand.PairCards.All(x => x.Title.Contains('A'))) _player.Hand.PairCards[0].Cost = 1;

                    if (_player.Hand.PairCards[0].Title[0] == _player.Hand.PairCards[1].Title[0])
                    {
                        graphInter.IsSplitNeeded = true;
                        graphInter.IsDoubleNeeded = true;

                        graphInter.Print();
                    }
                    else
                    {
                        graphInter.Print();
                    }
                    
                    IsGameJustStarted = false;
                }

                if (Conditions.IsBlackJack(_player.Hand))
                {
                    _player.ChangeMoney(_player.Bet * 1.5m);
                    
                   
                    GraphicInterface graphInter = new(_player, _dealer, [false])
                    {
                        WinMessage = "You win! You have a Black Jack!\n"
                    };

                    graphInter.Print();
                    
                    return;
                }


                oper = Console.ReadLine()![0];

                if (oper == '0' || !acts.ContainsKey(oper)) Environment.Exit(0);

                if (acts[oper].Method.Name != "Split" 
                    && graphInter.MenuString.Contains(acts[oper].Method.Name) 
                    && (bool)acts![oper].DynamicInvoke()!) return;
                else if (acts[oper].Method.Name == "Split" && graphInter.MenuString.Contains(acts[oper].Method.Name))
                {
                    splitHands = [];
                    acts[oper].DynamicInvoke(splitHands);
                    SplitCycle();
                    _actions = new Actions(_player, _dealer);
                    graphInter = new GraphicInterface(_player, _dealer, [true]);
                }
            }
        }
        
        private void SplitCycle()
        {
            while (splitHands?.Count != 0)
            {
                Player player = (Player)splitHands?.Pop()!;

                _actions = new Actions(player, _dealer);
                Console.WriteLine($"HAND {splitHands?.Count + 1}");

                graphInter = new GraphicInterface(player, _dealer, [true]);
                graphInter.Print();

                bool res = false;

                while (!res)
                {
                    char oper = Console.ReadLine()![0];
                    res = (bool)acts![oper].DynamicInvoke()!;
                }
            }
        }
    }
}
