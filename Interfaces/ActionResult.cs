namespace BlackJack.Interfaces
{
    /// <summary>
    /// Result returned by action methods to indicate effects on game flow and UI.
    /// </summary>
    public sealed record ActionResult(bool RoundEnded, bool NeedRedraw, string? Message = null);
}