using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIManager;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]private downloadGLTFManager gltfManager;
    public List<GameObject> unitLists;
    public async void Start()
    {
        if (gltfManager.getURL() != null)
        {
            // First step: load glTF
            var gltf = new GLTFast.GltfImport();
            var success = await gltf.Load(gltfManager.getURL());

            if (success)
            {
                // Here you can customize the post-loading behavior
                for (int i = 0; i < 8; i++)
                {
                    var newObject = new GameObject();
                    newObject.transform.position = new Vector3((i / 4) * 3, 0, (int)(i % 4) * 3); //Place vector random position for new downloaded object
                    var posX = Random.Range(0f, 1.0f);
                    var posZ = Random.Range(0f, 1.0f);
                    newObject.transform.forward = new Vector3(posX, 0,posZ); //placing the object
                    await gltf.InstantiateMainSceneAsync(newObject.transform);
                    unitLists.Add(newObject);
                    var newObjAnimation = newObject.GetComponent<Animation>(); //adding animation
                    List<AnimationState> anims = new List<AnimationState>();
                    foreach(AnimationState anim in newObjAnimation)
                    {
                        anims.Add(anim);
                    }

                    newObjAnimation.Play(anims.ElementAt(i).name);
                }
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
        }
        else Debug.Log("initial play: please click start");
    }
}
