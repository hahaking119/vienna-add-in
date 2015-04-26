We (at IOS) are using the [JetBrains ReSharper](http://www.jetbrains.com/resharper/index.html) Add-In for Visual Studio. Here are the settings we use for the vienna-add-in project.

# To-do Items #

We use ReSharper's To-do Items feature to annotate implementations of the UN/CEFACT's Naming and Design Rules in the code.

## Patterns ##

**NDR-Rule**: The following pattern is used to match rule identifiers:

```
(\W|^)(?<TAG>\[R ....\])(\W|$)(.*)
```

Usage example:

```
/// [R 1234]: This is the implementation of rule 1234.
```

**Deviations**: The following pattern is used to annotate deviations from NDR rules:

```
(\W|^)(?<TAG>DEVIATION)(\W|$)(.*)
```

Usage example:

```
/// [R 1234]
/// This is the implementation of rule 1234.
/// Deviation: We have implemented a simplified version of rule 1234.
```

## Setup ##

  1. Open the ReSharper Options panel (`ReSharper > Options`).
  1. Select `To-do Items` section.
  1. Create the two patterns described above.
  1. Create or modify a filter to include the new patterns.

## Usage ##

  1. Open the ReSharper To-do Explorer (`ReSharper > Windows > To-do Explorer`).
  1. Select a filter including the NDR patterns.