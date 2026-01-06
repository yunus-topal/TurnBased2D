using System;
using System.Collections.Generic;

namespace Models
{
    public enum StatusEffectType { Bleed, Poison, Stun, Sleep, None } // extend as needed

    // How stacking works for each effect
    public enum StackMode { None, Additive } // "None" = single instance (e.g., Stun). "Additive" = stack count (e.g., Poison).

    // When an effect should trigger
    [System.Flags]
    public enum TickTiming { None = 0, OnTurnStart = 1, OnTurnEnd = 2 }

    // Static rules per effect type (tuning knobs live here)
    public readonly struct StatusRule
    {
        public readonly StackMode StackMode;
        public readonly TickTiming TicksWhen;
        public readonly bool BlocksTurn;        // e.g., Stun/Sleep
        public readonly bool DecaysEachTurn;    // most effects decay by 1 each full turn

        public StatusRule(StackMode stackMode, TickTiming timing, bool blocksTurn, bool decaysEachTurn)
        {
            StackMode = stackMode;
            TicksWhen = timing;
            BlocksTurn = blocksTurn;
            DecaysEachTurn = decaysEachTurn;
        }
    }

    // Concrete runtime instance applied on a character
    [Serializable]
    public sealed class StatusInstance
    {
        public StatusEffectType Type;
        public int Stacks;     // 1 for non-stacking effects
        public int Duration;   // in turns, decays by 1 each tick cycle if enabled
        public int Potency;    // optional magnitude per stack (damage per stack, etc.)

        public StatusInstance(StatusEffectType type, int stacks, int duration, int potency)
        {
            Type = type;
            Stacks = Math.Max(1, stacks);
            Duration = Math.Max(1, duration);
            Potency = potency;
        }
    }

    public static class StatusLibrary
    {
        // Central rules table. Adjust to your design.
        public static readonly Dictionary<StatusEffectType, StatusRule> Rules = new()
        {
            { StatusEffectType.Poison, new StatusRule(StackMode.Additive, TickTiming.OnTurnStart, blocksTurn:false, decaysEachTurn:true) },
            { StatusEffectType.Bleed,  new StatusRule(StackMode.Additive, TickTiming.OnTurnEnd,   blocksTurn:false, decaysEachTurn:true) },
            { StatusEffectType.Stun,   new StatusRule(StackMode.None,     TickTiming.None,        blocksTurn:true,  decaysEachTurn:true) },
            { StatusEffectType.Sleep,  new StatusRule(StackMode.None,     TickTiming.OnTurnStart, blocksTurn:true,  decaysEachTurn:true) },
        };
    }
}
