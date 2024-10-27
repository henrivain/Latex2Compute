using MekLatexTranslationLibrary.OperatorBuilders;

namespace MekLatexTranslationLibraryTests;
public class MatrixTests
{


    [Theory]
    [InlineData(@"\begin{matrix}1&2\\3&4\end{matrix}", @"[[1,2][3,4]]")]
    [InlineData(@"\begin{matrix}1&2&3\\4&5&6\\7&8&9\end{matrix}", @"[[1,2,3][4,5,6][7,8,9]]")]
    [InlineData(@"\begin{matrix}&&\\&&\\&&\end{matrix}", @"[[,,][,,][,,]]")]
    public void ParseBuild_ShouldReturn_ParsedMatrix(string input, string expected)
    {
        // Arrange & Act
        string matrix = Matrix.Parse(input).Build();

        // Assert
        Assert.Equal(expected, matrix);
    }

    [Theory]
    [InlineData(@"\begin{matrix}1&2&3\\4&5&6\\7\end{matrix}", @"[[1,2,3][4,5,6][7,,]]")]
    public void ParseBuild_ShouldAdd_MissingColumns(string input, string expected)
    {
        // Arrange & Act
        string matrix = Matrix.Parse(input).Build();

        // Assert
        Assert.Equal(expected, matrix);
    }
    
    [Theory]
    [InlineData(@"\begin{matrix}1\\2\\3\end{matrix}", "[1,2,3]")]
    public void ParseBuild_ShouldFlip_OneColMatrix(string input, string expected)
    {
        // Arrange & Act
        string matrix = Matrix.Parse(input).Build();

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
}
