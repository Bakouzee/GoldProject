using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioController
{
    public enum MenuAudioTracks
    {
        S_Victory,
        S_Defeat
    }

    public enum PlayerAudioTracks
    {
        P_Footstep,
        P_BigFootstep,
        P_Attack,
        P_Death,
        P_Kill,
        P_Transformation,
        P_Detransformation,
        P_Hurt,
        P_Damaged
    }

    public enum WindowAudioTracks
    {
        W_Open,
        W_Close,
        W_Break
    }

    public enum VentAudioTracks
    {
        V_Use,
        V_Open,
        V_Close,
    }

    public enum EnemyAudioTracks
    {
        E_Entrance,
        E_Fear,
        E_Terrified,
        E_Run,
        E_Attack,
        E_Move,
        E_Trigger
    }

    public enum AmbianceAudioTracks
    {
        Light_On,
        Light_Off,
        Door_Open,
        Door_Close,
        Blood_Drink,
        Blood_PickUp,
        Garlic_Use,
        Garlic_Dispawn,
        Thunder,
        Cocorico,
    }

    public enum MusicAudioTracks
    {
        M_Menu,
        M_Game,
        M_DAY,
        M_NIGHT
        // anything else?
    }

    public enum ScaryAudioTracks
    {
        Sc_StatueWalk,
        Sc_Puppet,
        Sc_Ghost,
        Sc_Mirror,
        Sc_Painting
    }

    public enum MapAudioTracks
    {
        Map_Open,
        Map_Close,
        Map_Switch
    }
}