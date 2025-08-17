using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class GameLogic : IGameLogic
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;
        private IActions _actions;

        private bool IsGameJustStarted;

        private GraphicInterface? giOpened;
        private GraphicInterface? giClosed;
        
        private readonly Delegate _hit;
        private readonly Delegate _stand;
        private readonly Delegate _double;
        private readonly Delegate _exit;
        private readonly Delegate _split;

        private readonly Dictionary<char, Delegate>? acts = [];

        private ObservableCollection<IPlayer>? splitHands;
        private List<Card>? cards = [];

        public GameLogic(IPlayer player, IDealer dealer, IActions actions)
        {
            _player = player;
            _dealer = dealer;
            _actions = actions;
            _hit = actions.Hit;
            _stand = actions.Stand;
            _double = actions.Double;
            _exit = actions.Exit;
            _split = actions.Split;

            IsGameJustStarted = true;
        }

        public void BeginGame()
        {
            Initialize();

            acts.Add('1', _hit);
            acts.Add('2', _stand);
            acts.Add('3', _double);
            acts.Add('4', _split);
            acts.Add('0', _exit);

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
        }

        private void MainCycle()
        {
            char oper = ' ';
            
            while (oper != '0')
            {
                if(IsGameJustStarted)
                {
                    Console.Write("Make a bet -> ");
                    _player.Bet = decimal.Parse(Console.ReadLine()!);
                }

                if (Conditions.IsBlackJack(_player.Hand))
                {
                    _player.ChangeMoney(_player.Bet * 1.5m);
                   
                    giOpened = new GraphicInterface(_player, _dealer, [false])
                    {
                        WinMessage = "You win! You have a Black Jack!\n"
                    };

                    giOpened.Print();
                    
                    return;
                }

                if(IsGameJustStarted)
                {
                    if (_player.Hand.PairCards.All(x => x.Title.Contains('A'))) _player.Hand.PairCards[0].Cost = 1;

                    if (_player.Hand.PairCards[0].Title[0] == _player.Hand.PairCards[1].Title[0])
                    {
                        giClosed = new GraphicInterface(_player, _dealer, [true])
                        {
                            IsSplitNeeded = true,
                            IsDoubleNeeded = true
                        };

                        giClosed.Print();
                    }
                    else
                    {
                        giClosed = new GraphicInterface(_player, _dealer, [true])
                        {
                            IsDoubleNeeded = true
                        };

                        giClosed.Print();
                    }
                    
                    IsGameJustStarted = false;
                }

                oper = Console.ReadLine()![0];

                if (acts[oper].Method.Name != "Split" && giClosed.MenuString.Contains(acts[oper].Method.Name) && (bool)acts![oper].DynamicInvoke()!) return;
                else if (acts[oper].Method.Name == "Split" && giClosed.MenuString.Contains(acts[oper].Method.Name))
                {
                    splitHands = [];
                    acts[oper].DynamicInvoke(splitHands);
                    SplitCycle(splitHands.Count);
                    _actions = new Actions(_player, _dealer);
                }
            }
        }

        private void SplitCycle(int handCount)
        {
            if (handCount == 0) return;

            SplitCycle(handCount - 1);

            _actions = new Actions(splitHands[handCount - 1], _dealer);
            Console.WriteLine("HAND " + handCount);

            while (true)
            {
                giClosed = new(splitHands![handCount - 1], _dealer, [true]);
                giClosed.Print();

                char oper = Console.ReadLine()![0];
                if ((bool)acts![oper].DynamicInvoke()!) return;
            }
        }
    }
}
