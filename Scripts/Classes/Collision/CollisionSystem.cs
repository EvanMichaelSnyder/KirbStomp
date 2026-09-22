using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Classes.Platforms;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using KirbStomp;
using KirbStomp.Scripts.Projectiles;
// using static HitboxTypeEnum;

public class CollisionSystem
{
    private readonly Dictionary<HitboxTypeEnum, List<Carrier>> _carrierGroups = new()
    {
        { HitboxTypeEnum.Body, new List<Carrier>() },
        { HitboxTypeEnum.Attack, new List<Carrier>() },
        { HitboxTypeEnum.Platform, new List<Carrier>() },
        { HitboxTypeEnum.Item, new List<Carrier>() },
        { HitboxTypeEnum.Boundary, new List<Carrier>() }
    };

    private List<CollisionObject> _allObjects = new List<CollisionObject>();

    public void RegisterObject(CollisionObject obj)
    {
        if (!_allObjects.Contains(obj))
        {
            _allObjects.Add(obj);
            foreach (var carrier in obj.Carriers)
            {
                var type = GetCarrierHitboxType(carrier);
                if (!_carrierGroups[type].Contains(carrier))
                {
                    _carrierGroups[type].Add(carrier);
                }
            }
        }
        //Debug.WriteLine(_carrierGroups[HitboxTypeEnum.Boundary].Count);
    }

    public void RemoveObject(CollisionObject obj)
    {
        if (_allObjects.Remove(obj))
        {
            foreach (var carrier in obj.Carriers)
            {
                var type = GetCarrierHitboxType(carrier);
                _carrierGroups[type].Remove(carrier);
            }
        }
    }

    public void CheckCollisionPair(HitboxTypeEnum typeA, HitboxTypeEnum typeB, GameTime gameTime)
    {
        var carriersA = _carrierGroups[typeA];
        var carriersB = _carrierGroups[typeB];

        foreach (var carrierA in carriersA)
        {

            foreach (var carrierB in carriersB)
            {

                //Debug.WriteLine("Collision Occured");
                if (carrierA.Parent == carrierB.Parent) continue;
                if (carrierA.Parent.GetType().BaseType == typeof(AProjectile) && carrierB.Parent.GetType().BaseType == typeof(AProjectile))
                {
                    AProjectile aProj = (AProjectile)carrierA.Parent;
                    AProjectile bProj = (AProjectile)carrierB.Parent;
                    if (aProj.GetSpawningCharacter() == bProj.GetSpawningCharacter()) continue;
                }

                bool skip = false;
                //carve out for projectiles
                if(carrierB.Parent.GetType().BaseType == typeof(AProjectile))
                {
                    AProjectile Bcomp = (AProjectile)carrierB.Parent;
                    if(carrierA.Parent.GetType() == typeof(Character))
                    {
                        Character Acomp = (Character)carrierA.Parent;
                        if (Acomp == Bcomp.GetSpawningCharacter())
                        {
                            skip = true;
                        }
                    }
                }

                if (skip) continue;
                if (carrierA.IsDisabled | carrierB.IsDisabled) continue;
                
                if (CheckCollisionApprox(carrierA.HitboxManager, carrierB.HitboxManager))
                {
                    //Debug.WriteLine("Approx is Collide");
                    var intersection = GetDetailedIntersection(
                        carrierA.HitboxManager,
                        carrierB.HitboxManager
                    );

                    if (intersection != Rectangle.Empty)
                    {
                        HandleCollision(carrierA, carrierB, intersection, gameTime);
                    }
                }
            }
        }
    }

    public void CheckCollisionPairGround(HitboxTypeEnum typeA, HitboxTypeEnum typeB, GameTime gameTime)
    {
        var carriersA = _carrierGroups[typeA];
        var carriersB = _carrierGroups[typeB];

        foreach (var carrierA in carriersA)
        {
            foreach (var carrierB in carriersB)
            {
                //Debug.WriteLine("Collision Occured");
                if (carrierA.Parent == carrierB.Parent) continue;

                if (carrierA.IsDisabled | carrierB.IsDisabled) continue;

                if (CheckCollisionApprox(carrierA.HitboxManager, carrierB.HitboxManager))
                {
                    //Debug.WriteLine("Approx is Collide");
                    var intersection = GetDetailedIntersection(
                        carrierA.HitboxManager,
                        carrierB.HitboxManager
                    );

                    if (intersection != Rectangle.Empty)
                    {
                        HandleCollision(carrierA, carrierB, intersection, gameTime);
                    }
                }
            }
        }
    }

    private HitboxTypeEnum GetCarrierHitboxType(Carrier carrier)
    {
        return carrier switch
        {
            BodyCarrier _ => HitboxTypeEnum.Body,
            AttackCarrier _ => HitboxTypeEnum.Attack,
            ItemCarrier _ => HitboxTypeEnum.Item,
            PlatformCarrier _ => HitboxTypeEnum.Platform,
            BoundaryCarrier _ => HitboxTypeEnum.Boundary,
            _ => HitboxTypeEnum.Body
        };
    }

    private Rectangle GetDetailedIntersection(HitboxManager a, HitboxManager b)
    {
        Rectangle intersection = Rectangle.Empty;
        foreach (var rectA in a.getRectangles())
        {
            foreach (var rectB in b.getRectangles())
            {
                Rectangle currentIntersection = Rectangle.Intersect(rectA, rectB);
                if (!currentIntersection.IsEmpty)
                {
                    intersection = intersection.IsEmpty ? currentIntersection : Rectangle.Union(intersection, currentIntersection);
                }
            }
        }
        return intersection;
    }


    private bool CheckCollisionApprox(HitboxManager a, HitboxManager b)
    {
        return a.GetApproximation().Intersects(b.GetApproximation());
    }
    private bool CheckCollisionApproxForGround(HitboxManager a, HitboxManager b)
    {
        Rectangle approx = a.GetApproximation();
        approx.Y = approx.Y + (1 * (approx.Height / 2));
        approx.Height += approx.Height / 2;
        return approx.Intersects(b.GetApproximation());
    }

    private void HandleCollision(Carrier a, Carrier b, Rectangle intersect)
    {
        var contextA = new CollisionContext(
            a.Parent,
            b.Parent,
            GetCarrierHitboxType(a),
            GetCarrierHitboxType(b),
            intersect
        );

        var contextB = contextA.SwapPerspective();

        /*
        Debug.Assert(a.Parent != null, "Carrier A has no Parent");
        Debug.Assert(b.Parent != null, "Carrier B has no Parent");

        Debug.WriteLine(a.Parent.ToString());
        Debug.WriteLine(b.Parent.ToString());
        Debug.WriteLine(contextA.ToString());
        Debug.WriteLine(contextB.ToString());
        */
        a.Parent.HandleCollision(contextA);
        b.Parent.HandleCollision(contextB);
        //Debug.WriteLine("COLLISION HERE___________________________________________________");

    }

    private void HandleCollision(Carrier a, Carrier b, Rectangle intersect, GameTime gameTime)
    {
        var contextA = new CollisionContext(
            a.Parent,
            b.Parent,
            GetCarrierHitboxType(a),
            GetCarrierHitboxType(b),
            intersect,
            gameTime
        );

        var contextB = contextA.SwapPerspective();

        /*
        Debug.Assert(a.Parent != null, "Carrier A has no Parent");
        Debug.Assert(b.Parent != null, "Carrier B has no Parent");

        Debug.WriteLine(a.Parent.ToString());
        Debug.WriteLine(b.Parent.ToString());
        Debug.WriteLine(contextA.ToString());
        Debug.WriteLine(contextB.ToString());
        */
        a.Parent.HandleCollision(contextA);
        b.Parent.HandleCollision(contextB);
        //Debug.WriteLine("COLLISION HERE___________________________________________________");

    }
}