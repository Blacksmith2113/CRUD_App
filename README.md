# ArmyForge

A desktop app for building and managing tabletop army rosters. Create, edit and delete units with a name, point cost, category and image, all stored locally in SQLite.

## Features
- Full CRUD on units (add, edit, delete, browse)
- Units grouped and sorted by custom category priority (Characters, Battleline, Dedicated Transport, ...)
- Image upload per unit, stored as a BLOB in the database
- Auto-created database on first launch
- Material Design UI

## Tech Stack
C# · .NET 8 · WPF (XAML) · SQLite (Microsoft.Data.Sqlite) · MaterialDesignInXAML

## Architecture
- `IUnitRepository` / `UnitRepository`: data access layer behind an interface
- `Unit`: domain model
- `CategoryComparer`: custom sorting for grouped `ListCollectionView`
- Window-based UI (`StartWindow`, `MainWindow`, `AddUnitWindow`, `EditUnitWindow`)

## Getting Started
```bash
git clone https://github.com/Blacksmith2113/ArmyForge.git
cd ArmyForge
dotnet run
```
Requires Windows and the .NET 8 SDK.
