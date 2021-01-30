using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game i;

    public event Action ItemCollected;

    public Player player;
    public UIItemManager itemManager;
    public InnerToughts innerToughts;
    
    private Game()
    {
        i = this;
    }

    public static void NotifyItemCollected(NarrativeItem item)
    {
      
    }
    
    
}
