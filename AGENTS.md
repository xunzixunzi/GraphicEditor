# AGENTS.md

This document contains essential information for agentic coding assistants working on this WPF Graphic Editor project.

## Build Commands

```bash
# Build entire solution
dotnet build GraphicEditor.sln

# Build specific project
dotnet build GraphicEditor/GraphicEditor.csproj
dotnet build GraphicsViewFramework/GraphicsViewFramework.csproj

# Clean build
dotnet clean GraphicEditor.sln

# Run application (after build)
dotnet run --project GraphicEditor/GraphicEditor.csproj

# Restore packages
dotnet restore GraphicEditor.sln

# Build in release mode
dotnet build GraphicEditor.sln --configuration Release
```

## Testing

No test projects are currently configured in this solution. When adding tests:
- Create xUnit/NUnit/MSTest projects following .NET conventions
- Name test projects `*.Tests.csproj`
- Run tests: `dotnet test`
- Run single test: `dotnet test --filter "FullyQualifiedName~TestMethodName"`

## Project Structure

- **GraphicEditor** - Main WPF application (WinExe)
- **GraphicsViewFramework** - Class library for graphics components
- Target framework: .NET 8.0-windows
- UI framework: WPF (Windows Presentation Foundation)

## Code Style Guidelines

### General
- Use standard C# conventions
- 4-space indentation (no tabs)
- UTF-8 encoding
- No trailing whitespace

### Imports and Namespaces
- `ImplicitUsings` is enabled - avoid explicit global usings when possible
- Group imports: System namespaces first, then third-party, then local
- Use namespace declarations matching folder structure
- Prefer `using` directives over fully qualified names for readability

### Types and Nullability
- `Nullable` is enabled - always specify nullability (string? vs string)
- Use nullable reference types consistently
- Initialize reference types to non-null values or declare as nullable
- Use null-conditional operators (`?.`) and null-coalescing (`??`) appropriately

### Naming Conventions
- **Classes**: PascalCase (e.g., `MainWindow`, `ShapeRenderer`)
- **Interfaces**: PascalCase with `I` prefix (e.g., `IGraphicShape`)
- **Methods**: PascalCase (e.g., `InitializeComponent`, `DrawShape`)
- **Properties**: PascalCase (e.g., `Title`, `Width`)
- **Fields**: _camelCase with underscore prefix for private fields (e.g., `_canvas`, `_shapes`)
- **Local variables**: camelCase (e.g., `width`, `shape`)
- **Constants**: PascalCase (e.g., `MaxWidth`, `DefaultSize`)
- **Events**: PascalCase (e.g., `ShapeSelected`, `CanvasChanged`)
- **Async methods**: Append `Async` suffix (e.g., `LoadImageAsync`)

### XAML Guidelines
- Use `x:Class` attribute matching C# namespace and class name
- Define `xmlns:local` for local namespace references
- Keep XAML clean and well-structured
- Use meaningful element names for controls accessed from code-behind
- Prefer data binding over code-behind where appropriate

### Error Handling
- Use `try-catch` blocks for recoverable exceptions
- Log errors appropriately (add logging framework if needed)
- Validate arguments at method entry using ArgumentNullException.ThrowIfNull
- Avoid catching `Exception` without specific exception types
- Use `throw` (without argument) to re-throw caught exceptions

### WPF-Specific Guidelines
- Use `partial class` for XAML code-behind
- Keep code-behind minimal - move logic to view models (MVVM pattern preferred)
- Use `INotifyPropertyChanged` for data binding
- Prefer dependency properties for attached behavior
- Use commands (`ICommand`) instead of event handlers for user actions

### File Organization
- One class per file for main types
- Match filename to class name (e.g., `MainWindow.xaml.cs` for `MainWindow`)
- Place XAML files alongside code-behind files
- Use nested folders for related feature groups

### Comments and Documentation
- Use XML documentation comments (`///`) for public APIs
- Keep comments brief and accurate - code should be self-documenting
- Explain WHY, not WHAT - complex logic needs rationale
- TODO comments should include context and assignee if possible

### Dependencies
- This is a fresh project with minimal dependencies
- Prefer .NET built-in libraries over third-party packages
- When adding NuGet packages, update both projects if shared functionality needed
- Keep dependencies minimal and well-maintained

## Performance Guidelines

- Dispose IDisposable objects properly (use `using` statements or `Dispose()`)
- Avoid UI thread blocking - use async/await for I/O operations
- Virtualize large collections in DataGrid/ListBox controls
- Freeze freezable objects (Brushes, Geometries) when possible
- Avoid excessive layout recalculations - batch UI updates when possible
- Use `Dispatcher.Invoke` sparingly - prefer async patterns for cross-thread operations

## Git Workflow

- Feature branches: `feature/description`, `bugfix/description`, `hotfix/description`
- Commit messages: conventional format (e.g., `feat: add shape rotation`)
- Keep commits atomic and focused on single logical changes
- Pull request required for merging to main branch
- Ensure solution builds before pushing commits

## Development Notes

- Project uses SDK-style .csproj format
- Solution file: `GraphicEditor.sln`
- Currently in early development phase with minimal code
- No linting/formatting tools configured - follow VS default formatting
- Target platform: Windows 7.0 minimum
