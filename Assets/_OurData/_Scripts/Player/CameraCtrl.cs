using UnityEngine;

public class CameraCtrl : MyBehaviour
{
    public static CameraCtrl instance;
    [Header("Camera Ctrl")]
    public Camera _camera;
    public CameraMovement cameraMovement;


    protected override void Awake()
    {
        base.Awake();
        if (CameraCtrl.instance != null) Debug.LogError("Only 1 CameraCtrl allow");
        CameraCtrl.instance = this;
        {
            
        }
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCamera();
        this.LoadCameraMovement();
    }

    protected virtual void LoadCameraMovement()
    {
        if (this.cameraMovement != null) return;
        this.cameraMovement = GetComponent<CameraMovement>();
        Debug.Log(transform.name + ": LoadCameraMovement", gameObject);
    }

    protected virtual void LoadCamera()
    {
        if (this._camera != null) return;
        this._camera =transform.Find("Camera").GetComponent<Camera>();
        this._camera.transform.rotation = Quaternion.Euler(this.cameraMovement.camView.x, this.cameraMovement.camView.y, this.cameraMovement.camView.z);

        Debug.Log(transform.name+": LoadCamera", gameObject);
    }
}
