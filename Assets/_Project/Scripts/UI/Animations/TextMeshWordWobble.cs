using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextMeshWordWobble : MonoBehaviour
{
    [SerializeField] private float _wobbleScale;

    TMP_Text _textMesh;
    Mesh _mesh;
    Vector3[] _verticies;

    List<int> _wordIndexes;
    List<int> _wordLenghts;

    void Start()
    {
        _textMesh = GetComponent<TMP_Text>();

        _wordIndexes = new List<int> { 0 };
        _wordLenghts = new List<int>();

        string s = _textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            _wordLenghts.Add(index - _wordIndexes[_wordIndexes.Count - 1]);
            _wordIndexes.Add(index + 1);
        }
        _wordLenghts.Add(s.Length - _wordIndexes[_wordIndexes.Count - 1]);
    }

    void Update()
    {
        _textMesh.ForceMeshUpdate();
        _mesh = _textMesh.mesh;
        _verticies = _mesh.vertices;

        for (int w = 0; w<_wordIndexes.Count; w++)
        {
            int wordIndex = _wordIndexes[w];
            Vector3 offset = Wobble(Time.time + w);

            for (int i = 0; i < _wordLenghts[w]; i++)
            {
                TMP_CharacterInfo c = _textMesh.textInfo.characterInfo[wordIndex + i];

                int index = c.vertexIndex;
                _verticies[index] += offset;
                _verticies[index + 1] += offset;
                _verticies[index + 2] += offset;
                _verticies[index + 3] += offset;
            }
        }
        _mesh.vertices = _verticies;
        _textMesh.canvasRenderer.SetMesh(_mesh);
    }

    private Vector2 Wobble(float time)
    {
        return new Vector2(_wobbleScale * Mathf.Sin(time * 3.3f), _wobbleScale * Mathf.Cos(time * 2.5f));
    }
}
