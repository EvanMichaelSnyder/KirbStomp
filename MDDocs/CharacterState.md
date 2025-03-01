# CharacterState

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
| DesiredMovementDirection | `DirectionEnum` | |
| DesiredAttackDirection | `DirectionEnum` | |
| CurrentState | `StateEnum` | |
| FacingDirection | `DirectionEnum` | |
| MovementDirection | `DirectionEnum` | |
| IsGrounded | `Boolean` | |
| JumpsLeft | `Int32` | |

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
| GetFrameIndex | `Int32` |  |
| ResetFrameIndex | `Void` |  |
| IncrementFrameIndex | `Void` |  |
| getElapsedTime | `Single` |  |
| resetElapsedTime | `Void` |  |
| addToElapsedTime | `Void` | `Single` time |
| ResetJumps | `Void` |  |
| ConvertToRelativeAttackDirection | `DirectionEnum` | `DirectionEnum` direction |
| ConvertToRelativeMovementDirection | `DirectionEnum` | `DirectionEnum` direction |

<a id='static-methods'></a>
## Static Methods
### Public Static Methods
| Name | Return Type | Parameters |
|------|-------------|------------|
| EnterAttack | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterSpecial | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterJump | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterLanding | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterKnockedBack | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterIdle | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterFalling | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterRecover | `Void` | [`CharacterState`](CharacterState.md) current |
| EnterMovement | `Void` | [`CharacterState`](CharacterState.md) current |
| AlterMovement | `Void` | [`CharacterState`](CharacterState.md) current |

### Private Static Methods
*No private static methods*

<a id='constructors'></a>
## Constructors
| Constructor | Parameters |
|------------|------------|
| CharacterState |  |

<a id='interfaces'></a>
## Implemented Interfaces
*No implemented interfaces*

<a id='inheritance'></a>
## Inheritance
Inherits from: `Object`
<a id='static-fields'></a>
## Static Fields
*No public static fields*
