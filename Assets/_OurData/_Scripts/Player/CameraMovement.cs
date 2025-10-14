using UnityEngine;

public class CameraMovement : MyBehaviour
{
    public CameraCtrl cameraCtrl;
    public float speed = 27f;
    public bool speedShift = false;
    public float minY = 4f;
    public float maxY = 70f;
    public Vector3 camRotation = new Vector3(0, 0, 0);
    public Vector3 camView = new Vector3(45f, 0, 0);
    public Vector3 cameraMovement = new Vector3(0, 0, 0);

    protected override void Update()
    {
        base.Update();
        this.Moving();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCameraCtrl();
    }

    protected virtual void LoadCameraCtrl()
    {
        if (this.cameraCtrl != null) return;
        this.cameraCtrl = GetComponent<CameraCtrl>();
        Debug.Log(transform.name + ": LoadCameraCtrl", gameObject);
    }

    protected virtual void Moving()
    { 
        float speed = this.speed;
        if (this.speedShift) speed += this.speed * 2;

        Vector3 movement = this.cameraMovement;
        movement.x *= speed;
        movement.z *= speed;
        movement.y *= speed * 7;

        transform.Translate(movement * Time.deltaTime);
        Vector3 newPos = transform.position;

        if (newPos.y < this.minY) newPos.y = this.minY;
        if (newPos.y > this.maxY) newPos.y = this.maxY; 
        transform.position = newPos;

        transform.Rotate(this.camRotation);

    }
}
