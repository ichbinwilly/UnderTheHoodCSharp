# Problem to be solved
Enumeration is not allowed "-" (hyphen) "." (dot) in the variables. You can't define a Enumeration class example below.

```csharp
public enum Phase
{
    L1, //valid variable
    L2, //valid variable
    L3, //valid variable
    N,  //valid variable
    L1-N, //Syntax Error '-' is not invalid
    L2-N, //Syntax Error '-' is not invalid
    L3-N, //Syntax Error '-' is not invalid
    L1-L2, //Syntax Error '-' is not invalid
    L2-L3, //Syntax Error '-' is not invalid
    L3-L1 //Syntax Error '-' is not invalid
}
```

# EnumMemberAttribute

Refers to [MSDN](https://msdn.microsoft.com/zh-tw/library/system.runtime.serialization.enummemberattribute(v=vs.110).aspx), EnumMemberAttribute enables fine control of the names of the enumerations as they are serialized. Of course, I can take advantage of this annotation on the varaible in the enum.

# How to fix it

Set up a EumMember attribute on L1N instead, and its value with L1-N.

```csharp
public enum Phase
{
    L1,
    L2,
    L3,
    N,
    [EnumMember(Value = "L1-N")]
    L1N,
    [EnumMember(Value = "L2-N")]
    L2N,
    [EnumMember(Value = "L3-N")]
    L3N,
    [EnumMember(Value = "L1-L2")]
    L1L2,
    [EnumMember(Value = "L2-L3")]
    L2L3,
    [EnumMember(Value = "L3-L1")]
    L3L1
}
```
# JSON Serialization Example
```csharp
ElectricPower power = new ElectricPower()
{
    Phase = Phase.L1L2
};

JsonSerializerSettings settings = new JsonSerializerSettings()
{
    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
    Converters = new List<JsonConverter> { new StringEnumConverter { CamelCaseText = true } }, //Specified CamelCaseText StringConverter
    NullValueHandling = NullValueHandling.Ignore
};

Console.WriteLine(JsonConvert.SerializeObject(power, settings));
```

> Result  
>     {"phase":"L1-L2"}