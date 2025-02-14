using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KirbStomp;
public enum StateEnum
{
    DEBUG,
    //Movement
    Idle,
    Walk,
    Run,
    Sprint,
    SlideTurn,
    Crouch,
    Jump,
    AirIdle,
    AirMove,
    Landing,
    //Attack
    AttackNeutral,
    AttackNeutral2,
    AttackNeutral3,
    AttackBack,
    AttackForward,
    AttackUp,
    AttackDown,
    AttackDash,
    //Aerial
    AerialNeutral,
    AerialForward,
    AerialBack,
    AerialUp,
    AerialDown,
    //Special
    SpecialNeutral,
    SpecialForward,
    SpecialBack,
    SpecialUp,
    SpecialDown,
    //
    KnockedBack,
    Ragdolled,
    LayingDown,
    Recover,
    FreeFall,
}

public enum EventType
{
    // Permissive events
    TryAttack,
    TrySpecial,
    TryJump,
    TryMove,
    GotHit,
    // Non-permissive events
    HitGround,
    EndOfState
}
public enum DirectionEnum
{
    None,
    Right,
    Left,
    Up,
    Down,
    DEBUG,
    //relative
    Forward,
    Back,
}
public enum AccelEnum
{
    Faster,
    Slower,
    Same,
}