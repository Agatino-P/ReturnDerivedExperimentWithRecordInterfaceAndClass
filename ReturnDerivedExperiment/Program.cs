using System.Data;
using static System.Console;

namespace ReturnDerivedExperiment;

internal class Program
{
    static void Main(string[] args)
    {
        Son s = new("son","1 meter");
        Daughter d = new("dgtr","blonde");
        WriteLine(s.RequiresPreProcessing());
        WriteLine(d.RequiresPreProcessing());


        //this two fail because RequiresPreProcessing   is seen as not implemented
        //looks like the default implementation is not carried along
        SonI si = new("son", "1 meter");
        DaughterI di = new("dgtr", "blonde");
        //WriteLine(si.RequiresPreProcessing());    
        //WriteLine(di.RequiresPreProcessing());    

        //Will not work with class and interface, you get the Interface method if the variable is InterfaceType
        //  or the class method if the variable is declared as class
        SonC sc = new SonC("son", "1 meter");
        IBaseInterfaceC dc = new DaugtherC("dgtr", "blonde");
        //WriteLine(sc.RequiresPreProcessing());
        //WriteLine(dc.RequiresPreProcessing());
    }

}

//Will work with abstract record

public abstract record BaseRecord<T> (string LastName) where T : BaseRecord<T>
{
    protected virtual BaseRecord<T> PreProcess()=>this;

    public string RequiresPreProcessing() 
        => PreProcess().DoSomething();

    private string DoSomething() 
        => string.Join(", ", GetType().GetProperties().Select(p => $"{p.Name}: {p.GetValue(this)}"));
}

public record Son (string LastName, string Height): BaseRecord<Son> (LastName)
{
    protected override Son PreProcess() => new (LastName.ToUpper(), Height);
}

public record Daughter(string LastName, string Color) : BaseRecord<Daughter>(LastName);


//Will not work with record and interface

public interface IBaseInterfaceR<T> where T : IBaseInterfaceR<T>
{
    protected virtual IBaseInterfaceR<T> PreProcess() => this;

    public string RequiresPreProcessing()
        => PreProcess().doSomething();

    protected string doSomething()
        => string.Join(", ", GetType().GetProperties().Select(p => $"{p.Name}: {p.GetValue(this)}"));
}

public record SonI(string LastName, string Height) : IBaseInterfaceR<SonI>
{
    protected SonI PreProcess() => new(LastName.ToUpper(), Height);
}

public record DaughterI(string LastName, string Color) : IBaseInterfaceR<DaughterI>;


//Will not work with class and interface, you get the Interface method if the variable is InterfaceType
//  or the class method if the variable is declared as class

public interface IBaseInterfaceC
{
    protected virtual IBaseInterfaceC PreProcess() => this;

    public string RequiresPreProcessing()
        => PreProcess().doSomething();

    protected string doSomething()
        => string.Join(", ", GetType().GetProperties().Select(p => $"{p.Name}: {p.GetValue(this)}"));
}

public class SonC : IBaseInterfaceC
{
    public SonC(string lastName, string height)
    {
        LastName = lastName;
        Height = height;
    }

    public string LastName { get; init; } = default!;
    public string Height { get; init; } = default!;

    protected SonC PreProcess() => new(LastName.ToUpper(), Height);
}

public class DaugtherC : IBaseInterfaceC
{
    public DaugtherC(string lastName, string color)
    {
        LastName = lastName;
        Color = color;
    }

    public string LastName { get; set; } = default!;
    public string Color { get; set; } = default!;
}

