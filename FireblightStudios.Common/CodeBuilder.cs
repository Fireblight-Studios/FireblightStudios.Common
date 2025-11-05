using System.Runtime.CompilerServices;
using System.Text;

namespace FireblightStudios.Common;

/// <summary>
/// Represents a builder class for generating code with proper indentation and line handling.
/// </summary>
/// <remarks>
/// This class allows for the construction of multi-line strings, typically used in generating source code,
/// by managing indentation levels, indentation characters, and line breaks.
/// </remarks>
public class CodeBuilder
{
    /// <summary>
    /// Represents the internal StringBuilder used to construct code.
    /// </summary>
    private StringBuilder _builder;

    /// <summary>
    /// Gets or sets the number of indentation levels.
    /// A positive integer value that represents how many times the IndentationChars should be repeated for each level.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the assigned value is less than 0.</exception>
    public int IndentationLevel
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            field = value;
        }
    } = 0;

    /// Gets or sets the number of characters used for each level of indentation.
    /// The value must be non-negative. Default is 4.
    public int IndentationChars
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            field = value;
        }
    } = 4;

    /// Gets or sets the character used for indentation.
    /// The default value is a space (' ').
    /// Changing this property allows customization of the indentation style,
    /// such as switching from spaces to tabs.
    public char IndentationChar { get; set; } = ' ';

    /// <summary>
    /// Gets a value indicating whether the last character in the internal StringBuilder is a newline.
    /// </summary>
    /// <value>
    /// true if the last character is a newline ('\n'); otherwise, false.
    /// If the StringBuilder is empty, it returns true as there is no content to check against.
    /// </value>
    public bool IsNewLine
    {
        get
        {
            if (_builder.Length == 0)
                return true;
            return _builder[_builder.Length-1] == '\n';
        }
    }

    /// <summary>
    /// Represents a utility class used to build code strings with indentation and line management.
    /// </summary>
    /// <remarks>
    /// This class provides methods for appending lines of text, managing indentation levels,
    /// entering and leaving code scopes, and ensuring proper formatting of the generated code.
    /// </remarks>
    public CodeBuilder()
    {
        _builder = new StringBuilder();
    }

    /// <summary>
    /// Represents a class that builds C# code with indentation and line management.
    /// </summary>
    /// <remarks>
    /// This class is useful for generating formatted code snippets programmatically. It allows appending lines of text
    /// while maintaining proper indentation according to the current indentation level and character settings.
    /// </remarks>
    public CodeBuilder(string startingText)
    {
        _builder = new StringBuilder(startingText);
    }

    /// <summary>
    /// Increases the indentation level by one.
    /// </summary>
    /// <returns>The current CodeBuilder instance to support method chaining.</returns>
    public CodeBuilder IncreaseIndentation()
    {
        IndentationLevel++;
        return this;
    }

    /// Decreases the indentation level of the CodeBuilder by one.
    /// If the current indentation level is greater than zero, it will be decremented,
    /// otherwise, it remains unchanged.
    /// <returns>The instance of the CodeBuilder itself to allow for method chaining.</returns>
    public CodeBuilder DecreaseIndentation()
    {
        if (IndentationLevel > 0)
            IndentationLevel--;

        return this;
    }

    /// <summary>
    /// Appends a new line to the internal StringBuilder.
    /// </summary>
    /// <returns>The current instance of CodeBuilder, allowing for method chaining.</returns>
    public CodeBuilder AppendLine()
    {
        _builder.AppendLine();
        return this;
    }

    /// <summary>
    /// Appends a line terminator to the current <see cref="StringBuilder"/> and returns the instance.
    /// </summary>
    /// <returns>The current instance of <see cref="CodeBuilder"/>.</returns>
    public CodeBuilder AppendLine(string text)
    {
        string[] lines = text.Replace("\r","").Split('\n');
        foreach (string line in lines)
        {
            if (IsNewLine)
                AppendIndentation();
            _builder.AppendLine(line);
        }
        
        return this;
    }

    /// <summary>
    /// Appends the specified text to the current code block without adding a new line.
    /// </summary>
    /// <param name="text">The text to append.</param>
    /// <returns>The current CodeBuilder instance, enabling method chaining.</returns>
    public CodeBuilder Append(string text)
    {
        string[] lines = text.Replace("\r","").Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (IsNewLine)
                AppendIndentation();
            if (i == lines.Length - 1)
            {
                _builder.Append(line);
            }
            else
                _builder.AppendLine(line);
        }

        return this;
    }

    /// <summary>
    /// Appends the specified text followed by an opening curly brace and a new line to the builder, then increases the indentation level.
    /// </summary>
    /// <param name="text">The text to append before the scope begins.</param>
    /// <returns>The current instance of CodeBuilder to allow for chaining method calls.</returns>
    public CodeBuilder EnterScope(string text)
    {
        return AppendLine(text).AppendLine("{").IncreaseIndentation();
    }

    /// <summary>
    /// Appends a closing brace to the code and decreases the indentation level.
    /// </summary>
    /// <returns>The current instance of CodeBuilder.</returns>
    public CodeBuilder LeaveScope()
    {
        return DecreaseIndentation().AppendLine("}");
    }

    /// <summary>
    /// Appends indentation to the current builder based on the <see cref="IndentationLevel"/> and <see cref="IndentationChars"/>.
    /// The method uses the <see cref="IndentationChar"/> for each character of indentation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendIndentation()
    {
        for(int i =0; i < IndentationLevel; i++)
            for(int j = 0; j < IndentationChars; j++)
                _builder.Append(IndentationChar);
    }

    /// <summary>
    /// Returns the string representation of the accumulated code content in the builder.
    /// </summary>
    /// <returns>
    /// A string containing the concatenated text managed by the internal StringBuilder, including any applied indentation and formatting.
    /// </returns>
    public override string ToString()
    {
        return _builder.ToString();
    }

    /// <summary>
    /// Clears all the contents of the internal string builder used for constructing the code, resetting it to an empty state.
    /// </summary>
    /// <returns>
    /// Returns the current instance of the <see cref="CodeBuilder"/> to allow method chaining.
    /// </returns>
    public CodeBuilder Clear()
    {
        _builder.Clear();
        return this;
    }
    
    
}