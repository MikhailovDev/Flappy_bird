using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteAtlasScript : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _spriteAtlas;
    [SerializeField] private string _spriteName;

    void Start()
    {
        GetComponent<Image>().sprite = _spriteAtlas.GetSprite(_spriteName);
    }
}
