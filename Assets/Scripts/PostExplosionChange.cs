using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostExplosionChange : MonoBehaviour
{
    private PostProcessVolume ThisPost;
    private Bloom pBloom;
    private ChromaticAberration pChrom;

    private float BloomChange = 70;
    private float ChromChange = .67f;

    private void Awake()
    {
        ThisPost = this.GetComponent<PostProcessVolume>();
        pBloom = ThisPost.profile.GetSetting<Bloom>();
        pChrom = ThisPost.profile.GetSetting<ChromaticAberration>();
    }

    public IEnumerator PostChange(float duration)
    {
        float OGBloom = pBloom.intensity.value;
        float OGChrom = pChrom.intensity.value;

        float elapsed = 0.0f;
        float Bstep = 0;
        float Cstep = 0;

        while (elapsed < duration)
        {
            Cstep = Mathf.Lerp(ChromChange, OGChrom, elapsed);
            Bstep = Mathf.Lerp(BloomChange, OGBloom, elapsed);

            pBloom.intensity.value = Bstep;
            pChrom.intensity.value = Cstep;

            elapsed += Time.deltaTime;
            yield return null;
        }

        pChrom.intensity.value = OGChrom;
        pBloom.intensity.value = OGBloom;
    }
}
