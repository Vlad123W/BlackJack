namespace BlackJack.Implementation.Data
{
    /// <summary>
    /// Provides ANSI color codes and formatting utilities for console output.
    /// </summary>
    public static class ConsoleColors
    {
        // ANSI Color Codes
        public const string Reset = "\u001b[0m";

        // Foreground Colors
        public const string Red = "\u001b[31m";
        public const string Green = "\u001b[32m";
        public const string Yellow = "\u001b[33m";
        public const string Blue = "\u001b[34m";
        public const string Magenta = "\u001b[35m";
        public const string Cyan = "\u001b[36m";
        public const string White = "\u001b[37m";
        public const string BrightRed = "\u001b[91m";
        public const string BrightGreen = "\u001b[92m";
        public const string BrightYellow = "\u001b[93m";
        public const string BrightBlue = "\u001b[94m";

        // Background Colors
        public const string BgBlack = "\u001b[40m";
        public const string BgRed = "\u001b[41m";
        public const string BgGreen = "\u001b[42m";
        public const string BgYellow = "\u001b[43m";
        public const string BgBlue = "\u001b[44m";

        // Text Styles
        public const string Bold = "\u001b[1m";
        public const string Dim = "\u001b[2m";
        public const string Italic = "\u001b[3m";
        public const string Underline = "\u001b[4m";

        /// <summary>
        /// Wraps text with a color code.
        /// </summary>
        public static string ColorText(string text, string color) => $"{color}{text}{Reset}";

        /// <summary>
        /// Wraps text with bold formatting.
        /// </summary>
        public static string BoldText(string text) => $"{Bold}{text}{Reset}";

        /// <summary>
        /// Wraps text with bold and color formatting.
        /// </summary>
        public static string BoldColorText(string text, string color) => $"{Bold}{color}{text}{Reset}";

        /// <summary>
        /// Creates a colored box title.
        /// </summary>
        public static string CreateBoxTitle(string title, string color)
        {
            int padding = (40 - title.Length) / 2;
            return $"{BrightBlue}┌──────────────────────────────────────┐{Reset}\n" +
                   $"{BrightBlue}│{Reset}{new string(' ', padding)}{BoldColorText(title, color)}{new string(' ', 40 - title.Length - padding)}{BrightBlue}│{Reset}";
        }

        /// <summary>
        /// Creates a separator line.
        /// </summary>
        public static string CreateSeparator() => $"{BrightBlue}├──────────────────────────────────────┤{Reset}";

        /// <summary>
        /// Creates bottom border.
        /// </summary>
        public static string CreateBottomBorder() => $"{BrightBlue}└──────────────────────────────────────┘{Reset}";
    }
}
