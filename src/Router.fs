module Feliz.Router.BasePath

open Feliz
open Feliz.UseElmish
open Feliz.Router
open Feliz.Router.List
open Elmish
open Browser.Types
open Browser.Dom


module Types =

    type State =
        { BaseUrl: string list
          CurrentUrl: string list }

    type Msg =
        | UrlChanged of string list
        | NavigateTo of string list

type BaseUrl =
    { /// Current Url without basePath
      current: string list
      /// Adds base path to the url.
      ///
      navigatePath: string list -> string list
      /// Navigates to the provided url
      /// * param url
      goto: string list -> Browser.Types.MouseEvent -> unit
      /// Creates a path including the base path
      /// * param url
      href: string list -> string }

module State =
    open Types

    let getBasePath () : string =

        let anchor =
            document.createElement ("a") :?> HTMLAnchorElement

        anchor.href <- document.baseURI
        anchor.pathname

    let init (baseUrl: string list) =
        { BaseUrl = baseUrl
          CurrentUrl = Router.currentPath () |> List.trim baseUrl },
        Cmd.none

    let update (msg: Msg) (state: State) =

        match msg with
        | UrlChanged url ->
            url
            |> List.trim state.BaseUrl
            |> fun url -> { state with CurrentUrl = url }, Cmd.none

        | NavigateTo url -> state, Cmd.navigatePath ((state.BaseUrl @ url) |> List.toArray)

open Types
open State

[<RequireQualifiedAccess>]
type router =

    /// Sets up *Feliz.Router* to work against a different base path than "/"
    ///
    /// Returns
    /// * _BaseUrl_ - Use it to setup navigation
    /// * UrlChanged dispatch for *Feliz.Router*
    static member useBaseUrl(baseurl: string list) =
        let (state, dispatch) =
            React.useElmish ((fun () -> init baseurl), update, [||])

        { current = state.CurrentUrl
          navigatePath = fun url -> state.BaseUrl @ url
          href = fun url -> state.BaseUrl @ url |> Router.formatPath
          goto =
              fun url ev ->
                  url |> NavigateTo |> dispatch
                  ev.preventDefault () },
        UrlChanged >> dispatch

    /// Sets up *Feliz.Router* to work against a different base path than "/"
    ///
    /// Returns
    /// * _BaseUrl_ - Use it to setup navigation
    /// * UrlChanged dispatch for *Feliz.Router*
    static member useBasePath(basePath: string) =
        Router.urlSegments basePath RouteMode.Path
        |> router.useBaseUrl

    /// Sets up *Feliz.Router* to work against document.baseUri
    ///
    /// Returns
    /// * _BaseUrl_ - Use it to setup navigation
    /// * UrlChanged dispatch for *Feliz.Router*
    ///
    /// document.baseUri can be set with the [base](https://developer.mozilla.org/docs/Web/HTML/Element/base) tag
    static member useBaseUri() =
        Router.urlSegments (getBasePath ()) RouteMode.Path
        |> router.useBaseUrl
