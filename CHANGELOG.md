# Changelog

## 0.1.4

### Added

- Korean user interface. The options page and the status bar message follow the Visual Studio display language: Korean for Korean, English for every other language.
- Live preview on the options page that shows what a single-line selection, a multi-line selection and an empty selection copy with the current settings.

### Changed

- Redesigned options page: format samples in a monospace column, a short note under each setting, and a two-column layout that stacks when the page is narrow.

## 0.1.3

### Added

- Options page under `Tools` > `Options` > `Copy Code Reference` > `General` with radio buttons that select the location format: `Foo.cs:12` (colon, default), `Foo.cs(12)` (parentheses) or `Foo.cs#L12` (GitHub). Multi-line selections follow the same choice: `Foo.cs:12-15`, `Foo.cs(12-15)` or `Foo.cs#L12-L15`.
- Path separator option that writes paths with `/` instead of `\`, which suits GitHub and Markdown. The selected text is never rewritten.
- Multi-line selection option that copies the selected code below the location line, either as plain text or inside a Markdown fence. The fence language comes from the file extension, and the fence grows longer than any backtick run inside the code so that fenced content survives.
- Option to append the caret line text when nothing is selected. It is off by default.
- The settings are stored in the Visual Studio settings store and are included in settings import and export.

### Changed

- When nothing is selected, both commands now copy the location of the caret line, such as `Foo.cs:12`. Earlier versions did nothing in this case. The location follows the location format and path separator settings. A selection that covers virtual space only is handled the same way.
- Both commands read the settings at run time. With a selection, every default matches the earlier behaviour, so the output is unchanged until a setting is changed.

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
