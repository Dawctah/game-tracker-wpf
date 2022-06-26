namespace Core.Commands
{
    public interface ICommand<T>
    {
        T Execute();
    }
}
