using System.Collections.ObjectModel;

namespace BlackJack.Interfaces
{
    public interface IActions
    {
        delegate void Notify();
        event Notify? Hitted;
        event Notify? GameEnded;
        
        bool Hit();
        bool Stand();
        bool Double();
        void Split(Stack<IPlayer> splitHands);
    }
}