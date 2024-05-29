using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject {
    private string name;
    private int health;
    private int attack;
    private int defense;
    private int speed;
    private int level;
    private int experience;

    public string Name => name;

    public int Health => health;

    public int Attack => attack;

    public int Defense => defense;

    public int Speed => speed;

    public int Level => level;

    public int Experience => experience;
}