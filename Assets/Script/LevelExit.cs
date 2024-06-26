using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    HashSet<GameObject> playersInExit = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !playersInExit.Contains(other.gameObject)) {
            playersInExit.Add(other.gameObject);
            if (playersInExit.Count >= 2) {
                StartCoroutine(LoadNextLevel());
            }
        }
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
