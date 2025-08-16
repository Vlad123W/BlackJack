using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Card
    {
        private string? title;
        public string Title
        {
            get
            {
                if(!IsHidden)
                {
                    return title!;
                }

                return "?";
            }

            set => title = value;
        }
        
        public int Cost { get; set; } = 0;

        public bool IsHidden { get; set; } = false;

        public override string ToString() => $"{Title}:{Cost}";
    }
}
