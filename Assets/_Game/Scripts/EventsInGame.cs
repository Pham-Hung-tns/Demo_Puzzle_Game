using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsInGame : MonoBehaviour
{
    public static Action<Sprite, JewelsController, string> PlaceGrid;
    public static Action CreateJewels;
}