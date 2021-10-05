using System;
public class Disposer : IDisposable
{
    public Disposer()
    {
        //Console.WriteLine("+ {0} was created", this.GetType().Name);
    }

    public void Dispose()
    {
        //Console.WriteLine("- {0} was disposed!", this.GetType().Name);
    }
}