# Formatting Guide

## Captialisation
Capitalisation should follow standard C# conventions - the main ones being:

### Pascal Case:
* Class names
* Method names
* Property names
* Names of public static/readonly fields
* Names of constants
* Names of delegates

### Camel Case:
* Names of non-public fields
* Names of local variables

## Ordering
The inside of classes should be ordered in a standard layout in order to preserve readability. The order should follow this sort of pattern:

```
Public static properties
Public static fields
Non-public static properties
Non-public static fields
Public instance properties
Public instance fields
Non-public instance properties
Non-public instance fields
Static constructors
Instance constructors
Delegates
Public instance methods
Non-public instance methods
Public static methods
Non-public static methods
```

## Whitespace
Make sure to insert regular whitespace to prevent your code from being difficult to read. You don't need to put it after every line, but separating code into related chunks helps a lot. Example below:

```cs
array[i] = 1;
array[j] = 2;
array[k] = 3;
if (array[i] > array[j])
	array[k] = 4;
```

This code is currently quite dense, but it could be made easier to read be splitting it into related chunks:

```cs
array[i] = 1;
array[j] = 2;
array[k] = 3;

if (array[i] > array[j])
	array[k] = 4;
```

This code is more spaced out.

## Miscellaneous
* Ensure if/else/for/while statements are on different lines to their blocks, although it is acceptable to not include the curly braces if the block can be cleanly expressed in one line.
* Make regular use of local variables; this not only avoids repeated code but it's just easier to tell what the code is doing.
* In the case of projectile and NPC AI arrays, make sure you alias them with ref properties for ease of understanding:
```cs
private ref float Timer => ref npc.ai[0];
```
* Leave comments where necessary, preferably in the form of the triple-slash XML documentation where possible. This codebase is maintained by multiple people and ideally someone shouldn't need to ask what your code is supposed to do.
* Give variables sensible names that describe what they do, likewise with classes.

