# Latex2Compute
 Translate LaTeX into calculator form

## What is this?

Have you ever written LaTeX and after completing a long mathematical formula you need to move it to your calculator? Just go ahead and rewrite it with small syntax changes. 

Actually, you don't have to do that anymore! Latex2Compute takes LaTex code as input and translates it to the format understood by the calculator. 

![Latex2Compute](https://github.com/user-attachments/assets/af4164d1-2499-4206-9669-83dfe490381b)


## A bit of history

In finnish upper secondary schools a lot of studying happens electronically. Even though some like to do their maths by hand, I liked to write the answers as you would write in Matriculation exams, electronically. I got somewhat annoyed after needing to write both latex and calculator input in every time, even though the differences weren't very big. 

[Matikkaeditorinkaantaja](https://github.com/henrivain/Matikkaeditorinkaantaja) is the product created by that annoyance. Just translate the latex into correct form by a press of a button. 

Creating this project has been a very interesting for me. I actually had zero experience in coding when I firstly started to explore this problem with python back in 2021. After some time I fell in love with programming and problem solving. Currently in 2025 I am studying computer science in Tampere University so this problem literally changed my life. Some of the code is farely old, so when you look at it just remember back then I was a noob.  :)

## More about the technical aspects

### Code

Code is written in C#. At the beginning this project was written in .NET framework 4.8 if I remember correctly. After a while I upgraded to .NET 5 and quicly to .NET 6. Today the project is targetting mainly .NET 9 but should still compile pretty good for .NET 6.

### Targetted Platforms

The project is based on latex used by Finnish Matriculation Exam board [Abitti Latex -editor](https://math-demo.abitti.fi/). Targetted calculator syntax is Texas Instruments TI-Nspire CX CAS. Currently TI-Nspire is the only main target platform, but in the future I might add support for Matlab.

### Nuget package


## Getting started

### Using the library 
1) Open your own C# project
2) Add line below inside a <ItemGroup> in your .csproj file
``` xml
<PackageReference Include="Latex2Compute" Version="3.2.1" />
```
or use dotnet cli
```ps
dotnet add package Latex2Compute
```

## Licence
This repository is licenced under GNU General Public License v3.0. See LICENCE -file.

## Support

If you have any questions about anything related to this project, open a issue on Github or send me an email.

Henri Vainio
matikkaeditorinkaantaja(at)gmail.com
