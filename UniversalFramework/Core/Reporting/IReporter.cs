namespace Unicorn.Core.Reporting
{
    public interface IReporter
    {
        void Init();


        void Report(string info);


        void Complete();

    }
}
