using System.Linq;
using System.Text;
using TarodevController;
using UnityEngine;

public class Recording 
{
    private readonly AnimationCurve _posXCurve = new();
    private readonly AnimationCurve _posYCurve = new();
    private readonly AnimationCurve _scaleYCurve = new();
    private readonly AnimationCurve _scaleXCurve = new();
    private readonly AnimationCurve _rotationCurve = new();
    public float Duration { get; private set; }
    private readonly Transform _target;
    private readonly PlayerAnimator _targetAnimator;
    private readonly SpriteRenderer _targetSpriteRenderer;
    public struct ReplayStepData
    {
        public ReplayStepData(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }
        public Vector3 Position;
        public Vector3 Scale;
        public Quaternion Rotation;
    }

    #region Used For Recording

    public Recording(Transform target, PlayerAnimator playerAnimator) 
    {
        _target = target;
        _targetAnimator = playerAnimator;
    }

    public void AddSnapshot(float elapsed) 
    {
        Duration = elapsed;
        Transform targetTransform = _targetAnimator.transform;
        Vector3 position = targetTransform.position;
        var localScale = targetTransform.localScale;
        Vector3 scale = new Vector3(localScale.x * _targetAnimator.LastMoveDirection, localScale.y, localScale.z);
        Quaternion rotation = targetTransform.rotation;

        UpdateCurve(_posXCurve, elapsed, position.x);
        UpdateCurve(_posYCurve, elapsed, position.y);
        UpdateCurve(_scaleYCurve, elapsed, scale.y);
        UpdateCurve(_scaleXCurve, elapsed, scale.x);
        UpdateCurve(_rotationCurve, elapsed, rotation.z);
        

        void UpdateCurve(AnimationCurve curve, float time, float val) 
        {
            var count = curve.length;
            var kf = new Keyframe(time, val);

            if (count > 1 &&
                Mathf.Approximately(curve.keys[count - 1].value, curve.keys[count - 2].value) &&
                Mathf.Approximately(val, curve.keys[count - 1].value)) 
            {
                curve.MoveKey(count - 1, kf);
            }
            else 
            {
                curve.AddKey(kf);
            }
        }
    }

    #endregion

    #region Used For Playback

    public ReplayStepData EvaluatePointToGetTransformData(float elapsed)
    {
        return new ReplayStepData(
            new Vector3(_posXCurve.Evaluate(elapsed), _posYCurve.Evaluate(elapsed), 0f),
            new Vector3(_scaleXCurve.Evaluate(elapsed), _scaleYCurve.Evaluate(elapsed), 1),
            Quaternion.Euler(0,0,_rotationCurve.Evaluate(elapsed)));
    }

    #endregion

    #region Saving and Loading

    public Recording(string data) 
    {
        _target = null;
        Deserialize(data);
        Duration = Mathf.Max(_posXCurve.keys.LastOrDefault().time, _posYCurve.keys.LastOrDefault().time);
    }

    private const char DATA_DELIMITER = '|';
    private const char CURVE_DELIMITER = '_';

    public string Serialize() 
    {
        var builder = new StringBuilder();

        StringifyPoints(_posXCurve);
        StringifyPoints(_posYCurve);
        StringifyPoints(_scaleXCurve);
        StringifyPoints(_scaleYCurve);
        StringifyPoints(_rotationCurve, false);

        void StringifyPoints(AnimationCurve curve, bool addDelimiter = true) 
        {
            for (var i = 0; i < curve.length; i++) 
            {
                var point = curve[i];
                builder.Append($"{point.time:F3}:{point.value:F3}");
                if (i != curve.length - 1) builder.Append(DATA_DELIMITER);
            }

            if (addDelimiter) builder.Append(CURVE_DELIMITER);
        }

        return builder.ToString();
    }

    private void Deserialize(string data) 
    {
        var components = data.Split(CURVE_DELIMITER);

        DeserializePoint(_posXCurve, components[0]);
        DeserializePoint(_posYCurve, components[1]);
        DeserializePoint(_scaleXCurve, components[2]);
        DeserializePoint(_scaleYCurve, components[3]);
        DeserializePoint(_rotationCurve, components[4]);

        void DeserializePoint(AnimationCurve curve, string d) 
        {
            var splitValues = d.Split(DATA_DELIMITER);
            foreach (var timeValPair in splitValues)
            {
                var s = timeValPair.Split(':');
                var kf = new Keyframe(float.Parse(s[0]), float.Parse(s[1]));
                curve.AddKey(kf);
            }
        }
    }

    #endregion
}