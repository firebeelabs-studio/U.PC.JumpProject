using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextMeshWobble : MonoBehaviour
{
    [SerializeField] private float _wobbleScale;

    TMP_Text _textMesh;
    Mesh _mesh;
    Vector3[] _verticies;

    void Start()
    {
        _textMesh = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _textMesh.ForceMeshUpdate();
        _mesh = _textMesh.mesh;
        _verticies = _mesh.vertices;
        
        for (int i=0; i < _verticies.Length; i++)
        {
            Vector3 offset = Wobble(Time.time + i);
            _verticies[i] = _verticies[i] + offset;
        }
        _mesh.vertices = _verticies;
        _textMesh.canvasRenderer.SetMesh(_mesh);
    }

    private Vector2 Wobble(float time)
    {
        return new Vector2(_wobbleScale * Mathf.Sin(time * 3.3f), _wobbleScale * Mathf.Cos(time * 2.5f));
    }
}
