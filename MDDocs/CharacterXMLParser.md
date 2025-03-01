# CharacterXMLParser

## Quick Navigation
- [Properties](#properties)
- [Static Properties](#static-properties)
- [Methods](#methods)
- [Static Methods](#static-methods)
- [Constructors](#constructors)
- [Interfaces](#interfaces)
- [Inheritance](#inheritance)
- [Back to Index](index.md)

<a id='properties'></a>
## Properties
### Public Properties
*No public properties*

### Private Properties
*No private properties*

<a id='static-properties'></a>
## Static Properties
### Public Static Properties
*No public static properties*

### Private Static Properties
*No private static properties*

<a id='methods'></a>
## Methods
### Public Methods
*No public methods*

<a id='static-methods'></a>
## Static Methods
### Public Static Methods
| Name | Return Type | Parameters |
|------|-------------|------------|
| LoadCharacter | `Void` | `String` name |

### Private Static Methods
| Name | Return Type | Parameters |
|------|-------------|------------|
| GetCharacterStats | `TempCharacterStats` | `String` name |
| ParseCharacterXML | `TempCharacterStats` | `String` fileName |
| GetCharacterStatsXElement | `XElement` | `String` name |
| GetXElementOrAssert | `XElement` | `String` name, `XElement` parent |
| GetMovementStatsFromCharacter | `MovementStats` | `XElement` characterElement |
| GetPhysicsStatsFromCharacter | `PhysicsStats` | `XElement` characterElement |

<a id='constructors'></a>
## Constructors
| Constructor | Parameters |
|------------|------------|
| CharacterXMLParser |  |

<a id='interfaces'></a>
## Implemented Interfaces
*No implemented interfaces*

<a id='inheritance'></a>
## Inheritance
Inherits from: `Object`
<a id='static-fields'></a>
## Static Fields
*No public static fields*
