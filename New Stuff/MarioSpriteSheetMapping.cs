using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using KirbStomp;
using static MarioState;
using static KirbStomp.StateEnum;

public enum MarioState
{
    //Used for state Identification only animation knows about the 2-6
    None,
    //Idle Right
    IdleRight,                  
    IdleRight2,
    IdleRight3,
    IdleRight4,
    IdleRight5,
    IdleRight6,
    IdleRight7,
    IdleRight8,
    IdleRight9,
    IdleRight10,

    //Idle Left
    IdleLeft,
    IdleLeft2,
    IdleLeft3,
    IdleLeft4,
    IdleLeft5,
    IdleLeft6,
    IdleLeft7,
    IdleLeft8,
    IdleLeft9,
    IdleLeft10,

    //Jump Right
    JumpRight,
    JumpRight2,
    JumpRight3,

    //Land Right
    LandRight,
    LandRight2,
    LandRight3,

    //Jump Left
    JumpLeft,
    JumpLeft2,
    JumpLeft3,
    JumpLeft4,
    JumpLeft5,
    JumpLeft6,

    //RunningRight
    RunRight,
    RunRight2,
    RunRight3,
    RunRight4,
    RunRight5,
    RunRight6,
    RunRight7,
    RunRight8,

    //QuickTurnToLeft
    TurnLeftWhileRunRight,
    TurnLeftWhileRunRight2,//not a unique frame

    //QuickTurnToRight
    TurnRightWhileRunLeft,
    TurnRightWhileRunLeft2,

    //RunningLeft
    RunLeft,
    RunLeft2,
    RunLeft3,
    RunLeft4,
    RunLeft5,
    RunLeft6,
    RunLeft7,
    RunLeft8,

    //NeutralAttackRight
    //Start Part
    StartNeutralAttackRight,
    StartNeutralAttackRight2,
    StartNeutralAttackRight3,
    StartNeutralAttackRight4,
    StartNeutralAttackRight5,
    //Middle Part
    MiddleNeutralAttackRight,
    MiddleNeutralAttackRight2,
    MiddleNeutralAttackRight3,
    MiddleNeutralAttackRight4,
    MiddleNeutralAttackRight5,
    //End Part
    LastNeutralAttackRight,
    LastNeutralAttackRight2,
    LastNeutralAttackRight3,
    LastNeutralAttackRight4,
    LastNeutralAttackRight5,
    LastNeutralAttackRight6,
    LastNeutralAttackRight7,

    //Up Special Attack
    UpSpecialRight,
    UpSpecialRight2,
    UpSpecialRight3,
    UpSpecialRight4,
    UpSpecialRight5,
    //UpSpecialFall
    UpSpecialFallRight,
    UpSpecialFallRight2,
    UpSpecialFallRight3,


    //Hit

    //Laying On Ground
    LayingRight,
    //GetUp
    GetUpRight,
    GetUpRight2,
    GetUpRight3,

    //Up tilt
    UpTiltRight,
    UpTiltRight2,
    UpTiltRight3,
    UpTiltRight4,
    UpTiltRight5,
    UpTiltRight6,
    UpTiltRight7,

    //Up Aerial
    UpAerialRight,
    UpAerialRight2,
    UpAerialRight3,
    UpAerialRight4,
    UpAerialRight5,
    UpAerialRight6,

    //Up kick spin
    UpKickSpinRight,
    UpKickSpinRight2,
    UpKickSpinRight3,
    UpKickSpinRight4,
    UpKickSpinRight5,
    UpKickSpinRight6,
    UpKickSpinRight7,
    UpKickSpinRight8,

    //Down Tilt
    DownTiltRight,
    DownTiltRight2,
    DownTiltRight3,
    DownTiltRight4,
    DownTiltRight5,
    DownTiltRight6,
    DownTiltRight7,
    DownTiltRight8,
    DownTiltRight9,

    //Down Aerial
    DownAerialRight,
    DownAerialRight2,
    DownAerialRight3,
    DownAerialRight4,
    DownAerialRight5,
    DownAerialRight6,

    //Down Spin
    DownSpinRight,
    DownSpinRight2,
    DownSpinRight3,
    DownSpinRight4,
    DownSpinRight5,
    DownSpinRight6,
    DownSpinRight7,

    // Left/Right Cape Special
    StartSideSpecialRight,
    StartSideSpecialRight2,
    StartSideSpecialRight3,
    MiddleSideSpecialRight,
    MiddleSideSpecialRight2,
    MiddleSideSpecialRight3,
    MiddleSideSpecialRight4,
    MiddleSideSpecialRight5,
    MiddleSideSpecialRight6,
    LastSideSpecialRight,
    LastSideSpecialRight2,

    //Down Special Attack
    StartDownSpecialRight,
    StartDownSpecialRight2,
    StartDownSpecialRight3,
    StartDownSpecialRight4,
    StartDownSpecialRight5,
    StartDownSpecialRight6,
    StartDownSpecialRight7,

    //Down Special Power
    EndDownSpecialRight,
    EndDownSpecialRight2,
    EndDownSpecialRight3,
    EndDownSpecialRight4,
    EndDownSpecialRight5,
    EndDownSpecialRight6,
    EndDownSpecialRight7,

    //Special Punch Attack
    SpecialPunchAttackRight,
    SpecialPunchAttackRight2,
    SpecialPunchAttackRight3,
    SpecialPunchAttackRight4,
    SpecialPunchAttackRight5,
    SpecialPunchAttackRight6,
    SpecialPunchAttackRight7,
    SpecialPunchAttackRight8,
    SpecialPunchAttackRight9,
    SpecialPunchAttackRight10,

    //Rolling
    RollingRight,
    RollingRight2,
    RollingRight3,
    RollingRight4,
    RollingRight5,
    RollingRight6,
    RollingRight7,
    RollingRight8,

    //Guard
    GuardRight,
    GuardRight2,
    GuardRight3,
    GuardRight4,
    
    //Free Fall
    FreeFallRight,
    FreeFallRight2,
    FreeFallRight3,
    FreeFallRight4,
    FreeFallRight5,
    FreeFallRight6,
    FreeFallRight7,
    FreeFallRight8

}

public class MarioSpriteSheetMapping
{
    public static Dictionary<StateEnum, MarioState> convertToMarioState =
        new Dictionary<StateEnum, MarioState>
        {
                //Movement                          
                {Idle,IdleRight                     },     
                {Walk,RunRight                      },
                {Run,RunRight                       },   
                {Sprint,RunRight                    },    
                {SlideTurn,TurnLeftWhileRunRight    },                
                {Crouch,None                        },
                {Jump,JumpRight                     },
                {AirIdle,JumpRight3                },      
                {AirMove,JumpRight3                 },      
                {Landing,LandRight                  },
        };
        public static Dictionary<MarioState, StateEnum> convertToEnumState =
        new Dictionary<MarioState, StateEnum>
        {
                    //Movement                          
                    {IdleRight             , Idle              },
                    {RunRight              , Walk              },
                    {RunRight2              , Run                },
                    {RunRight3              , Sprint          },
                    {TurnLeftWhileRunRight , SlideTurn    },
                    {None                  , Crouch          },
                    {JumpRight             , Jump              },
                    {JumpRight3            , AirIdle        },
                    //{JumpRight3            , AirMove        },
                    {LandRight             , Landing        },
        };
    public static Dictionary<MarioState, bool> isFinalFrame =
    new Dictionary<MarioState, bool>
        {
            { IdleRight10,true },
            { JumpRight3,true },
            { LandRight3,true },
            { RunRight8,true },
            {default,false },

        };

    public static double MarioSpriteScale = 1.2; //changing this will change behaivor think of this as a multiplier for character size
    // Using a tuple to store Rectangle (sprite), Vector2 (offset), Vector2 (scale) and MarioState (next frame)
    // scale is also used for inversion
    public static Dictionary<MarioState, (Rectangle sprite, Vector2 offsetAnimation, Vector2 offsetState, int frameDuration, MarioState nextFrame)> StateToSpriteMap = new Dictionary<MarioState, (Rectangle sprite, Vector2 offsetTransition, Vector2 offsetState, int frameDuration, MarioState nextFrame)>
    {
        //IDLE RIGHT
            { IdleRight,  (new Rectangle(18, 23, 23, 38),  new Vector2(0, 0), new Vector2(0, 0),  100, IdleRight2) },
            { IdleRight2, (new Rectangle(45, 23, 23, 38),  new Vector2(0, 0), new Vector2(0, 0),  100, IdleRight3) },
            { IdleRight3, (new Rectangle(72, 23, 23, 38),  new Vector2(0, 0), new Vector2(0, 0),  100, IdleRight4) },
            { IdleRight4, (new Rectangle(99, 23, 24, 38),  new Vector2(0, 0), new Vector2(0, 0),  100, IdleRight5) },
            { IdleRight5, (new Rectangle(126, 23, 25, 38), new Vector2(0, 0), new Vector2(0, 0), 100, IdleRight6) },
            { IdleRight6, (new Rectangle(153, 23, 25, 38), new Vector2(0, 0), new Vector2(0, 0), 100, IdleRight7) },
            { IdleRight7, (new Rectangle(126, 23, 25, 38), new Vector2(0, 0), new Vector2(0, 0), 100, IdleRight8) },
            { IdleRight8, (new Rectangle(99, 23, 24, 38),  new Vector2(0, 0), new Vector2(0, 0),  100, IdleRight9) },
            { IdleRight9, (new Rectangle(72, 23, 23, 38),  new Vector2(0, 0), new Vector2(0, 0),  100, IdleRight10) },
            { IdleRight10, (new Rectangle(45, 23, 23, 38), new Vector2(0, 0), new Vector2(0, 0),  100, IdleRight) },


        //IDLE LEFT
        //Process to get inversions
        //Length 1104 - original X
            { IdleLeft,  (new Rectangle(1104-23-18, 23, 23, 38), new Vector2(0, 0), new Vector2(2, 0), 100, IdleLeft2) },
            { IdleLeft2, (new Rectangle(1104-23-45, 23, 23, 38), new Vector2(0, 0), new Vector2(2, 0), 100, IdleLeft3) },
            { IdleLeft3, (new Rectangle(1104-23-72, 23, 23, 38), new Vector2(0, 0), new Vector2(2, 0), 100, IdleLeft4) },
            { IdleLeft4, (new Rectangle(1104-24-99, 23, 24, 38), new Vector2(0, 0), new Vector2(1, 0), 100, IdleLeft5) },
            { IdleLeft5, (new Rectangle(1104-25-126, 23, 25, 38), new Vector2(0, 0), new Vector2(0, 0), 100, IdleLeft6) },
            { IdleLeft6, (new Rectangle(1104-25-153, 23, 25, 38), new Vector2(0, 0), new Vector2(0, 0), 100, IdleLeft7) },
            { IdleLeft7, (new Rectangle(1104-25-126, 23, 25, 38), new Vector2(0, 0), new Vector2(0, 0), 100, IdleLeft8) },
            { IdleLeft8, (new Rectangle(1104-24-99, 23, 24, 38), new Vector2(0, 0), new Vector2(1, 0), 100, IdleLeft9) },
            { IdleLeft9, (new Rectangle(1104-23-72, 23, 23, 38), new Vector2(0, 0), new Vector2(2, 0), 100, IdleLeft10) },
            { IdleLeft10, (new Rectangle(1104-23-45, 23, 23, 38), new Vector2(0, 0), new Vector2(2, 0), 100, IdleLeft) },

        

        //Jump RIGHT
            { JumpRight,  (new Rectangle(15, 84, 24, 42), new Vector2(0, 0), new Vector2(0, -4), 100, JumpRight2) },
            { JumpRight2, (new Rectangle(45, 84, 27, 42), new Vector2(0, 0), new Vector2(0, -4), 70, JumpRight3) },
            { JumpRight3, (new Rectangle(78, 84, 29, 42), new Vector2(0, 0), new Vector2(0, -4), 100, JumpRight3) },

        //Land RIGHT
            { LandRight, (new Rectangle(111, 84, 29, 42), new Vector2(0, 0), new Vector2(0, -4), 100, LandRight2) },
            { LandRight2, (new Rectangle(141, 84, 29, 42), new Vector2(0, 0), new Vector2(0, -4), 100, LandRight3) },
            { LandRight3, (new Rectangle(171, 84, 29, 42), new Vector2(0, 0), new Vector2(0, -4), 100, LandRight3) },

        //Run Right
            { RunRight,  (new Rectangle(12, 148, 28, 39), new Vector2(0, 0), new Vector2(0, -1),  70, RunRight2) },
            { RunRight2, (new Rectangle(44, 148, 30, 39), new Vector2(0, 0), new Vector2(0, -1),  70, RunRight3) },
            { RunRight3, (new Rectangle(82, 148, 29, 39), new Vector2(0, 0), new Vector2(0, -1),  70, RunRight4) },
            { RunRight4, (new Rectangle(116, 148, 24, 39), new Vector2(0, 0), new Vector2(0, -1),  70, RunRight5) },
            { RunRight5, (new Rectangle(146, 148, 28, 39), new Vector2(0, 0), new Vector2(0, -1), 70, RunRight6) },
            { RunRight6, (new Rectangle(181, 148, 30, 39), new Vector2(0, 0), new Vector2(0, -1), 70, RunRight7) },
            { RunRight7, (new Rectangle(220, 148, 28, 39), new Vector2(0, 0), new Vector2(0, -1), 70, RunRight8) },
            { RunRight8, (new Rectangle(254, 148, 24, 39), new Vector2(0, 0), new Vector2(0, -1),  70, RunRight) },

        //Turn While Running Right From Right To Left
            { TurnLeftWhileRunRight, (new Rectangle(311, 175, 22, 36), new Vector2(0, 0), new Vector2(0, 2),  100, TurnLeftWhileRunRight2) },
            { TurnLeftWhileRunRight2, (new Rectangle(311, 175, 22, 36), new Vector2(0, 0), new Vector2(0, 2),  100, RunLeft) },

        //Turn While Running Right From Right To Left
            { TurnRightWhileRunLeft, (new Rectangle(1104-22-311, 175, 22, 36), new Vector2(0, 0), new Vector2(0, 2),  100, TurnRightWhileRunLeft2) },
            { TurnRightWhileRunLeft2, (new Rectangle(1104-22-311, 175, 22, 36), new Vector2(0, 0), new Vector2(0, 2),  100, RunRight) },

        //Run Left
            { RunLeft,  (new Rectangle(1104-12-28, 148, 28, 39), new Vector2(0, 0), new Vector2(2, -1),  70, RunLeft2) },
            { RunLeft2, (new Rectangle(1104-44-30, 148, 30, 39), new Vector2(0, 0), new Vector2(0, -1),  70, RunLeft3) },
            { RunLeft3, (new Rectangle(1104-82-29, 148, 29, 39), new Vector2(0, 0), new Vector2(1, -1),  70, RunLeft4) },
            { RunLeft4, (new Rectangle(1104-116-24, 148, 24, 39), new Vector2(0, 0), new Vector2(6, -1),  70, RunLeft5) },
            { RunLeft5, (new Rectangle(1104-146-28, 148, 28, 39), new Vector2(0, 0), new Vector2(2, -1), 70, RunLeft6) },
            { RunLeft6, (new Rectangle(1104-181-30, 148, 30, 39), new Vector2(0, 0), new Vector2(0, -1), 70, RunLeft7) },
            { RunLeft7, (new Rectangle(1104-220-28, 148, 28, 39), new Vector2(0, 0), new Vector2(2, -1), 70, RunLeft8) },
            { RunLeft8, (new Rectangle(1104-254-24, 148, 24, 39), new Vector2(0, 0), new Vector2(6, -1),  70, RunLeft) },

        //Neutral Attack Right
            //from anchor of right foot offset 0 is needed
            //As tallest sprite is 34 offset of 4 is needed as it transitions from sprite with height 38
            { StartNeutralAttackRight,  (new Rectangle(12, 261, 36, 34), new Vector2(0, 0), new Vector2(0, 4),  50,  StartNeutralAttackRight2) },
            { StartNeutralAttackRight2, (new Rectangle(54, 261, 48, 34), new Vector2(0, 0), new Vector2(0, 4),  50,  StartNeutralAttackRight3) },
            { StartNeutralAttackRight3, (new Rectangle(105, 261, 44, 34), new Vector2(0, 0), new Vector2(0, 4),  50, StartNeutralAttackRight4) },
            { StartNeutralAttackRight4, (new Rectangle(152, 261, 34, 34), new Vector2(0, 0), new Vector2(0, 4),  50, StartNeutralAttackRight5) },
            { StartNeutralAttackRight5, (new Rectangle(192, 261, 34, 34), new Vector2(0, 0), new Vector2(0, 4), 200, MiddleNeutralAttackRight) },
            //End of part 1
            { MiddleNeutralAttackRight, (new Rectangle(233, 261, 31, 34), new Vector2(0, 0), new Vector2(0, 4), 50,   MiddleNeutralAttackRight2) },
            { MiddleNeutralAttackRight2, (new Rectangle(270, 261, 31, 34), new Vector2(0, 0), new Vector2(0, 4), 50,   MiddleNeutralAttackRight3) },
            { MiddleNeutralAttackRight3, (new Rectangle(307, 261, 44, 34), new Vector2(0, 0), new Vector2(0, 4),  50,  MiddleNeutralAttackRight4) },
            { MiddleNeutralAttackRight4, (new Rectangle(356, 261, 40, 34), new Vector2(0, 0), new Vector2(0, 4),  50,  MiddleNeutralAttackRight5) },
            { MiddleNeutralAttackRight5, (new Rectangle(400, 261, 35, 34), new Vector2(0, 0), new Vector2(0, 4),  200, LastNeutralAttackRight) },
            //End of part 2
            //Tallest Sprite is 44 so offset of -6 is required
            { LastNeutralAttackRight, (new Rectangle(13, 303, 27, 44), new Vector2(0, 0), new Vector2(0, -6),  50, LastNeutralAttackRight2) },
            { LastNeutralAttackRight2, (new Rectangle(46, 303, 23, 44), new Vector2(0, 0), new Vector2(0, -6), 50, LastNeutralAttackRight3) },
            { LastNeutralAttackRight3, (new Rectangle(73, 303, 48, 44), new Vector2(0, 0), new Vector2(0, -6), 50, LastNeutralAttackRight4) },
            { LastNeutralAttackRight4, (new Rectangle(125, 303, 40, 44), new Vector2(0, 0), new Vector2(0, -6), 50, LastNeutralAttackRight5) },
            { LastNeutralAttackRight5, (new Rectangle(169, 303, 32, 44), new Vector2(0, 0), new Vector2(0, -6),  50, LastNeutralAttackRight6) },
            { LastNeutralAttackRight6, (new Rectangle(205, 303, 23, 44), new Vector2(0, 0), new Vector2(0, -6),  50, LastNeutralAttackRight7) },
            { LastNeutralAttackRight7, (new Rectangle(232, 303, 26, 44), new Vector2(0, 0), new Vector2(0, -6),  200, StartNeutralAttackRight) },  


        //Up Special
            //Attack
                { UpSpecialRight,  (new Rectangle(11, 1261, 33, 56), new Vector2(0, 0), new Vector2(0, -18),  100,  UpSpecialRight2) },
                { UpSpecialRight2, (new Rectangle(50, 1261, 38, 56), new Vector2(0, 0), new Vector2(0, -18),  100,  UpSpecialRight3) },
                { UpSpecialRight3, (new Rectangle(94, 1261, 32, 56), new Vector2(0, 0), new Vector2(0, -18),  100, UpSpecialRight4) },
                { UpSpecialRight4, (new Rectangle(132, 1261, 27, 56), new Vector2(0, 0), new Vector2(0, -18),  100, UpSpecialRight5) },
                { UpSpecialRight5, (new Rectangle(165, 1261, 23, 56), new Vector2(0, 0), new Vector2(0, -18), 100, UpSpecialFallRight) },  //This could be argued to be fall anim
            //Fall
                { UpSpecialFallRight, (new Rectangle(194, 1261, 24, 56), new Vector2(-3, 0), new Vector2(0, -18),  100, UpSpecialFallRight2) },
                { UpSpecialFallRight2, (new Rectangle(224, 1261, 31, 56), new Vector2(-3, 0), new Vector2(0, -18),  100, UpSpecialFallRight3) },
                { UpSpecialFallRight3, (new Rectangle(261, 1261, 33, 56), new Vector2(-3, 0), new Vector2(0, -18), 100, UpSpecialRight) },

        //Laying on ground normally unanimated
                { LayingRight, (new Rectangle(165, 1518, 42, 39), new Vector2(-20, 0), new Vector2(0, -1),  300, GetUpRight) },
        //Getup From Laying Down Right
                { GetUpRight, (new Rectangle(213, 1518, 22, 39), new Vector2(0, 0), new Vector2(0, -1),  200, GetUpRight2) },
                { GetUpRight2, (new Rectangle(241, 1518, 24, 39), new Vector2(0, 0), new Vector2(0, -1),  200, GetUpRight3) },
                { GetUpRight3, (new Rectangle(271, 1518, 25, 39), new Vector2(0, 0), new Vector2(0, -1), 1000, LayingRight) },

        //Up 
            //Up Tilt
            //default height should be is 58, but highest height is 48, so we need offset of 9?
            { UpTiltRight,  (new Rectangle(12, 456, 39, 57), new Vector2(-15, 0), new Vector2(0, -19),  100, UpTiltRight2) },
            { UpTiltRight2, (new Rectangle(57, 456, 28, 57), new Vector2(0, 0), new Vector2(0, -19),  100, UpTiltRight3) },
            { UpTiltRight3, (new Rectangle(91, 456, 22, 57), new Vector2(10, 0), new Vector2(0, -19),  100, UpTiltRight4) },
            { UpTiltRight4, (new Rectangle(119, 456, 39, 57), new Vector2(0, 0), new Vector2(0, -19), 100, UpTiltRight5) },
            { UpTiltRight5, (new Rectangle(164, 456, 36, 57), new Vector2(0, 0), new Vector2(0, -19), 100, UpTiltRight6) },
            { UpTiltRight6, (new Rectangle(204, 456, 22, 57), new Vector2(12, 0), new Vector2(0, -19), 100, UpTiltRight7) },
            { UpTiltRight7, (new Rectangle(233, 456, 26, 57), new Vector2(0, 0), new Vector2(0, -19), 100, UpTiltRight) },
            
            //Up Aerial
            //can't do fixed height since there is a special character on the first and second sprite
            { UpAerialRight,  (new Rectangle(14, 540, 35, 48), new Vector2(0, 5), new Vector2(0, -15), 100, UpAerialRight2) },
            { UpAerialRight2, (new Rectangle(54, 540, 34, 48), new Vector2(0, 5), new Vector2(0, -15), 100, UpAerialRight3) },
            { UpAerialRight3, (new Rectangle(96, 535, 22, 53), new Vector2(0, 0), new Vector2(0, -15), 100, UpAerialRight4) },
            { UpAerialRight4, (new Rectangle(126, 535, 20, 53), new Vector2(0, 0), new Vector2(0, -15), 100, UpAerialRight5) },
            { UpAerialRight5, (new Rectangle(154, 535, 22, 53), new Vector2(0, 0), new Vector2(0, -15), 100, UpAerialRight6) },
            { UpAerialRight6, (new Rectangle(182, 535, 22, 53), new Vector2(1, 0), new Vector2(0, -15), 100, UpAerialRight) },

            //Up Kick Spin
            //change offset for kicks, 
            { UpKickSpinRight, (new Rectangle(14, 612, 37, 77), new Vector2(0, 0), new Vector2(0, -39), 100, UpKickSpinRight2) },
            { UpKickSpinRight2, (new Rectangle(57, 612, 50, 77), new Vector2(2, 0), new Vector2(0, -39), 100, UpKickSpinRight3) },
            { UpKickSpinRight3, (new Rectangle(113, 612, 37, 77), new Vector2(-1, 0), new Vector2(0, -39), 100, UpKickSpinRight4) },
            { UpKickSpinRight4, (new Rectangle(156, 612, 38, 77), new Vector2(-18, 0), new Vector2(0, -39), 100, UpKickSpinRight5) },
            { UpKickSpinRight5, (new Rectangle(200, 612, 39, 77), new Vector2(-18, 0), new Vector2(0, -39), 100, UpKickSpinRight6) },
            { UpKickSpinRight6, (new Rectangle(244, 612, 38, 77), new Vector2(-18, 0), new Vector2(0, -39), 100, UpKickSpinRight7) },
            { UpKickSpinRight7, (new Rectangle(287, 612, 24, 77), new Vector2(0, 0), new Vector2(0, -39), 100, UpKickSpinRight8) },
            { UpKickSpinRight8, (new Rectangle(316, 612, 30, 77), new Vector2(0, 0), new Vector2(0, -39), 100, UpKickSpinRight) },
            // for testing purpose, this will be changed later to idle

        //Down
            //Down Tilt
            { DownTiltRight, (new Rectangle(14, 719, 29, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownTiltRight2) },
            { DownTiltRight2, (new Rectangle(50, 719, 33, 40), new Vector2(-3, 0), new Vector2(0, -2), 100, DownTiltRight3) },
            { DownTiltRight3, (new Rectangle(91, 719, 38, 40), new Vector2(12, 0), new Vector2(0, -2), 100, DownTiltRight4) },
            { DownTiltRight4, (new Rectangle(141, 719, 24, 40), new Vector2(12, 0), new Vector2(0, -2), 100, DownTiltRight5) },
            { DownTiltRight5, (new Rectangle(169, 719, 41, 40), new Vector2(-10, 0), new Vector2(0, -2), 100, DownTiltRight6) },
            { DownTiltRight6, (new Rectangle(217, 719, 34, 40), new Vector2(-2, 0), new Vector2(0, -2), 100, DownTiltRight7) },
            { DownTiltRight7, (new Rectangle(260, 719, 23, 40), new Vector2(5, 0), new Vector2(0, -2), 100, DownTiltRight8) },
            { DownTiltRight8, (new Rectangle(290, 719, 24, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownTiltRight9) },
            { DownTiltRight9, (new Rectangle(323, 719, 26, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownTiltRight) },
            
            //Down Aerial
            { DownAerialRight, (new Rectangle(14, 788, 25, 35), new Vector2(0, 0), new Vector2(0, 3), 100, DownAerialRight2) },
            { DownAerialRight2, (new Rectangle(43, 788, 52, 35), new Vector2(0, 0), new Vector2(0, 3), 100, DownAerialRight3) },
            { DownAerialRight3, (new Rectangle(99, 788, 35, 35), new Vector2(0, 0), new Vector2(0, 3), 100, DownAerialRight4) },
            { DownAerialRight4, (new Rectangle(144, 788, 33, 35), new Vector2(0, 0), new Vector2(0, 3), 100, DownAerialRight5) },
            { DownAerialRight5, (new Rectangle(181, 788, 29, 35), new Vector2(0, 0), new Vector2(0, 3), 100, DownAerialRight6) },
            { DownAerialRight6, (new Rectangle(214, 788, 31, 35), new Vector2(0, 0), new Vector2(0, 3), 100, DownAerialRight) },

            //Down Spin
            { DownSpinRight, (new Rectangle(13, 853, 31, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownSpinRight2) },
            { DownSpinRight2, (new Rectangle(50, 853, 23, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownSpinRight3) },
            { DownSpinRight3, (new Rectangle(79, 853, 26, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownSpinRight4) },
            { DownSpinRight4, (new Rectangle(111, 853, 29, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownSpinRight5) },
            { DownSpinRight5, (new Rectangle(146, 853, 27, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownSpinRight6) },
            { DownSpinRight6, (new Rectangle(179, 853, 28, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownSpinRight7) },
            { DownSpinRight7, (new Rectangle(213, 853, 31, 40), new Vector2(0, 0), new Vector2(0, -2), 100, DownSpinRight) },

        //Special attacks
            
            //Cape Attack
            { StartSideSpecialRight, (new Rectangle(12, 1321, 22, 53), new Vector2(0, 0), new Vector2(0, -15), 100, StartSideSpecialRight2) },
            { StartSideSpecialRight2, (new Rectangle(40, 1321, 30, 53), new Vector2(-4, 0), new Vector2(0, -15), 100, StartSideSpecialRight3)},
            { StartSideSpecialRight3, (new Rectangle(80, 1321, 23, 53), new Vector2(-1, 0), new Vector2(0, -15), 100, MiddleSideSpecialRight) },
            { MiddleSideSpecialRight, (new Rectangle(109, 1321, 53, 53), new Vector2(-27, 0), new Vector2(0, -15), 100, MiddleSideSpecialRight2) },
            { MiddleSideSpecialRight2, (new Rectangle(168, 1321, 47, 53), new Vector2(-1, 0), new Vector2(0, -15), 100, MiddleSideSpecialRight3) },
            { MiddleSideSpecialRight3, (new Rectangle(221, 1321, 46, 53), new Vector2(-18, 0), new Vector2(0, -15), 100, MiddleSideSpecialRight4) },
            { MiddleSideSpecialRight4, (new Rectangle(275, 1321, 27, 53), new Vector2(-4, 0), new Vector2(0, -15), 100, MiddleSideSpecialRight5) },
            { MiddleSideSpecialRight5, (new Rectangle(307, 1321, 28, 53), new Vector2(-5, 0), new Vector2(0, -15), 100, MiddleSideSpecialRight6) },
            { MiddleSideSpecialRight6, (new Rectangle(340, 1321, 39, 53), new Vector2(-11, 0), new Vector2(0, -15), 100, LastSideSpecialRight) },
            { LastSideSpecialRight, (new Rectangle(384, 1321, 28, 53), new Vector2(0, 0), new Vector2(0, -15), 100, LastSideSpecialRight2) },
            { LastSideSpecialRight2, (new Rectangle(418, 1321, 26, 53), new Vector2(0, 0), new Vector2(0, -15), 100, StartSideSpecialRight) },

            //Spin Attack + power up
            { StartDownSpecialRight, (new Rectangle(11, 1392, 28, 39), new Vector2(0,0), new Vector2(0, -1), 100, StartDownSpecialRight2) },
            { StartDownSpecialRight2, (new Rectangle(45, 1392, 26, 39), new Vector2(0,0), new Vector2(0, -1), 100, StartDownSpecialRight3) },
            { StartDownSpecialRight3, (new Rectangle(77, 1392, 20, 39), new Vector2(3,0), new Vector2(0, -1), 100, StartDownSpecialRight4) },
            { StartDownSpecialRight4, (new Rectangle(103, 1392, 36, 39), new Vector2(0,0), new Vector2(0, -1), 100, StartDownSpecialRight5) },
            { StartDownSpecialRight5, (new Rectangle(145, 1392, 27, 39), new Vector2(8,0), new Vector2(0, -1), 100, StartDownSpecialRight6) },
            { StartDownSpecialRight6,(new Rectangle(179, 1392, 22, 39), new Vector2(10,0), new Vector2(0, -1), 100, StartDownSpecialRight7) },
            { StartDownSpecialRight7,(new Rectangle(206, 1392, 37, 39), new Vector2(-2,0), new Vector2(0, -1), 100, EndDownSpecialRight) },

            //Power up
            { EndDownSpecialRight, (new Rectangle(248, 1392, 31, 39), new Vector2(-1, 0), new Vector2(0, -1), 100, EndDownSpecialRight2) },
            { EndDownSpecialRight2, (new Rectangle(284, 1392, 25, 39), new Vector2(2, 0), new Vector2(0, -1), 100, EndDownSpecialRight3) },
            { EndDownSpecialRight3, (new Rectangle(314, 1392, 37, 39), new Vector2(-4, 0), new Vector2(0, -1), 100, EndDownSpecialRight4) },
            { EndDownSpecialRight4, (new Rectangle(356, 1392, 38, 39), new Vector2(-5, 0), new Vector2(0, -1), 100, EndDownSpecialRight5) },
            { EndDownSpecialRight5, (new Rectangle(399, 1392, 38, 39), new Vector2(-5, 0), new Vector2(0, -1), 100, EndDownSpecialRight6) },
            { EndDownSpecialRight6, (new Rectangle(443, 1392, 31, 39), new Vector2(-1, 0), new Vector2(0, -1), 100, EndDownSpecialRight7) },
            { EndDownSpecialRight7, (new Rectangle(479, 1392, 26, 39), new Vector2(0, 0), new Vector2(0, -1), 100, StartDownSpecialRight) },

            //Punch Attack
            { SpecialPunchAttackRight, (new Rectangle(10, 1445, 35, 36), new Vector2(-5, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight2) },
            { SpecialPunchAttackRight2, (new Rectangle(51, 1445, 36, 36), new Vector2(-6, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight3) },
            { SpecialPunchAttackRight3, (new Rectangle(93, 1445, 36, 36), new Vector2(-7, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight4) },
            { SpecialPunchAttackRight4, (new Rectangle(135, 1445, 36, 36), new Vector2(-6, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight5) },
            { SpecialPunchAttackRight5, (new Rectangle(177, 1445, 31, 36), new Vector2(-5, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight6) },
            { SpecialPunchAttackRight6, (new Rectangle(214, 1445, 39, 36), new Vector2(-4, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight7) },
            { SpecialPunchAttackRight7, (new Rectangle(257, 1445, 32, 36), new Vector2(2, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight8) },
            { SpecialPunchAttackRight8, (new Rectangle(294, 1445, 22, 36), new Vector2(2, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight9) },
            { SpecialPunchAttackRight9, (new Rectangle(321, 1445, 28, 36), new Vector2(0, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight10) },
            { SpecialPunchAttackRight10, (new Rectangle(355, 1445, 26, 36), new Vector2(0, 0), new Vector2(0, 2), 100, SpecialPunchAttackRight) },

        //Other Moves
            //Rolling
            { RollingRight, (new Rectangle(16, 1592, 19, 29), new Vector2(0,0), new Vector2(0, 0), 100, RollingRight2) },
            { RollingRight2, (new Rectangle(41, 1592, 23, 29), new Vector2(-3,0), new Vector2(0, 0), 100, RollingRight3) },
            { RollingRight3, (new Rectangle(70, 1592, 29, 29), new Vector2(-4,0), new Vector2(0, 0), 100, RollingRight4) },
            { RollingRight4, (new Rectangle(105, 1592, 28, 29), new Vector2(-4,0), new Vector2(0, 0), 100, RollingRight5) },
            { RollingRight5, (new Rectangle(16, 1631, 19, 29), new Vector2(0,0), new Vector2(0, 0), 100, RollingRight6) },
            { RollingRight6,(new Rectangle(41, 1631, 23, 29), new Vector2(0,0), new Vector2(0, 0), 100, RollingRight7) },
            { RollingRight7,(new Rectangle(70, 1631, 29, 29), new Vector2(-3,0), new Vector2(0, 0), 100, RollingRight8) },
            { RollingRight8,(new Rectangle(105, 1631, 28, 29), new Vector2(-3,0), new Vector2(0, 0), 100 , RollingRight) },

            //Guard
            { GuardRight, (new Rectangle(154, 1612, 26, 34), new Vector2(0,0), new Vector2(0, 4), 100, GuardRight2) },
            { GuardRight2,(new Rectangle(186, 1612, 28, 34), new Vector2(-1,0), new Vector2(0, 4), 100, GuardRight3) },
            { GuardRight3,(new Rectangle(220, 1612, 24, 34), new Vector2(0,0), new Vector2(0, 4), 100, GuardRight4) },
            { GuardRight4,(new Rectangle(249, 1612, 27, 34), new Vector2(0,0), new Vector2(0, 4), 100 , GuardRight) },

            //Free Fall
            { FreeFallRight, (new Rectangle(290, 1592, 34, 38), new Vector2(0,0), new Vector2(0, 0), 100, FreeFallRight2) },
            { FreeFallRight2,(new Rectangle(329, 1592, 34, 38), new Vector2(0,0), new Vector2(0, 0), 100, FreeFallRight3) },
            { FreeFallRight3,(new Rectangle(368, 1592, 39, 38), new Vector2(0,0), new Vector2(0, 0), 100, FreeFallRight4) },
            { FreeFallRight4,(new Rectangle(412, 1592, 36, 38), new Vector2(0,0), new Vector2(0, 0), 100, FreeFallRight5) },
            { FreeFallRight5,(new Rectangle(291, 1636, 34, 38), new Vector2(0,0), new Vector2(0, 0), 100, FreeFallRight6) },
            { FreeFallRight6,(new Rectangle(330, 1636, 34, 38), new Vector2(0,0), new Vector2(0, 0), 100, FreeFallRight7) },
            { FreeFallRight7,(new Rectangle(369, 1636, 39, 38), new Vector2(0,0), new Vector2(0, 0), 100 , FreeFallRight8) },
            { FreeFallRight8,(new Rectangle(412, 1636, 36, 38), new Vector2(0,0), new Vector2(0, 0), 100 , FreeFallRight) }
            
    };

}