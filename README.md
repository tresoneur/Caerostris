# Cærostris

## Demo

The latest version of _Cærostris_ can be accessed [__here__](https://caerostris.azurewebsites.net/). _Cærostris_ should be viewed in desktop browsers and on screens at least 1600 pixels wide.

## Status of this project

This project is currently under development.

## The goal of this project

To create a proof-of-concept Blazor WebAssembly PWA Spotify client with .NET languages and tooling.

## How to build

* Get the latest .NET SDK (and the latest Visual Studio 2019 Preview)

* Get [_SpotifyService_](https://github.com/tresoneur/SpotifyService). You can either place _SpotifyService_ where `Caerostris.sln` and `Caerostris.csproj` expect it to be or you can edit the path in the following locations:

    ```
    Project("{...}") = "SpotifyService", "..\SpotifyService\SpotifyService.csproj", "{...}"
    EndProject
    ```

    ```xml
    <ItemGroup>
        <ProjectReference Include="..\SpotifyService\SpotifyService.csproj" />
    </ItemGroup>
    ```

* Get a DevExpress Blazor (free (as in beer)) license and use VS or `dotnet nuget add source` to supply the resulting nuget source.

* Run `dotnet build`.

## Design considerations

* _Cærostris_ uses SCSS. Each razor folder contains a `Styles` folder with at least one `.scss` file, which are included in `/Styles/Site.scss` along with other `.scss` files in the `/Styles` folder that do not belong to any one particular component.

* _Cærostris_ is a [Progressive Web App](https://devblogs.microsoft.com/aspnet/blazor-webassembly-3-2-0-preview-2-release-now-available/).