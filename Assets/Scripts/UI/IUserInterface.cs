namespace UI{
    public interface IUserInterface{
        public void Open();
        public void Close();
        public string Name{ get; }

        public event UINormalEvent OnOpen;

        public event UINormalEvent OnClose;

    }
}