using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game i;

    [SerializeField] private Player player;
    
    private Game()
    {
        i = this;
    }
}
