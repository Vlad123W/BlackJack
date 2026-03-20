namespace BlackJack.Implementation.Entities
{
    public class Card
    {
        private string? title = string.Empty;
        public string Title
        {
            get => !IsHidden ? title! : "?";

            set => title = value;
        }

        public int Cost { get; set; } = 0;

        public bool IsHidden { get; set; } = false;

        public override string ToString() => $"{Title}:{Cost}";
    }
}
