﻿using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public enum State {
        Taking,   // removing a piece from the Jenga tower
        Placing,  // placing a piece on the Jenga tower
        GameOver, // everything collapsed
    };

    private static GameState gameState;
    private static CameraControl cam;

    private State currentState;
    //public GameObject finger;
    public GameObject placeLocations;
    public GameObject tower;
    public bool enableCheats;

    public static State CurrentState {
        get {
            return gameState.currentState;
        }
    }

    public static GameObject PlaceLocations {
        get {
            return gameState.placeLocations;
        }
    }

    public static GameObject Tower {
        get {
            return gameState.tower;
        }
    }

    // Handle state change logic
    public static void ChangeState(State newState) {
        switch (newState) {
            case State.Taking:
                Debug.Log("switching to state: taking");
				cam.ScrollToTarget(new Vector3(1f, 4.5f, 1.5f));
                foreach (Transform child in PlaceLocations.transform) {
                    child.gameObject.SetActive(false);
                }
                break;

            case State.Placing:
                Debug.Log("switching to state: placing");

                // Find max height of all children
                float maxHeight = 0f;
                foreach (Transform child in GameState.Tower.transform) {
                    maxHeight = Mathf.Max(maxHeight, child.position.y);
                }
                if (maxHeight >= PlaceLocations.transform.position.y + 1.0f) {
                    Vector3 newPosition = PlaceLocations.transform.position;
                    newPosition.y = maxHeight + 1.0f;
                    PlaceLocations.transform.position = newPosition;
                }

                foreach (Transform child in PlaceLocations.transform) {
                    child.gameObject.SetActive(true);
                }

                cam.ScrollToTarget(new Vector3(1f, 9.5f, 1.5f));
                break;

            case State.GameOver:
                Debug.Log("switching to state: gameover");
                foreach (Transform child in PlaceLocations.transform) {
                    child.gameObject.SetActive(false);
                }
                //Go to center
				cam.ScrollToTarget(new Vector3(0f, 0f, 1.5f));
				AudioSource a = cam.GetComponent<AudioSource>();
				if(!a.isPlaying) a.Play();
                break;
        }

        gameState.currentState = newState;
    }

    // Use this for initialization
    void Start() {
        gameState = this;
        cam = Camera.main.GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update () {
        if (!enableCheats) {
            return;
        }

        if (Input.GetKeyDown("1")) {
            ChangeState(State.Taking);
        } else if (Input.GetKeyDown("2")) {
            ChangeState(State.Placing);
        } else if (Input.GetKeyDown("3")) {
            ChangeState(State.GameOver);
        }
    }

}
