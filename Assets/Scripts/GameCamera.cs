using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameCamera : MonoBehaviour {
    [SerializeField]
	private float minZoom = 30;
    [SerializeField]
	private float maxZoom = 90;
    [SerializeField]
	private float zoom = 30;
    [SerializeField]
    private float scrollSpeed = 50;
    [SerializeField]
    private float zoomInSwitchCount = 0;
    [SerializeField]
    private float zoomOutSwitchCount = 0;
    [SerializeField]
    private int zoomSwitchMax = 20;
    [SerializeField]
    private float zoomSwitchResetSpeed = 0.05f;
    [SerializeField]
    private float panSpeed = 0.3f;
    [SerializeField]
    private float panBuffer = 10;
    [SerializeField]
	private CameraZoomType cameraZoomType;

    public float MinZoom {
		get { return minZoom; }
    }
    public float MaxZoom {
        get { return maxZoom; }
    }
    public float Zoom {
        get { return zoom; }
        private set { zoom = value; }
    }

    public float ScrollSpeed {
        get { return scrollSpeed; }
    }

    public float PanSpeed {
        get { return panSpeed; }
    }

    public float PanBuffer {
        get { return panBuffer; }
    }

    public float ZoomInSwitchCount {
        get { return zoomInSwitchCount; }
        private set {zoomInSwitchCount = value; }
    }

    public float ZoomOutSwitchCount {
        get { return zoomOutSwitchCount; }
        private set {zoomOutSwitchCount = value; }
    }

    public float ZoomSwitchMax {
        get { return zoomSwitchMax; }
    }

    public float ZoomSwitchResetSpeed {
        get { return zoomSwitchResetSpeed; }
    }

    public CameraZoomType MyCameraZoomType {
        get { return cameraZoomType; }
        set { cameraZoomType = value; }
    }

    void Start () {
        Camera.main.fieldOfView = Zoom;
	}

	void Update () {
        handleZooming();
        handlePanning();
        handleTownSelection();
	}

    private void handleZooming() {
        float scrollInput = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
        if (scrollInput != 0) {
            Zoom = Mathf.Clamp(Zoom + ScrollSpeed * scrollInput, MinZoom, MaxZoom);
            Camera.main.fieldOfView = Zoom;

            if (scrollInput < 0 && Zoom == MinZoom) {
                ZoomInSwitchCount++;
                if (ZoomInSwitchCount >= ZoomSwitchMax) {
                    switchZoom(true);
                }
            } else if (scrollInput > 0 && Zoom == MaxZoom) {
                ZoomOutSwitchCount++;
                if (ZoomOutSwitchCount >= ZoomSwitchMax) {
                    switchZoom(false);
                }
            }
        } else {
            ZoomInSwitchCount = Mathf.Max(ZoomInSwitchCount - ZoomSwitchResetSpeed, 0);
            ZoomOutSwitchCount = Mathf.Max(ZoomOutSwitchCount - ZoomSwitchResetSpeed, 0);
        }
    }

    private void switchZoom(Boolean zoomIn) {
        if (zoomIn) {
            MyCameraZoomType -= 1;
        } else {
            MyCameraZoomType += 1;
        }
        if (MyCameraZoomType < 0) {
            MyCameraZoomType = 0;
        }
        if (MyCameraZoomType > Enum.GetValues(typeof(CameraZoomType)).Cast<CameraZoomType>().Max()) {
            MyCameraZoomType = Enum.GetValues(typeof(CameraZoomType)).Cast<CameraZoomType>().Max();
        }
        ZoomInSwitchCount = 0;
    }

    private void handlePanning() {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x >= Screen.width - PanBuffer) {
            transform.position += new Vector3(panSpeed * (Zoom/MaxZoom), 0, 0);
        } else if (mousePos.x <= 0 + PanBuffer) {
            transform.position += new Vector3(-panSpeed * (Zoom/MaxZoom), 0, 0);
        }
        if (mousePos.y >= Screen.height - PanBuffer) {
            transform.position += new Vector3(0, panSpeed * (Zoom/MaxZoom), 0);
        } else if (mousePos.y <= 0 + PanBuffer) {
            transform.position += new Vector3(0, -panSpeed * (Zoom/MaxZoom), 0);
        }

        //Keyboard controls
        float horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis > 0) {
            transform.position += new Vector3(panSpeed * (Zoom/MaxZoom), 0, 0);
        } else if (horizontalAxis < 0) {
            transform.position += new Vector3(-panSpeed * (Zoom/MaxZoom), 0, 0);
        }
        float verticalAxis = Input.GetAxis("Vertical");
        if (verticalAxis > 0) {
            transform.position += new Vector3(0, panSpeed * (Zoom/MaxZoom), 0);
        } else if (verticalAxis < 0) {
            transform.position += new Vector3(0, -panSpeed * (Zoom/MaxZoom), 0);
        }
    }

    private void handleTownSelection() {
        Town nearestTown = null;
        //Mouse click selection
        if (Input.GetMouseButtonUp(0)) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -transform.position.z;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            nearestTown = GameManager.Instance.getNearestTown(mousePos);
            if (Vector3.Distance(mousePos, nearestTown.transform.position) < nearestTown.Radius) {
                GameManager.Instance.SelectTown(GameManager.Instance.SelectedTown == nearestTown ? null : nearestTown);
            }
        }

        if (MyCameraZoomType == CameraZoomType.Province && ZoomInSwitchCount > ZoomSwitchMax/2) {
            nearestTown = GameManager.Instance.getNearestTown(transform.position);
            GameManager.Instance.SelectTown(nearestTown);
            if (transform.position != nearestTown.transform.position) {
                float speed = Vector3.Distance(transform.position, nearestTown.transform.position)/(10*ZoomSwitchMax/3);
                Vector3 lerpPos = Vector3.Lerp(transform.position, nearestTown.transform.position, speed);
                transform.position = new Vector3(lerpPos.x, lerpPos.y, transform.position.z);
            }
        }

        if (nearestTown != null ) {
            if (GameManager.Instance.SelectedTown == nearestTown) {
                nearestTown.GetComponent<SpriteRenderer>().color = Color.red;
            } else {
                nearestTown.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    void OnDrawGizmos() {
        if (ZoomInSwitchCount > 0) {
            //GUI.Label(new Rect(Screen.width/2, 5, 20, 10), "Switching Zoom - "+Mathf.Floor(100*ZoomSwitchCount/ZoomSwitchMax)+"%");
            UnityEditor.Handles.Label(transform.position + new Vector3(0,0,1), "Switching Zoom - "+Mathf.Floor(100*ZoomInSwitchCount/ZoomSwitchMax)+"%");
        }
    }
}