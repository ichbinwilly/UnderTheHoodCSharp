## AddSumLibrary Example 
```csharp
namespace AddSumLibrary
{
    public class AddSum
    {
        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public AddSum(int num1, int num2)
        {
            Num1 = num1;
            Num2 = num2;
        }
        public int GetSum()
        {
            return Num1 + Num2;
        }
    }
}
```
## Main Example 
```csharp
class Program
{
	static void Main()
	{
		var asm = Assembly.LoadFile(@"C:\AddSumLibrary.dll");
		var type = asm.GetType("AddSumLibrary.AddSum"); //Namespace.ClassName
		var constructorObjects = new object[] { 10 ,5 };
		dynamic runnable = Activator.CreateInstance(type, constructorObjects);
		
		var method = runnable.GetType().GetMethod("GetSum");
		var result = method?.Invoke(runnable, null);
		Console.WriteLine("***Call *GetSum* Method using invoke***");
		Console.WriteLine(result);

		Console.WriteLine("***Call *GetSum* Method using dynamic***");
		Console.WriteLine(runnable.GetSum());
		Console.ReadKey();
	}
}
```