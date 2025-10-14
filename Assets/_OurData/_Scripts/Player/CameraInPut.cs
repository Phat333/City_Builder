using UnityEngine;

public class CameraInPut : MyBehaviour
{
    public CameraCtrl cameraCtrl;
    public bool isMouseRotating = false;
    public Vector2 mouseScroll = new Vector2();
    public Vector3 mouseReference = new Vector3();
    public Vector3 mouseRotation = new Vector3();


    protected override void Update()
    {
        this.InputHandler();
        this.MouseRotation();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCameraCtrl();
    }

    protected virtual void LoadCameraCtrl()
    {
        if (this.cameraCtrl != null) return;
        this.cameraCtrl = transform.GetComponent<CameraCtrl>();
        Debug.Log("LoadCameraCtrl", gameObject);
    }

    protected virtual void InputHandler()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.mouseScrollDelta.y * -1;
        bool leftShift = Input.GetKey(KeyCode.LeftShift);
        this.isMouseRotating = Input.GetMouseButton(1);
        if(Input.GetMouseButtonDown(1))
        {
            this.mouseReference = Input.mousePosition;
        }
        this.cameraCtrl.cameraMovement.cameraMovement.x = x;
        this.cameraCtrl.cameraMovement.cameraMovement.z = z;
        this.cameraCtrl.cameraMovement.cameraMovement.y = y;
        this.cameraCtrl.cameraMovement.speedShift = leftShift;

    }

    protected virtual void MouseRotation()
    {
        this.isMouseRotating = Input.GetMouseButton(1);
        if (Input.GetMouseButtonDown(1)) this.mouseReference = Input.mousePosition;
        if (this.isMouseRotating)
        {
            this.mouseRotation = (Input.mousePosition - this.mouseReference);
            this.mouseRotation.y = -(this.mouseRotation.x + this.mouseRotation.y);
            this.mouseReference = Input.mousePosition;

        }
        else
        {
            this.mouseRotation = Vector3.zero;
        }
        this.cameraCtrl.cameraMovement.camRotation.y = this.mouseRotation.x;
    }

}
