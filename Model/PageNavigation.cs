namespace Gevlee.RestTunes.Model
{
    public class PageNavigation
    {
        public int Page { get; set; } = 1;

        public int Size { get; set; } = 10;

        public int Skip => (Page * Size - Size);
    }
}