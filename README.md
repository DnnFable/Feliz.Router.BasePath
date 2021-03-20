# Feliz.Router.BasePath [![Nuget](https://img.shields.io/nuget/v/Feliz.Router.BasePath?style=flat-square)](https://www.nuget.org/packages/Feliz.Router.BasePath/)

Feliz.Router.BaseBath offers three statefull hooks for [Feliz.Router](https://github.com/Zaid-Ajaj/Feliz.Router) to enable routing from URLs that have different base URLs than just '/ ' and should run in `pathMode`. 

```fsharp
router.useBaseUri () -> (BaseUrl * string list-> unit)
router.useBasePath (string)-> (BaseUrl * string list-> unit)
router.useBaseUrl (string list)-> (BaseUrl * string list-> unit)

type BaseUrl =
    { /// Current Url without basePath
      current: string list
      
      /// Creates a path including the base path
      /// * param url
      href: string list -> string
      
      /// Navigates to the provided url
      /// * param url
      goto: string list -> Browser.Types.MouseEvent -> unit

      /// Adds base path to the url for Cmd.navigatePath
      /// * param url
      navigatePath: string list -> string list }
```
### `router.useBaseUri` detects uri with help of base tag
This hook detects the base bath from `document.baseUri`. The baseUri can be set using a [base](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/base) element on your page:

```html
<base href="/some/where/deeply/nested" />
```

### router.useBaseUrl
Use this hook if you want to handle routing in a sub component

### router.useBasePath
This hook expects the base path as string. Usefull if the base tag is unset and you don't fully control the html of the page.

#### Example
```fsharp
feliz.Router
Feliz.Router.BasePath

[<ReactComponent>]
let App () =
    let (url, urlChanged) = router.useBasePath ("/some/where/deeply/nested")

    let activePage =
        match url.current with
        | [ ] -> Html.h1 "Home"
        | [ "users" ] -> Html.a 
                          [ prop.text "Show User with Id 0"
                            prop.href (url.href ["users", "0"])
                            prop.onClick (url.goto ["users", "0"]) ]
        | [ "users"; Route.Int userId ] -> 
           Html.h1 (sprintf "User ID %d" userId)
        | _ -> Html.h1 "Not found"

    React.router [ router.pathMode
                   router.onUrlChanged (urlChanged)
                   router.children [ activePage ]
```
## Installation

`dotnet add package Feliz.Router.BasePath`

## Development

```
dotnet tool restore
dotnet build .\src\BasePath.fsproj
```

### Tests

Before doing anything, start with installing npm dependencies using `npm install`.

The template includes a test project that ready to go which you can either run in the browser in watch mode or run in the console using node.js and mocha. To run the tests in watch mode:
```
npm run test:live
```
This command starts a development server for the test application and makes it available at http://localhost:8085.

To run the tests using the command line and of course in your CI server, you have to use the mocha test runner which doesn't use the browser but instead runs the code using node.js:
```
npm run test
```