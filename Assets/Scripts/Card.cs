using UnityEngine;
using System;

[Serializable]
public class Card 
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _sp;
    public string Id=>_id;
    public Sprite Sp=>_sp;
}

