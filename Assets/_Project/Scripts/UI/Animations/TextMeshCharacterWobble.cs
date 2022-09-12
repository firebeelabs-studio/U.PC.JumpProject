using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextMeshCharacterWobble : MonoBehaviour
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

        for (int i = 0; i < _textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = _textMesh.textInfo.characterInfo[i];
            int index = c.vertexIndex;
                
            Vector3 offset = Wobble(Time.time + i);
            _verticies[index] += offset;
            _verticies[index + 1] += offset;
            _verticies[index + 2] += offset;
            _verticies[index + 3] += offset;
        }
        _mesh.vertices = _verticies;
        _textMesh.canvasRenderer.SetMesh(_mesh);
    }

    private Vector2 Wobble(float time)
    {
        return new Vector2(_wobbleScale * Mathf.Sin(time * 3.3f), _wobbleScale * Mathf.Cos(time * 2.5f));
    }
}
