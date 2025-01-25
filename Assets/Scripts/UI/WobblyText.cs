using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class WobblyText : MonoBehaviour
{
    public TMP_Text textComponent;
    private float localtime = 0f;

    private float offset = 0;

    void Start(){
        offset = Random.Range(0, Mathf.PI);
    }   

    void Update(){
        localtime += Time.deltaTime;
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++ ){
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible){
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++){                    // 4 vertices in a character, do not change
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] += new Vector3(0, Mathf.Sin(localtime * 2 + orig.x*0.05f + offset) * 3f, 0);
            }

        }
    
        for (int i = 0; i < textInfo.meshInfo.Length; i++){
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }


    }

}