using UnityEngine;

public class CameraScript : MonoBehaviour {
    Camera _camera;

    void Awake() {
        _camera = GetComponent<Camera>();
    }

    public void SetPosition(Vector3 pos, float size=5.0f) {
        transform.localPosition = pos;
        if(_camera != null) { _camera.orthographicSize = size; }
    }

    public void SetPosition(GroundCameraData pos) {
        SetPosition(pos:pos.Position, size:pos.Size);
    }
}
