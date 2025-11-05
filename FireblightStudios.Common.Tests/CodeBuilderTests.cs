namespace FireblightStudios.Common.Tests;

public class CodeBuilderTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithoutParameters_CreatesEmptyBuilder()
    {
        // Arrange & Act
        var builder = new CodeBuilder();

        // Assert
        Assert.Equal(string.Empty, builder.ToString());
        Assert.Equal(0, builder.IndentationLevel);
        Assert.Equal(4, builder.IndentationChars);
        Assert.Equal(' ', builder.IndentationChar);
        Assert.True(builder.IsNewLine);
    }

    [Fact]
    public void Constructor_WithStartingText_InitializesWithText()
    {
        // Arrange
        const string startingText = "public class Test";

        // Act
        var builder = new CodeBuilder(startingText);

        // Assert
        Assert.Equal(startingText, builder.ToString());
        Assert.False(builder.IsNewLine);
    }

    [Fact]
    public void Constructor_WithStartingTextEndingInNewline_IsNewLineReturnsTrue()
    {
        // Arrange
        const string startingText = "public class Test\n";

        // Act
        var builder = new CodeBuilder(startingText);

        // Assert
        Assert.True(builder.IsNewLine);
    }

    #endregion

    #region Property Safeguard Tests

    [Fact]
    public void IndentationLevel_SetToNegative_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.IndentationLevel = -1);
    }

    [Fact]
    public void IndentationLevel_SetToZero_DoesNotThrow()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.IndentationLevel = 0;

        // Assert
        Assert.Equal(0, builder.IndentationLevel);
    }

    [Fact]
    public void IndentationLevel_SetToPositive_SetsValue()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.IndentationLevel = 5;

        // Assert
        Assert.Equal(5, builder.IndentationLevel);
    }

    [Fact]
    public void IndentationChars_SetToNegative_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.IndentationChars = -1);
    }

    [Fact]
    public void IndentationChars_SetToZero_DoesNotThrow()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.IndentationChars = 0;

        // Assert
        Assert.Equal(0, builder.IndentationChars);
    }

    [Fact]
    public void IndentationChars_SetToPositive_SetsValue()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.IndentationChars = 2;

        // Assert
        Assert.Equal(2, builder.IndentationChars);
    }

    [Fact]
    public void IndentationChar_SetToTab_SetsValue()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.IndentationChar = '\t';

        // Assert
        Assert.Equal('\t', builder.IndentationChar);
    }

    #endregion

    #region IsNewLine Property Tests

    [Fact]
    public void IsNewLine_EmptyBuilder_ReturnsTrue()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act & Assert
        Assert.True(builder.IsNewLine);
    }

    [Fact]
    public void IsNewLine_AfterAppendingText_ReturnsFalse()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.Append("some text");

        // Assert
        Assert.False(builder.IsNewLine);
    }

    [Fact]
    public void IsNewLine_AfterAppendLine_ReturnsTrue()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.AppendLine();

        // Assert
        Assert.True(builder.IsNewLine);
    }

    [Fact]
    public void IsNewLine_AfterAppendLineWithText_ReturnsTrue()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.AppendLine("some text");

        // Assert
        Assert.True(builder.IsNewLine);
    }

    #endregion

    #region Indentation Helper Tests

    [Fact]
    public void IncreaseIndentation_IncrementsLevel()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.IncreaseIndentation();

        // Assert
        Assert.Equal(1, builder.IndentationLevel);
    }

    [Fact]
    public void IncreaseIndentation_ReturnsSelf_ForChaining()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        var result = builder.IncreaseIndentation();

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void IncreaseIndentation_Multiple_IncrementsCorrectly()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.IncreaseIndentation().IncreaseIndentation().IncreaseIndentation();

        // Assert
        Assert.Equal(3, builder.IndentationLevel);
    }

    [Fact]
    public void DecreaseIndentation_DecrementsLevel()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 2 };

        // Act
        builder.DecreaseIndentation();

        // Assert
        Assert.Equal(1, builder.IndentationLevel);
    }

    [Fact]
    public void DecreaseIndentation_AtZero_RemainsAtZero()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 0 };

        // Act
        builder.DecreaseIndentation();

        // Assert
        Assert.Equal(0, builder.IndentationLevel);
    }

    [Fact]
    public void DecreaseIndentation_ReturnsSelf_ForChaining()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };

        // Act
        var result = builder.DecreaseIndentation();

        // Assert
        Assert.Same(builder, result);
    }

    #endregion

    #region AppendLine Tests

    [Fact]
    public void AppendLine_NoParameters_AddsNewLine()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.AppendLine();

        // Assert
        Assert.Equal(Environment.NewLine, builder.ToString());
    }

    [Fact]
    public void AppendLine_NoParameters_ReturnsSelf()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        var result = builder.AppendLine();

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void AppendLine_SingleLineText_AddsTextWithNewLine()
    {
        // Arrange
        var builder = new CodeBuilder();
        const string text = "public class Test";

        // Act
        builder.AppendLine(text);

        // Assert
        Assert.Equal($"{text}{Environment.NewLine}", builder.ToString());
    }

    [Fact]
    public void AppendLine_SingleLineText_WithIndentation_AddsIndentedText()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };
        const string text = "public class Test";

        // Act
        builder.AppendLine(text);

        // Assert
        Assert.Equal($"    {text}{Environment.NewLine}", builder.ToString());
    }

    [Fact]
    public void AppendLine_MultilineText_AddsEachLineIndented()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };
        const string text = "line1\nline2\nline3";

        // Act
        builder.AppendLine(text);

        // Assert
        var expected = $"    line1{Environment.NewLine}    line2{Environment.NewLine}    line3{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void AppendLine_MultilineTextWithCarriageReturn_HandlesCorrectly()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };
        const string text = "line1\r\nline2\r\nline3";

        // Act
        builder.AppendLine(text);

        // Assert
        var expected = $"    line1{Environment.NewLine}    line2{Environment.NewLine}    line3{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void AppendLine_EmptyString_AddsOnlyNewLine()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.AppendLine(string.Empty);

        // Assert
        Assert.Equal(Environment.NewLine, builder.ToString());
    }

    [Fact]
    public void AppendLine_MultipleCallsWithIndentation_MaintainsIndentation()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };

        // Act
        builder.AppendLine("line1");
        builder.AppendLine("line2");

        // Assert
        var expected = $"    line1{Environment.NewLine}    line2{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    #endregion

    #region Append Tests

    [Fact]
    public void Append_SingleLineText_AddsTextWithoutNewLine()
    {
        // Arrange
        var builder = new CodeBuilder();
        const string text = "public class";

        // Act
        builder.Append(text);

        // Assert
        Assert.Equal($"{text}", builder.ToString());
    }

    [Fact]
    public void Append_SingleLineText_WithIndentation_AddsIndentedText()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 2 };
        const string text = "public class";

        // Act
        builder.AppendLine(text);

        // Assert
        Assert.Equal($"        {text}{Environment.NewLine}", builder.ToString());
    }
    

    [Fact]
    public void Append_MultilineTextWithCarriageReturn_HandlesCorrectly()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };
        const string text = "line1\r\nline2\r\nline3";

        // Act
        builder.Append(text);

        // Assert
        var expected = $"    line1{Environment.NewLine}    line2{Environment.NewLine}    line3";
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void Append_ReturnsSelf_ForChaining()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        var result = builder.Append("text");

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void Append_EmptyString_DoesNothing()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.Append(string.Empty);

        // Assert
        Assert.Equal(string.Empty, builder.ToString());
    }

    [Fact]
    public void Append_MultipleCallsOnSameLine_DoesNotAddExtraIndentation()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };

        // Act
        builder.AppendLine("start");
        builder.Append("middle");

        // Assert
        var expected = $"    start{Environment.NewLine}    middle";
        Assert.Equal(expected, builder.ToString());
    }

    #endregion

    #region Scope Management Tests

    [Fact]
    public void EnterScope_AddsTextAndOpeningBrace_IncreasesIndentation()
    {
        // Arrange
        var builder = new CodeBuilder();
        const string text = "public class Test";

        // Act
        builder.EnterScope(text);

        // Assert
        Assert.Equal(1, builder.IndentationLevel);
        var expected = $"{text}{Environment.NewLine}{{{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void EnterScope_ReturnsSelf_ForChaining()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        var result = builder.EnterScope("test");

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void LeaveScope_AddsClosingBrace_DecreasesIndentation()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };

        // Act
        builder.LeaveScope();

        // Assert
        Assert.Equal(0, builder.IndentationLevel);
        var expected = $"}}{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void LeaveScope_ReturnsSelf_ForChaining()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };

        // Act
        var result = builder.LeaveScope();

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void EnterScope_WithExistingIndentation_IncreasesCorrectly()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 2 };

        // Act
        builder.EnterScope("if (true)");

        // Assert
        Assert.Equal(3, builder.IndentationLevel);
    }

    #endregion

    #region Indentation Behavior Tests

    [Fact]
    public void Indentation_WithTwoSpaces_UsesCorrectIndentation()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1, IndentationChars = 2 };

        // Act
        builder.AppendLine("test");

        // Assert
        Assert.Equal($"  test{Environment.NewLine}", builder.ToString());
    }

    [Fact]
    public void Indentation_WithTabCharacter_UsesTabForIndentation()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1, IndentationChar = '\t', IndentationChars = 1 };

        // Act
        builder.AppendLine("test");

        // Assert
        Assert.Equal($"\ttest{Environment.NewLine}", builder.ToString());
    }

    [Fact]
    public void Indentation_WithZeroChars_NoIndentation()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 5, IndentationChars = 0 };

        // Act
        builder.AppendLine("test");

        // Assert
        Assert.Equal($"test{Environment.NewLine}", builder.ToString());
    }

    [Fact]
    public void Indentation_WithMultipleLevels_AppliesCorrectly()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 3, IndentationChars = 2 };

        // Act
        builder.AppendLine("test");

        // Assert
        Assert.Equal($"      test{Environment.NewLine}", builder.ToString());
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void IntegrationTest_BuildSimpleClass_ProducesCorrectOutput()
    {
        // Arrange
        var builder = new CodeBuilder();

        // Act
        builder.AppendLine("namespace TestNamespace;");
        builder.AppendLine();
        builder.EnterScope("public class TestClass");
        builder.AppendLine("private int _value;");
        builder.AppendLine();
        builder.EnterScope("public void TestMethod()");
        builder.AppendLine("_value = 42;");
        builder.LeaveScope();
        builder.LeaveScope();

        // Assert
        var expected = $"namespace TestNamespace;{Environment.NewLine}" +
                      $"{Environment.NewLine}" +
                      $"public class TestClass{Environment.NewLine}" +
                      $"{{{Environment.NewLine}" +
                      $"    private int _value;{Environment.NewLine}" +
                      $"{Environment.NewLine}" +
                      $"    public void TestMethod(){Environment.NewLine}" +
                      $"    {{{Environment.NewLine}" +
                      $"        _value = 42;{Environment.NewLine}" +
                      $"    }}{Environment.NewLine}" +
                      $"}}{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void IntegrationTest_MethodChaining_WorksCorrectly()
    {
        // Arrange & Act
        var result = new CodeBuilder()
            .AppendLine("class Test")
            .AppendLine("{")
            .IncreaseIndentation()
            .AppendLine("void Method()")
            .AppendLine("{")
            .IncreaseIndentation()
            .AppendLine("// code")
            .DecreaseIndentation()
            .AppendLine("}")
            .DecreaseIndentation()
            .AppendLine("}")
            .ToString();

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("    void Method()", result);
        Assert.Contains("        // code", result);
    }

    [Fact]
    public void IntegrationTest_MultilineStringWithVaryingIndentation_HandlesCorrectly()
    {
        // Arrange
        var builder = new CodeBuilder();
        builder.AppendLine("start");
        builder.IncreaseIndentation();

        // Act
        builder.AppendLine("first\nsecond\nthird");
        builder.DecreaseIndentation();
        builder.AppendLine("end");

        // Assert
        var expected = $"start{Environment.NewLine}" +
                      $"    first{Environment.NewLine}" +
                      $"    second{Environment.NewLine}" +
                      $"    third{Environment.NewLine}" +
                      $"end{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void IntegrationTest_AppendVsAppendLine_BehaveDifferently()
    {
        // Arrange
        var builder = new CodeBuilder { IndentationLevel = 1 };

        // Act
        builder.Append("line1\nline2");
        builder.AppendLine("line3");

        // Assert - Append always ends with newline
        var expected = $"    line1{Environment.NewLine}    line2line3{Environment.NewLine}";
        Assert.Equal(expected, builder.ToString());
    }

    #endregion
}