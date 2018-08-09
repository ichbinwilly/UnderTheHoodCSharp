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