using UnityEngine;

public class BackgroundMovement : MonoBehaviour, IMovement
{
    private const float CITY_SPRITE_WIDTH = 2.87f;
    private const float BASE_SPRITE_WIDTH = 2.93f;

    [SerializeField] private GameObject[] _citySprites;
    [SerializeField] private GameObject[] _baseSprites;

    [SerializeField] private float citySpriteSpeed = -0.1f;
    [SerializeField] private float baseSpriteSpeed = -1f;

    private bool _isCityMoving = true;
    private bool _isBaseMoving = true;

    private bool _isFirstCitySprite = true;
    private bool _isFirstBaseSprite = true;

    void FixedUpdate()
    {
        if (_isCityMoving && _isBaseMoving)
        {
            MoveSprites(_citySprites, citySpriteSpeed);
            MoveSprites(_baseSprites, baseSpriteSpeed);
        }
        else if (_isBaseMoving)
        {
            MoveSprites(_baseSprites, baseSpriteSpeed);
        }

        CheckSpriteOffset(_citySprites, ref _isFirstCitySprite, CITY_SPRITE_WIDTH);
        CheckSpriteOffset(_baseSprites, ref _isFirstBaseSprite, BASE_SPRITE_WIDTH);
    }

    public void StartMoving()
    {
        _isCityMoving = true;
        _isBaseMoving = true;
    }

    public void StopMoving()
    {
        _isCityMoving = false;
        _isBaseMoving = false;
    }

    public void StartBaseMoving() => _isBaseMoving = true;

    private void MoveSprites(GameObject[] sprites, float spriteSpeed)
    {
        foreach (var sprite in sprites)
        {
            sprite.transform.position += new Vector3(spriteSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void CheckSpriteOffset(GameObject[] sprites, ref bool whichOfSprites, float spriteWidth)
    {
        if (whichOfSprites)
        {
            if (sprites[0].transform.position.x <= -spriteWidth)
            {
                SetSpriteOffset(sprites[0], ref whichOfSprites, spriteWidth);
            }
        }
        else
        {
            if (sprites[1].transform.position.x <= -spriteWidth)
            {
                SetSpriteOffset(sprites[1], ref whichOfSprites, spriteWidth);
            }
        }
    }

    private void SetSpriteOffset(GameObject sprite, ref bool whichOfSprites, float offset)
    {
        sprite.transform.position = new Vector3(offset, sprite.transform.position.y, sprite.transform.position.z);

        whichOfSprites = !whichOfSprites;
    }
}
