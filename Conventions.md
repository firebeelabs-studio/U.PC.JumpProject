# PufciaConventions
### Classes & Interfaces
Classes and interfaces are written in **PascalCase**. For example `SoundManager`
### Methods
Methods are written in **PascalCase**. For example `MoveTowardsPlayer()`.
### Fields
All private fields are written with **_** before name, and public are written with **PascalCase**.
```C#
public class MyClass 
{
    public int PublicField;
    private int _packagePrivate;
    private int _myPrivate;
    protected int _myProtected;
}
```
### Parameters
Parameters are written in **camelCase**.
```C#
void DoSomething(Vector3 location)
```
### Interfaces
All interfaces should be prefaced with the letter **I**.

### If statement
Always with **curly brackets**.
```C#
if (true)
{
    DoSomething();
}
```
Only exception is **Guard Clause** which should be shortened.
```C#
if (stopUpdate) return;
```
