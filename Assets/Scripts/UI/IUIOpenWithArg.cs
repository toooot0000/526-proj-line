namespace UI{
    public interface IUIOpenWithArg<in T>{
        void Open(T args);
    }
}