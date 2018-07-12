# Select Many Example

The following example demonstrates how to use **SelectMany** syntax to flatten the collection. As you can see, we simplify the foreach loop to a new object which is required a one loop to iterate the Teacher/Student Name.

Teacher/Student Relationship is listing as below

* Amy
	* Jack
	* Harley
* Kurt
	* Andrea

Create a list for the Teacher/Student relationship.
```csharp
var list = new List<Teacher>
{
	new Teacher()
	{
		Name = "Amy",
		Students = new List<Student>()
		{
			new Student() {Name = "Jack"},
			new Student() {Name = "Hartley"}
		}
	},
	new Teacher()
	{
		Name = "Kurt",
		Students = new List<Student>()
		{
			new Student() {Name = "Andrea"}
		}
	}
};

```
```csharp
foreach (var teacher in list)
{
	foreach (var student in teacher.Students)
	{
		Console.WriteLine("Teacher: " + teacher.Name);
		Console.WriteLine(student.Name);
	}
}
```
SelectMany 
```csharp
var data = list.SelectMany(teacher => teacher.Students, (teacher, student) => new { Teacher = teacher, a = student});
foreach (var student in data)
{
	Console.WriteLine("Teacher: " + student.Teacher.Name);
	Console.WriteLine(student.a.Name);
}
```
## POCA 
**Student** and **Teacher** Class
```csharp
public class Student
{
	public string Name { get; set; }
}

public class Teacher
{
	public string Name { get; set; }
	public List<Student> Students { get; set; }
}
```