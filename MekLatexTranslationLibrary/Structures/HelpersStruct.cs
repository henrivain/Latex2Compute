/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekLatexTranslationLibrary.Structures;

/// <summary>
/// Defines different bracket types
/// </summary>
/// <returns>"{}", "[]", "()" or all of them as list</returns>
internal struct Bracket 
{
    internal const string Curly = "{}";
    internal const string Square = "[]";
    internal const string Round = "()";
    
    /// <summary>
    /// GetBottom all Latex bracket types
    /// </summary>
    /// <returns>{ Curly, Square, Round }</returns>
    internal static readonly string[] All = { "{}", "[]", "()" };
}

/// <summary>
/// Info about where string element content ends and what it is
/// </summary>
/// <returns>int endIndex, string content</returns>
internal struct ContentAndEnd
{
    /// <summary>
    /// Info about endIndex point and content in string element
    /// </summary>
    /// <param name="endIndex"></param>
    /// <param name="content"></param>
    internal ContentAndEnd(int endIndex, string content)
    {
        EndIndex = endIndex;
        Content = content;
    }

    internal int EndIndex { get; private set; }
    internal string Content { get; private set; }
}

/// <summary>
/// First and Second part of string that is cut to two pieces (can be used instead of tuple)
/// </summary>
/// <returns>
/// string First, string Second
/// </returns>
internal struct TwoStrings
{
    /// <summary>
    /// First and Second part of string that is cut to two pieces (can be used instead of tuple)
    /// </summary>
    /// <returns>
    /// string First, string Second
    /// </returns>
    /// <param name="first"></param>
    /// <param name="last"></param>
    internal TwoStrings(string first, string last)
    {
        First = first;
        Second = last;
    }
    internal string First { get; init; } = string.Empty;
    internal string Second { get; init; } = string.Empty;
}


