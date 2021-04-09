namespace Player
{
    public class StockableItem
    {
        private string name;

        public StockableItem(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}