using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ZoomableCamera : MonoBehaviour {
    [SerializeField]
	private float minZoom = 30;
    [SerializeField]
	private float maxZoom = 90;
    [SerializeField]
	private float zoom = 30;
    [SerializeField]
    private float scrollSpeed = 50;
    [SerializeField]
    private float zoomSwitchCount = 0;
    [SerializeField]
    private int zoomSwitchMax = 20;
    [SerializeField]
    private float zoomSwitchResetSpeed = 0.01f;
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

    public float ZoomSwitchCount {
        get { return zoomSwitchCount; }
        private set {zoomSwitchCount = value; }
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
	}

    private void handleZooming() {
        float scrollInput = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
        if (scrollInput != 0) {
            Zoom = Mathf.Clamp(Zoom + ScrollSpeed * scrollInput, MinZoom, MaxZoom);
            Camera.main.fieldOfView = Zoom;

            if (scrollInput < 0 && Zoom == MinZoom) {
                ZoomSwitchCount++;
                if (ZoomSwitchCount >= ZoomSwitchMax) {
                    switchZoom(true);
                }
            } else if (scrollInput > 0 && Zoom == MaxZoom) {
                ZoomSwitchCount++;
                if (ZoomSwitchCount >= ZoomSwitchMax) {
                    switchZoom(false);
                }
            }
        } else {
            zoomSwitchCount = Mathf.Max(ZoomSwitchCount - ZoomSwitchResetSpeed, 0);
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
        ZoomSwitchCount = 0;
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
    }
}