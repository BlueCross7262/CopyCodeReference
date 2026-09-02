# Changelog

## 0.1.3

### Added

- Options page under `Tools` > `Options` > `Copy Code Reference` > `General` with radio buttons that select the location format: `Foo.cs:12` (colon, default), `Foo.cs(12)` (parentheses) or `Foo.cs#L12` (GitHub). Multi-line selections follow the same choice: `Foo.cs:12-15`, `Foo.cs(12-15)` or `Foo.cs#L12-L15`.
- The setting is stored in the Visual Studio settings store and is included in settings import and export.

### Changed

- Both commands read the selected format at run time. The default output is unchanged from earlier versions.

## 0.1.2

### Fixed

- The context menu entries now appear in every text editor that shares the standard Cut, Copy and Paste group, including the XAML text editor. Version 0.1.1 anchored them to a private group under the code window menu, which only the C# editor showed.

## 0.1.1

### Added

- Editor right-click context menu entries for both commands
- `Copy Code Reference (Relative Path)` command that emits a solution-relative path

### Changed

- The original command keeps emitting an absolute path and is unchanged

## 0.1.0

### Added

- Copy the absolute file path and line number of the current selection
- Append the selected text after a single space for single-line selections
- Copy the line range only for multi-line selections
- Copy the formatted reference to the clipboard
