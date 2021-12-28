using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ChunkData : UpdatableData
{
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public bool flatShading;
}
