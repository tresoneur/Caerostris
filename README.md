# Cærostris

## Demo

The latest version of _Cærostris_ can be accessed [__here__](https://caerostris.azurewebsites.net/). _Cærostris_ should be viewed on screens at least 1500 pixels wide.

## Status of this project

This project is currently under development.

## The goal of this project

To create a proof-of-concept Blazor WebAssembly PWA Spotify client with .NET languages and tooling.

## Remarks on the state of the proof of concept

While Blazor WebAssembly is not ready for production use yet, it is a very promising technology that the author believes will be a major player in web application development within a few years.

Other than the preview status of the framework, the lack of .NET wrappers (or preferably a single standard one) around browser APIs remains the main force standing in the way of general adoption. To get an idea about the main features still missing from Blazor WebAssembly, visit [this page](https://github.com/dotnet/aspnetcore/issues/21514).

## Pages

* __Playback__: displays the album art the way the Spotify desktop client shows it in fullscreen mode.

* __Context__: shows the currently playing artist, album or playlist.

* __Library__: lists the user's saved tracks in a customizable datagrid.

* __Insights__: displays graphs about several of the properties of the user's saved tracks.

* __Search__: lets the user search tracks, artists, albums and playlists.

## How to build

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

* Get a DevExpress Blazor (free (as in beer)) license and configure the project dependencies accordingly.

* Run `dotnet build`.

## Design considerations

* _Cærostris_ uses SCSS. Each razor folder contains a `Styles` folder with at least one `.scss` file, which are included in `/Styles/Site.scss` along with other `.scss` files in the `/Styles` folder that do not belong to any one particular component.

* _Cærostris_ is a [Progressive Web App](https://devblogs.microsoft.com/aspnet/blazor-webassembly-3-2-0-preview-2-release-now-available/).

## Shared components

(And where they are used in the app.)

* 📁 Cards
    
    * CardTileLayout: Context, Explore, Search

    * NavigationThumbnailCard: Context, Explore

        * AlbumCard

        * ArtistCard

        * PlaylistCard

        * TrackCard: Search

* 📁 Controls

    * 📁 Buttons

        * ActionText: e.g. artist name, album, playlist & track titles
        
        * InlineActionIcon: SaveButton; next to the Insights SubsectionTitle

        * PrimaryIconButton: play button

        * SaveButton: PlaybackBar

        * SecondaryIconButton: PlaybackBar

        * SecondaryIndicatorIconButton: PlaybackBar

    * 📁 Menu

        * ContextMenu: MainLayout

        * NavMenu: MainLayout

        * NavMenuItem: NavMenu, ContextMenu

    * Progressbar: CenteredLoadingIndicator, PlaybackBar

* 📁 Data

    * SavedTrackDataGrid: Library, Insights

    * UserPlaylistsList: MainLayout

* 📁 Info

    * CenteredInfo: Search, Context

    * CenteredLoadingIndicator: Context, Explore, Library, Insights

    * HighlightIfCurrentlyPlaying: Context

* 📁 Layout

    * MainLayout

* 📁 Providers

    * PlaybackContextProvider: MainLayout

* 📁 Typography

    * SubsectionTitle: ArtistPage, Insights, Search

* 📁 Utility

    * HeightMeasurementProvider: everywhere a DxDataGrid is used
