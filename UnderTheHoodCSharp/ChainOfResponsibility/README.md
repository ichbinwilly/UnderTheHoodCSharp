# Chain of Responsibility Pattern
---
The Chain of Responsibility Pattern is a design pattern that avoids coupling the sender of a request to the receiver by giving one or many handler to handle the request. 

A real-world example shown below the Chain of Responsibility pattern where the chained managers and executives can agree the purchase request or hand it off to a superior if they cannot approve.  Each position has its own permission which budget they can approve.

|Name	  |Position					| Budget Permission|
|:-------:|:------------------------:|:--------:|
|Alex|Team Leader				| <=$5000|
|Charles|Engineer Manager		|<=$10000|
|Hugh|Chief Financial Officer	|>$10000|

A Budget request should be approved by the managers (Team Leader -> Engineer Manager -> Chief Financial Officer)

## Concept

 1. The logic is encapsulated in decoupled components – Manager.
 2. Manager is independent in the application.
 3. The resposibility is chained from manager to manager.

In a Manager class, it contains a superior and name property. Take a look at the design diagram. When the client invokes RequestApplications method on concrete Manager implementation, it invokes underneath generic implementation of the method of its base
```csharp
public abstract class Manager
{
    public  Manager Superior { get; set; }
    public string Name { get; set; }

    protected Manager(string name)
    {
        this.Name = name;
    }

    public void SetSuperior(Manager superior)
    {
        this.Superior = superior;
    }

    public virtual void RequestApplications(PurchaseRequest request)
    {

    }
}
```

* Create concrete implementations - A Team Leader can approve the purchase cost under $5000. It's also trivial business logic for Engineer Manager and Chief Financial Officer. 
```csharp
public override void RequestApplications(PurchaseRequest request)
{
	if (request.Cost <= 5000)
	{
		Console.WriteLine($"{Name} approved the purchase request ${request.Cost}");
	}
	else
	{
		Superior?.RequestApplications(request);
	}
	base.RequestApplications(request);
}
```
* Setup the chain of resposibility
```chsharp
TeamLeader teamLeader = new TeamLeader("Alex");
EnginnerManager enginnerManager = new EnginnerManager("Charles");
ChiefFinancialOfficer chiefFinancialOfficer = new ChiefFinancialOfficer("Hugh");
teamLeader.SetSuperior(enginnerManager);
enginnerManager.SetSuperior(chiefFinancialOfficer);
```
			
* Test the solution
```csharp
PurchaseRequest requestA = new PurchaseRequest {Cost = 3000};
PurchaseRequest requestB = new PurchaseRequest { Cost = 10000 };
PurchaseRequest requestC = new PurchaseRequest { Cost = 50000 };
teamLeader.RequestApplications(requestA);
teamLeader.RequestApplications(requestB);
teamLeader.RequestApplications(requestC);
```
* Result
```csharp
//Alex approved the purchase request $3000
//Charles approved the purchase request $10000
//Hugh approved purchase the request $50000
```

## References
1. https://blogs.msdn.microsoft.com/alikl/2008/01/14/chain-of-responsibility-design-pattern-focus-on-security-performance-and-operations/
2. https://www.dofactory.com/net/chain-of-responsibility-design-pattern
3. https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern
4. https://wizardforcel.gitbooks.io/design-pattern-lessons/content/lesson24.html

## Further reading - 

ASP.NET uses Chain of Responsibility design principle for web request handling. Each web request goes through the chain of handlers (called middleware) a.k.a. ASP.NET HTTP Pipeline - HTTP Modules and Handlers. Each handler might do something with request and decide if request should go to the next handler in a chain or not.

![ASP.NET HTTP Pipeline](https://blogs.sans.org/appsecstreetfighter/files/2009/06/httpmodule.jpg)

1. https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/lifecycle-of-an-aspnet-mvc-5-application
2. https://msdn.microsoft.com/en-us/library/bb470252.aspx#Life%20Cycle%20Stages
3. https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.0/ms227673(v%3dvs.85)
4. https://www.cnblogs.com/fish-li/archive/2013/01/04/2844908.html
5. https://software-security.sans.org/blog/2009/06/14/session-attacks-and-aspnet-part-1