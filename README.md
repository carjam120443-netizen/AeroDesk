# AeroDesk 🪟✨

A lightweight Windows desktop widget and launcher system inspired by the glassy Aero era of Windows Vista and Windows 7.

## 🚧 Current status

The initial **WPF project shell** is now in the repository.

The first build includes:

- A Windows .NET 8 WPF application
- Borderless Aero-inspired glass UI
- A working clock/date widget
- Draggable custom title bar
- Portable-friendly project structure

## Planned features

- 🕒 Desktop clock and date widget
- 📊 CPU, RAM, disk, and network widgets
- 📝 Sticky notes
- 🚀 Application launcher
- 📌 Pinned applications and shortcuts
- 🧩 Extensible widget system
- 🎨 Glass, blur, transparency, and Aero animations
- ⚙️ Widget positions and settings
- 📦 Portable operation where practical

## Project structure

```text
AeroDesk/
├── AeroDesk.sln
├── src/
│   └── AeroDesk/
│       ├── AeroDesk.csproj
│       ├── App.xaml
│       ├── App.xaml.cs
│       ├── MainWindow.xaml
│       └── MainWindow.xaml.cs
├── widgets/
├── assets/
├── docs/
└── README.md
```

## Requirements

- Windows
- .NET 8 SDK
- A Windows-compatible IDE or `dotnet` CLI

## Build

```powershell
dotnet build AeroDesk.sln
```

## Run

```powershell
dotnet run --project src/AeroDesk/AeroDesk.csproj
```

## Roadmap

1. ✅ Create the base WPF project
2. ✅ Add the first Aero-style shell
3. 🔲 Build a reusable widget host
4. 🔲 Turn the clock into a standalone desktop widget
5. 🔲 Add the application launcher
6. 🔲 Add widget configuration and persistence
7. 🔲 Add Windows blur/backdrop effects
8. 🔲 Package portable releases

## License

License to be decided during early development.
