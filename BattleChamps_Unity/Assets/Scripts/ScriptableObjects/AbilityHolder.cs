using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public int abilityHolder;

    public Ability_Base ability;

    private float cooldownTime;
    public float activeTime;

    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    public AbilityState state = AbilityState.ready;

    public void Activate()
    {
        if (state == AbilityState.ready)
        {
            ability.ActivateAbility(gameObject.transform.root.gameObject);
            state = AbilityState.active;
            activeTime = ability.activeTime;
        }
        else
        { return; }
    }

    void Update()
    {
        switch (state)
        {
            //case AbilityState.ready:
               // if (true)
               // { 
               //     ability.ActivateAbility(gameObject.transform.root.gameObject);
              //      state = AbilityState.active;
              //      activeTime = ability.activeTime;
              //  }
               // break;

            case AbilityState.active:
                if (activeTime > 0)
                { activeTime -= Time.deltaTime; }
                else
                {
                    state = AbilityState.cooldown;
                    cooldownTime = ability.cooldownTime;
                }
                break;

            case AbilityState.cooldown:
                if (cooldownTime > 0)
                { cooldownTime -= Time.deltaTime; }
                else { state = AbilityState.ready; }
                break;
        }
    }
}
