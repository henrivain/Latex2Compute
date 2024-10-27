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
internal record struct Brackets(char Opening, char Closing)
{
    internal static readonly Brackets _curly = new('{', '}');
    internal static readonly Brackets _square = new('[', ']');
    internal static readonly Brackets _round = new('(', ')');
    
    /// <summary>
    /// GetBottom all Latex bracket types
    /// </summary>
    /// <returns>{ Curly, Square, Round }</returns>
    internal static Brackets[] All { get; } = { _curly, _square, _round };
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
        Content = content ?? string.Empty;
    }
    internal int EndIndex { get; set; }
    internal string Content { get; set; }
}

/// <summary>
/// First and Second part of string that is cut to two pieces (can be used instead of tuple)
/// </summary>
/// <returns>
/// string First, string Second
/// </returns>
internal record TwoStrings(string First = "", string Second = "");


