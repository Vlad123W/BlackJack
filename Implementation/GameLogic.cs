using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Microsoft.Extensions.DependencyInjection;

namespace BlackJack.Implementation
{
    public class GameLogic : IGameLogic
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;
        private IActions _actions;

        private bool IsGameJustStarted = true;

        private GraphicInterface? graphInter;
        
        private Stack<IPlayer>? splitHands;
        private decimal? _splitStartingMoney;
        private List<Card> cards = new();

        private readonly IServiceProvider _provider;

        public GameLogic(IPlayer player, IDealer dealer, IActions actions, IServiceProvider provider)
        {
            _player = player;
            _dealer = dealer;
            _actions = actions;
            _provider = provider;

            _actions.Hitted += Actions_Hitted;
        }

        private void Actions_Hitted()
              =>  graphInter?.Print();
        public void BeginGame()
        {
            Initialize();

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
            _player.Hand.PairCards.Clear();
            _dealer.Hand.PairCards.Clear();

            if (cards.Count > 0) _dealer.Refresh();

            cards = _dealer.Shuffle();

            if (cards.Count < 4) throw new InvalidOperationException("Not enough cards to initialize the game.");

            _player.Hand.PairCards.Add(cards[0]);
            _player.Hand.PairCards.Add(cards[1]);

            _dealer.Hand.PairCards.Add(cards[2]);
            _dealer.Hand.PairCards.Add(cards[3]);

            graphInter = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, _player, _dealer, true);
            graphInter.IsDoubleNeeded = true;
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
                    
                   
                    var graphInter = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, _player, _dealer, false);
                    graphInter.WinMessage = "You win! You have a Black Jack!\n";

                    graphInter.Print();
                    
                    return;
                }


                oper = Console.ReadLine()![0];

                if (oper == '0') Environment.Exit(0);

                if (oper == '3' && !graphInter.IsDoubleNeeded) continue;
                if (oper == '4' && !graphInter.IsSplitNeeded) continue;

                switch (oper)
                {
                    case '1':
                        if (_actions.Hit()) return;
                        break;
                    case '2':
                        if (_actions.Stand()) return;
                        break;
                    case '3':
                        if (_actions.Double()) return;
                        break;
                    case '4':
                        _splitStartingMoney = _player.Money;
                        splitHands = new Stack<IPlayer>();
                        _actions.Split(splitHands);
                        SplitCycle();

                        var totalMoney = _player.Money;
                        while (splitHands?.Count > 0) totalMoney += splitHands.Pop().Money;
                        _player.Money = totalMoney;

                        _actions = ActivatorUtilities.CreateInstance<Actions>(_provider, _player, _dealer);
                        graphInter = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, _player, _dealer, true);
                        break;
                    default:
                        break;
                }
            }
        }
        
        private void SplitCycle()
        {
            while (splitHands?.Count != 0)
            {
                IPlayer player = splitHands!.Pop();

                _actions = ActivatorUtilities.CreateInstance<Actions>(_provider, player, _dealer);
                Console.WriteLine($"HAND {splitHands.Count + 1}");
                graphInter = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, player, _dealer, true);
                graphInter.Print();

                bool res = false;

                while (!res)
                {
                    char oper = Console.ReadLine()![0];

                    switch (oper)
                    {
                        case '0':
                            Environment.Exit(0);
                            break;
                        case '1':
                            res = _actions.Hit();
                            break;
                        case '2':
                            res = _actions.Stand();
                            break;
                        case '3':
                            if (graphInter.IsDoubleNeeded) res = _actions.Double();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
