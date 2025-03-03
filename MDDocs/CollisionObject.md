# CollisionObject

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
| Physics | [`PhysicsComponent`](PhysicsComponent.md) | |
| IsActive | `Boolean` | |
| Carriers | List<[`Carrier`](Carrier.md)> | |

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
| RegisterCollisionResponse | `Void` | `HitboxTypeEnum` selfType, `HitboxTypeEnum` otherType, Action<[`CollisionObject`](CollisionObject.md), `CollisionContext`> handler |
| HandleCollision | `Void` | `CollisionContext` context |

<a id='static-methods'></a>
## Static Methods
### Public Static Methods
*No public static methods*

### Private Static Methods
*No private static methods*

<a id='constructors'></a>
## Constructors
| Constructor | Parameters |
|------------|------------|
| CollisionObject |  |

<a id='interfaces'></a>
## Implemented Interfaces
*No implemented interfaces*

<a id='inheritance'></a>
## Inheritance
Inherits from: [`PhysicsComponent`](PhysicsComponent.md)

### Inheritance Chain
```
CollisionObject → PhysicsComponent → Object
```
<a id='static-fields'></a>
## Static Fields
*No public static fields*
