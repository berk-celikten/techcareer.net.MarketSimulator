using UnityEngine;

public interface INPCState
{
    void Enter(NPC npc);
    void Update(NPC npc);
    void Exit(NPC npc);

}
