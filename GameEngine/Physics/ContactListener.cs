using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using GameEngine.Components;

namespace GameEngine.Physics; 

internal class ContactListener : Box2D.NetStandard.Dynamics.World.Callbacks.ContactListener {
    
    public void BeginContact(in Contact contact) {
        if(contact.FixtureA.Body.UserData is null || contact.FixtureB.Body.UserData is null)
            return;
        if(contact.FixtureA.Body.UserData is Collider colliderA && contact.FixtureB.Body.UserData is Collider colliderB)
            HandleCollision(colliderA, colliderB);
        if(contact.FixtureA.Body.UserData is Trigger triggerA && contact.FixtureB.Body.UserData is Trigger triggerB)
            HandleTrigger(triggerA, triggerB);
    }
    
    public void EndContact(in Contact contact) { }
    
    public void PreSolve(in Contact contact, in Manifold oldManifold) { }
    
    public void PostSolve(in Contact contact, in ContactImpulse impulse) { }
    
    private void HandleCollision(Collider colliderA, Collider colliderB) {
        colliderA.BeginCollision(colliderB);
        colliderB.BeginCollision(colliderA);
    }
    
    private void HandleTrigger(Trigger triggerA, Trigger triggerB) {
        triggerA.BeginTrigger(triggerB);
        triggerB.BeginTrigger(triggerA);
    }
    
}
