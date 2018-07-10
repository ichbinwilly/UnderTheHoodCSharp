# Notes
[Enum Converter](https://github.com/ichbinwilly/UnderTheHoodCSharp/blob/master/README.md "Enum Converter")
[Dynamic Load Assembly](https://github.com/ichbinwilly/UnderTheHoodCSharp/blob/master/UnderTheHoodCSharp/DynamicLoadAssembly/README.md "Dynamic Load Assembly")

# Enum Converter

# Problem to be solved
Imaging you are implementing a specification, you may see the following definition. A Phase is consist of different type of values. Our aim is to have the  output json object {"phase":"L1-L2"}.

|Phase|
|-----|
|L1   |
|L2   |
|L3|
|N|
|L1-N|
|L2-N|
|L3-N|
|L1-L2|
|L2-L3|
|L3-L1|

Let's assume that the best choice to restore the Phase is to use the Enumeration. You will immediately be frustrated with the first try. Enumeration is not allowed "-" (hyphen) "." (dot) the declaration in the variables. Therefore, you can not define a Enumeration class example below.

```csharp
public enum Phase
{
    L1, //valid variable
    L2, //valid variable
    L3, //valid variable
    N,  //valid variable
    L1-N, //Syntax Error '-' is INVALID
    L2-N, //Syntax Error '-' is INVALID
    L3-N, //Syntax Error '-' is INVALID
    L1-L2, //Syntax Error '-' is INVALID
    L2-L3, //Syntax Error '-' is INVALID
    L3-L1 //Syntax Error '-' is INVALID
}
```

# Introduction to EnumMemberAttribute

Refers to [MSDN](https://msdn.microsoft.com/zh-tw/library/system.runtime.serialization.enummemberattribute(v=vs.110).aspx), EnumMemberAttribute is a data annotation which enables fine control of the names of the enumerations as they are serialized. I can take advantage of this annotation on the varaible in the enum.

# How to fix it

Annotate the EumMember attribute on L1N instead, and its value with L1-N.

```csharp
public enum Phase
{
    L1,
    L2,
    L3,
    N,
    // EnumMember Annotation
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

We initialize a *power* instance of the **ElectricPower** class.The Phase is L1L2. We already annnoted the L1L2 EnumMember with a value "L1-L2".Make sure that you have to assign a JsonConverter **StringEnumConverter**. It converts an Enum to and from *its name string value*. After we serializes the object, we will get L1-L2.

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