# FluentValidator

## Motivation

When validating business logic, there is typically no single place where validation happens and error messages are often hard-coded and linked to each other, hard to read or don't provide enough info. This library aims to alleviate this by using C# expression tree's to dynamically generate the correct data.

## Usage

When using the following objects:
```cs
public class Foo {
    public long Id { get; set; }
    public string Name { get; set; }
    public long BarId { get; set; }
    public Bar Bar { get; set; }
}

public class Bar {
    public long Id { get; set; }
    public string Name { get; set; }
}
```

you would be able to provide the following validation rules
```cs
//Create a validationbuilder instance
IValidationBuilder<Foo> validator = new ValidationBuilder();
//or if you want to skip calling Start
IValidationBuilder<Foo> validator = new ValidatoinBuilder(foo);

var messages = validator.Start(foo) //<- skip if you used the second constructor
                        //Name must be non-null and non-empty value
                        .Require(o => o.Name)
                        //Name must have a length between 2 and 50 (inclusive)
                        .Length(o => o.Name, min: 2, max: 50)
                        //A custom check
                        .Check(o => o.Name != "forbidden name")
                        //At least one of the following properties must be non-null and non-zero
                        .RequireAny(o => o.ParentId, o => o.Parent)
                        //Nested property
                        .Require(o => o.Bar.Name)
                        //Changes all following validation calls to warning
                        //this will still add the messages to the end-result, but won't change
                        //the value of messages.Success
                        .AsWarning()
                        //If ID is a null or zero value, add it
                        .Require(o => o.Id)
                        //Needs to be called to expose the validation messages
                        .Build();

if (!messages.Success) {
    foreach (var message in messages)
        //handle message
} else {
    //handle success
}
//...
```

## Known issues / roadmap

* Does not support LINQ yet: `o.MyIEnumerable.All(e => e.Foo != null)` will view `o.MyIEnumerable` and `e.Foo` as 2 different properties and will not combine them to `o.MyIEnumerable.Foo`.
* The method `RequireAny` only supports up to 4 expressions and is currently very badly implemented
* The expression tree is evaluated every time the validation executes. This should be changed so you can build up metadata either at the start of the application or when the validation first runs and even better assign validation rules to a type rather than an instance of an object.
