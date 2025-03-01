# Character

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
| Name | Type | Description |
|------|------|-------------|
| GetButtonDataManager | [`ButtonDataManager`](ButtonDataManager.md) | |

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
| Name | Return Type | Parameters |
|------|-------------|------------|
| DoBehavior | `Void` |  |
| Animate | `Void` | `GameTime` gameTime |
| DebugState | `Void` |  |
| Draw | `Void` | `SpriteBatch` spriteBatch |
| DrawHitbox | `Void` | `SpriteBatch` spriteBatch |
| UpdateState | `Void` |  |
| ProcessButtons | `Void` |  |
| ApplyMovementBehavior | `Void` |  |
| MoveCharacter | `Void` | `GameTime` gameTime |
| CheckGroundCollision | `Void` |  |
| Gravity | `Void` | `GameTime` gameTime |

<a id='static-methods'></a>
## Static Methods
### Public Static Methods
*No public static methods*

### Private Static Methods
| Name | Return Type | Parameters |
|------|-------------|------------|
| IsDirection | `Boolean` | `GameButtons` input |

<a id='constructors'></a>
## Constructors
| Constructor | Parameters |
|------------|------------|
| Character | `String` name, `Texture2D` spriteSheet, `String` spriteSheetName |
| Character | `String` name, `Texture2D` spriteSheet, `String` spriteSheetName, `Vector2` spawnLocation |

<a id='interfaces'></a>
## Implemented Interfaces
- `ICharacter`

<a id='inheritance'></a>
## Inheritance
Inherits from: `Object`
<a id='static-fields'></a>
## Static Fields
*No public static fields*
