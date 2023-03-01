using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCaster2DGenerator : MonoBehaviour
{
    public string CommonName = "ShadowCaster2D";
    public bool SelfShadows;
    [HideInInspector] public bool ClearShadowCastersOnly = true;

    private CompositeCollider2D _compositeCollider2D;

    public void Generate()
    {
        Clear();
        if (!_compositeCollider2D) _compositeCollider2D = GetComponent<CompositeCollider2D>();
        for (int i = 0; i < _compositeCollider2D.pathCount; i++) CreateShadowCaster(i, CreateShapePath(i));
    }

    public void Clear()
    {
        List<Transform> childTransformList = transform.Cast<Transform>().ToList();
        foreach (Transform childTransform in childTransformList)
            if (ClearShadowCastersOnly && childTransform.TryGetComponent<ShadowCaster2D>(out _) || !ClearShadowCastersOnly)
                DestroyImmediate(childTransform.gameObject);
    }

    private Vector3[] CreateShapePath(int i)
    {
        Vector2[] pathPoints = new Vector2[_compositeCollider2D.GetPathPointCount(i)];
        _compositeCollider2D.GetPath(i, pathPoints);
        Vector3[] shapePath = new Vector3[pathPoints.Length];
        for (int j = 0; j < pathPoints.Length; j++) shapePath[j] = pathPoints[j];
        return shapePath;
    }

    private void CreateShadowCaster(int i, Vector3[] shapePath)
    {
        GameObject shadowCasterGameObject = new($"{CommonName.Trim()} ({i})");
        shadowCasterGameObject.transform.parent = transform;
        shadowCasterGameObject.transform.position = transform.TransformPoint(Vector3.zero);
        ShadowCaster2D shadowCaster = shadowCasterGameObject.AddComponent<ShadowCaster2D>();
        shadowCaster.selfShadows = SelfShadows;
        ShadowCaster2DReflection shadowCasterReflection = new(shadowCaster)
        {
            ShapePath = shapePath,
            ShapePathHash = Random.Range(int.MinValue, int.MaxValue),
            Mesh = new Mesh()
        };
        shadowCasterReflection.GenerateShadowMesh();
    }
}

[CustomEditor(typeof(ShadowCaster2DGenerator))]
public class ShadowCaster2DGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShadowCaster2DGenerator shadowCaster2DGenerator = target as ShadowCaster2DGenerator;
        DrawDefaultInspector();
        GUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate")) shadowCaster2DGenerator.Generate();
        if (GUILayout.Button("Clear")) shadowCaster2DGenerator.Clear();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10f);
        shadowCaster2DGenerator.ClearShadowCastersOnly 
            = GUILayout.Toggle(shadowCaster2DGenerator.ClearShadowCastersOnly, "Clear ShadowCaster2D Childs Only");
    }
}

public class ShadowCaster2DReflection
{
    private readonly ShadowCaster2D _shadowCaster2D;
    //private readonly FieldInfo _applyToSortingLayers;
    private readonly FieldInfo _meshField;
    private readonly FieldInfo _shapePathField;
    private readonly FieldInfo _shapePathHashField;
    private readonly MethodInfo _generateShadowMeshMethod;

    //public int[] ApplyToSortingLayers 
    //{
    //    get => _applyToSortingLayers.GetValue(_shadowCaster2D) as int[];
    //    set => _applyToSortingLayers.SetValue(_shadowCaster2D, value);
    //}

    public Mesh Mesh
    {
        get => _meshField.GetValue(_shadowCaster2D) as Mesh;
        set => _meshField.SetValue(_shadowCaster2D, value);
    }

    public Vector3[] ShapePath
    {
        get => _shapePathField.GetValue(_shadowCaster2D) as Vector3[];
        set => _shapePathField.SetValue(_shadowCaster2D, value);
    }

    public int ShapePathHash
    {
        get => (int)_shapePathHashField.GetValue(_shadowCaster2D);
        set => _shapePathHashField.SetValue(_shadowCaster2D, value);
    }

    public ShadowCaster2DReflection(ShadowCaster2D shadowCaster2D)
    {
        _shadowCaster2D = shadowCaster2D;
        //_applyToSortingLayers = typeof(ShadowCaster2D).GetField("m_ApplyToSortingLayers", BindingFlags.NonPublic | BindingFlags.Instance);
        _meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
        _shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
        _shapePathHashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
        _generateShadowMeshMethod = typeof(ShadowCaster2D).Assembly.GetType("UnityEngine.Rendering.Universal.ShadowUtility")
            .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);
    }

    public void GenerateShadowMesh() => _generateShadowMeshMethod.Invoke(_shadowCaster2D, new object[] { Mesh, ShapePath });
}
