using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
public class Character : ScriptableObject {
    [SerializeField] private string charName;
    [SerializeField] private Sprite charSprite;
    [SerializeField] private GameObject charPrefab;

    public string CharName => charName;
    public Sprite CharSprite => charSprite;
    public GameObject CharPrefab => charPrefab;
}
