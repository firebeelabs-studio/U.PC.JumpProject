using UnityEngine;

[RequireComponent(typeof(InsectPlayerFollower))]
public class PetTiltAnimation : MovementTiltAnimation
{
    private Vector2 _scale;
    private InsectPlayerFollower _pF;

    private void Awake()
    {
        _pF = GetComponent<InsectPlayerFollower>();
    }

    override protected void Start()
    {
        base.Start();
        _scale = transform.localScale;
    }

    override protected void Update()
    {
        base.Update();

        // flips sprite, depending on player position
        transform.localScale = new Vector3(_pF.IsGoingRight == true ? _scale.x : -_scale.x, _scale.y, 1);
    }
}
