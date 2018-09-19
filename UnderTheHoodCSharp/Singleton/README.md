# Singleton Design Pattern

## What is Singleton Pattern?
  > Class diagram exemplifying the singleton pattern.
In software engineering, the singleton pattern is a software design pattern that restricts the instantiation of a class to one object. - Wiki

A machine can only have a status like Idle, Operate, Unavailable, Faulted. It is not allowed to instantiate the second status in the machine. Therefore a programmer should have one status existed the application entirely.

## What are the design problems?
According to [w3sDesign](http://w3sdesign.com/?gr=c05&ugr=proble), it lists the problems you must conquere when you are going to implement the singleton pattern.

  - Creating Single Objects
    * How can be ensured that a class has only one instance?
    * How can the sole instance of a class be accessed globally?
  - Controlling Instantiation
    * How can a class control its instantiation? 
    * How can the number of instances of a class be restricted? 
    * How can creating large numbers of unnecessary objects be avoided?

What I thought..
* How can be ensured that a class has only one instance?
  > a instance can only be created (new Class()) only one time 
  * If a instance is existed, return an existed one
  * If a instance is not existed, a instance can be created
* How can the sole instance of a class be accessed globally?
  > the sole instance can be accessed by exposing a public method to get a instance
* How can a class control its instantiation?
  > It can be initialized once the class is instantiated (new Class()) -> this is called lazy loading
  
 
 
