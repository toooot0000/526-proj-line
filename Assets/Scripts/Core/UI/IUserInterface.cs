namespace Core.UI{
    public interface IUserInterface{
        public void Open();
        public void Close();

        public string Name{ get; }
    }
}