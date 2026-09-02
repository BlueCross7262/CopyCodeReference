# Copy Code Reference

Copy selected code from the Visual Studio editor together with its file path and line numbers. Choose between the absolute path and the solution-relative path.

Useful when you paste a snippet into a code review, an issue, a chat message, or a prompt for an AI coding agent and you want the reader to know exactly where the code lives.

## Features

- Copies the file path and the line number of the current selection
- Offers two commands, one for the absolute path and one for the solution-relative path
- Available from the editor right-click menu and from the Edit menu
- Lets you pick the location format on an options page: `Foo.cs:12`, `Foo.cs(12)` or `Foo.cs#L12`
- Appends the selected text for single-line selections
- Copies the line range only for multi-line selections
- Preserves the selected text exactly, including indentation, tabs, CRLF and trailing whitespace
- Silent on success, with a short status bar message instead of a popup
- Does nothing and leaves the clipboard untouched when there is no selection

## Example

Select one line and run the command:

```text
D:\Project\SampleApp\ViewModels\MainViewModel.cs:42 var data = await repository.LoadAsync();
```

Select several lines and run the command:

```text
D:\Project\SampleApp\ViewModels\MainViewModel.cs:42-46
```

The separator between the location and the code is a single space. The line numbers are 1-based and match the numbers shown in the editor margin.

A selection that ends at the first position of the following line does not include that line. Selecting lines 1 through 3 yields `:1-3`, never `:1-4`.

## Usage

- Select code in the Visual Studio editor.
- Right-click in the editor and pick `Copy Code Reference` or `Copy Code Reference (Relative Path)`. The same two entries are also under the `Edit` menu.
- Paste the generated reference anywhere.

No keyboard shortcut is assigned by default. Assign one under `Tools` then `Options` then `Environment` then `Keyboard` by searching for `Edit.CopyCodeReference` or `Edit.CopyCodeReferenceRelative`.

## Options

Open `Tools` then `Options` then `Copy Code Reference` then `General` and pick the location format with a radio button. Both commands follow the same setting.

| Format | Single line | Several lines |
| --- | --- | --- |
| Colon (default) | `Foo.cs:12` | `Foo.cs:12-15` |
| Parentheses | `Foo.cs(12)` | `Foo.cs(12-15)` |
| GitHub | `Foo.cs#L12` | `Foo.cs#L12-L15` |

The selected text is still appended after a single space for single-line selections, whatever the format. The setting is stored in the Visual Studio settings store and travels with settings import and export.

## When the command does nothing

The command exits quietly and leaves the clipboard unchanged in these cases:

- No text is selected. The current caret line is not copied automatically.
- The active window is not a text editor, for example a designer, a resource editor or a tool window.
- The document has no file path on disk, for example an unsaved new file.
- The clipboard could not be opened because another process is holding it.

## Requirements

- Visual Studio 2022, version 17.0 or later
- Windows, x64

## Privacy

Copy Code Reference does not collect telemetry and does not transmit source code to external services. It performs no network communication, requires no account, and writes nothing outside the Windows clipboard.

## Limitations in this version

- Box and column selections are not specially handled. They do not crash, but the copied range is derived from the equivalent stream selection.
- Multi-caret selections use the first stream selection only.
- The location format is limited to the three choices on the options page. Free-form format strings are not supported.
- The relative path is resolved against the directory that holds the solution file. When no solution is open, or when the file sits outside that directory, the absolute path is used instead.

## Source code

The source code is available on GitHub at https://github.com/BlueCross7262/CopyCodeReference and is released under the MIT License.
