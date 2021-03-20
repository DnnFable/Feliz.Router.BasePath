namespace Feliz.Router.List

module List =
    /// If the second list starts with the same elements as the first list
    /// a new list without these heading elements is created.
    let trim (list1: 'T list) (list2: 'T list) =
        let rec t (lst1: 'T list) (lst2: 'T list) =
            match lst1, lst2 with
            | [], l2s -> l2s
            | l1 :: l1s, l2 :: l2s when l1 = l2 -> t l1s l2s
            | _, _ -> list2

        t list1 list2
