using System.Diagnostics;

namespace ServiceLocator
{
    public class MyServiceImplementation : IMyService
    {
        public void Execute()
        {
            Debug.WriteLine("MyServiceImplementation.Execute()");
        }
    }
}
