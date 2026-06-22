# SpellRecast

A .NET 8 wrapper around [RecastSharp](https://github.com/nt153133/RecastSharp), which wraps the
native [Recast/Detour](https://github.com/recastnavigation/recastnavigation) navigation library.
It loads `.nav` navigation meshes (the RecastDemo `MSET` tile format) and runs pathfinding queries
on them.

## Layout

- `SpellRecast/` — high-level managed API (`NavMesh`, `NavMeshQuery`, `.nav` file reader)
- `SpellRecastTester/` — console demo that loads `d2t1.nav` and prints a smooth path
- `RecastSharp/` — git submodule: the P/Invoke bindings + the native C++ wrapper and its
  prebuilt `RecastWrapper64.dll`

All projects target **net8.0** (x64 — the native library is 64-bit).

## Build & run (managed)

The native `RecastWrapper64.dll` is committed in the submodule, so no C++ toolchain is required to
build or run the managed code:

```sh
git clone --recursive <repo>          # or: git submodule update --init --recursive
dotnet build SpellRecast.sln -c Release
dotnet run --project SpellRecastTester -c Release
```

The native DLL is located at runtime by `NativeLibraryResolver` (a `[ModuleInitializer]` that
registers a `NativeLibrary.SetDllImportResolver`), so it works regardless of which output
subfolder MSBuild copies it into.

## Rebuilding the native DLL (optional)

Only needed if you change the C++ wrapper (`RecastSharp/RecastCWrapper`). Requires Visual Studio
(or Build Tools) with the C++ workload.

```sh
premake.bat                           # generates RecastSharp\RecastCWrapper\Build\vs2019\recastwrapper.sln
```

Then build `recastwrapper.sln` for **Release | Win64** (e.g. open it in Visual Studio, or run
MSBuild with `/t:RecastWrapper /p:Configuration=Release /p:Platform=Win64`). The build outputs
`RecastWrapper64.dll` into `RecastSharp/RecastSharp/RecastSharp/Costura64/`, which is the canonical
binary shipped to consumers.
