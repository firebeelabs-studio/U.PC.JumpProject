using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//IMPORTANT NOTE: There are only few cases when using this script is eligible.
//If you have to use this probably there is something wrong. Think twice.
//It was made to overcome UI mask limitations (USE IT SMART ON UI IT WILL CAUSE ADDITIONAL CANVAS DRAWS).
public class HoldChildPositions : MonoBehaviour
{
    private readonly List<Vector3> _startingPositions = new();

    [SerializeField] private List<GameObject> _children;
    private bool _startHolding;

    private void Start()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            _startingPositions.Add(_children[i].transform.position);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_startHolding) return;
        
        for (int i = 0; i < _children.Count; i++)
        {
            _children[i].transform.position = _startingPositions[i];
        }
    }
    
    //used to overcome fookin panel animation
    public IEnumerator StartHolding()
    {
        yield return new WaitForSeconds(0.5f);
        _startHolding = true;
    }
    
    //used to overcome fookin panel animation
    public void StopHolding()
    {
        _startHolding = false;
    }
}
