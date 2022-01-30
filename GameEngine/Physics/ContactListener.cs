using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using GameEngine.Components;

namespace GameEngine.Physics; 

internal class ContactListener : Box2D.NetStandard.Dynamics.World.Callbacks.ContactListener {
    
    public void BeginContact(in Contact contact) {
        if(contact.FixtureA.Body.UserData is null || contact.FixtureB.Body.UserData is null)
            return;
        (contact.FixtureA.Body.UserData as RigidBody).BeginCollision(contact.FixtureB.Body.UserData as RigidBody);
        (contact.FixtureB.Body.UserData as RigidBody).BeginCollision(contact.FixtureA.Body.UserData as RigidBody);
    }

    public void EndContact(in Contact contact) { }

    public void PreSolve(in Contact contact, in Manifold oldManifold) { }

    public void PostSolve(in Contact contact, in ContactImpulse impulse) { }
    
}
