using Models.Scriptables;
using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    [SerializeField]
    private CharacterScriptable[] characters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // fetch default characters for now. In the future, player should have a menu to create from scratch

    }

}
