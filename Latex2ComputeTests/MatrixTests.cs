using MekLatexTranslationLibrary.Parsers;

namespace MekLatexTranslationLibraryTests;
public class MatrixTests
{


    [Theory]
    [InlineData(@"\begin{matrix}1&2\\3&4\end{matrix}", @"[[1,2][3,4]]")]
    [InlineData(@"\begin{matrix}1&2&3\\4&5&6\\7&8&9\end{matrix}", @"[[1,2,3][4,5,6][7,8,9]]")]
    [InlineData(@"\begin{matrix}&&\\&&\\&&\end{matrix}", @"[[,,][,,][,,]]")]
    public void ParseBuild_ShouldReturn_ParsedMatrix(string input, string expected)
    {
        // Arrange
        TranslationArgs args = new();

        // Act
        string matrix = Matrix.Parse(input).Build(args);

        // Assert
        Assert.Equal(expected, matrix);
    }

    [Theory]
    [InlineData(@"\begin{matrix}1&2&3\\4&5&6\\7\end{matrix}", @"[[1,2,3][4,5,6][7,,]]")]
    public void ParseBuild_ShouldAdd_MissingColumns(string input, string expected)
    {
        // Arrange
        TranslationArgs args = new();

        // Act
        string matrix = Matrix.Parse(input).Build(args);

        // Assert
        Assert.Equal(expected, matrix);
    }
    
    [Theory]
    [InlineData(@"\begin{matrix}1\\2\\3\end{matrix}", "[1,2,3]")]
    public void ParseBuild_ShouldTranspose_VerticalVector(string input, string expected)
    {
        // Arrange
        TranslationArgs args = new();

        // Act
        string matrix = Matrix.Parse(input).Build(args);

        // Assert
        Assert.Equal(expected, matrix);
    }

    [Theory]
    [InlineData(@"\begin{matrix}1\\2\\3\end{matrix}", "[1,2,3]")]
    public void LatexTranslation_ShouldReturn_ParsedMatrix(string input, string expected)
    {
        // Arrange
        TranslationItem item = new(input, new TranslationArgs());

        // Act
        TranslationResult result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expected, result.Result);
    }



    [Theory]
    [InlineData(@"\begin{matrix}1&2\\3&4\end{matrix}", "[1,2;\n  3,4]")]
    [InlineData(@"\begin{matrix}1&2&3\\4&5&6\\7&8&9\end{matrix}", "[1,2,3;\n  4,5,6;\n  7,8,9]")]
    [InlineData(@"\begin{matrix}&&\\&&\\&&\end{matrix}", "[,,;\n  ,,;\n  ,,]")]
    public void ParseBuild_Matlab_ShouldReturn_ParsedMatrix(string input, string expected)
    {
        // Arrange
        TranslationArgs args = new() { TargetSystem = TargetSystem.Matlab };

        // Act
        string matrix = Matrix.Parse(input).Build(args);

        // Assert
        Assert.Equal(expected, matrix);
    }

    [Theory]
    [InlineData(@"")]
    [InlineData(@"3\cdot3")]
    public void ParseBuild_ShouldDoNothing_IfNoMatrix(string input)
    {
        // Arrange
        TranslationArgs args = new() { TargetSystem = TargetSystem.Matlab };

        // Act
        string matrix = Matrix.Parse(input).Build(args);

        // Assert
        Assert.Equal(input, matrix);
    }
}
