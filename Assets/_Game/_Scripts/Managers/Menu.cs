using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartGameTwoPlayers()
        => Scenes.OpenGameTwoPlayers();

    public void StartGameAI()
        => Scenes.OpenGameAI();
}
