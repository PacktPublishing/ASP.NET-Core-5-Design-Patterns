using System.Diagnostics;

namespace ServiceLocatorFixed
{
    public class MyServiceImplementation : IMyService
    {
        public void Execute()
        {
            Debug.WriteLine("MyServiceImplementation.Execute()");
        }
    }



}
