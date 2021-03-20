module Tests

open Fable.Mocha
open Feliz.Router.List

let listTests =
    testList
        "List tests"
        [ testCase "trimming works"
          <| fun _ ->
              [ [], [], []
                [ "A"; "B" ], [], []
                [ "A"; "B" ], [ "A"; "B" ], []
                [ "A"; "B1" ], [ "A"; "B"; "C" ], [ "A"; "B"; "C" ]
                [ "A"; "B" ], [ "A"; "B"; "C" ], [ "C" ] ]
              |> List.iter
                  (fun (list1, list2, expected) ->

                      let result = List.trim list1 list2
                      Expect.equal result expected "Result not expected") ]


let allTests = testList "All" [ listTests ]

[<EntryPoint>]
let main (args: string []) = Mocha.runTests allTests
