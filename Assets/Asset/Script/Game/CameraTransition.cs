using UnityEngine;

public class CameraTransition : MonoBehaviour {
    public Material EffectMaterial;

    // public Texture hideTexture;
    // public Texture showTexture;

    // starting value for the Lerp    
    public float t = 0.0f, speed = 1f;

    public enum Mode {Hide, Show, Idle}

    public Mode mode {
        get {
            return cMode;
        }
        set {

            switch (value) {
                case Mode.Hide:
                    //EffectMaterial.SetTexture("_TransitionTex", hideTexture);
                break;

                case Mode.Show:
                    //EffectMaterial.SetTexture("_TransitionTex", showTexture);
                break;
            }

            cMode = value;
        }
    }

    private Mode cMode = Mode.Idle;

    public void ResetStart() {
        EffectMaterial.SetFloat("_Cutoff", 0);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        Graphics.Blit(src, dst, EffectMaterial);
    }

    void SetTransition(float p_startValue, float p_endValue  ) {
        float cutOffValue = Mathf.Lerp(p_startValue, p_endValue, t);
        t += speed * Time.deltaTime;
        EffectMaterial.SetFloat("_Cutoff", cutOffValue);
        
        if (t > 1.0f){
            cMode = Mode.Idle;
            t = 0;
            EffectMaterial.SetFloat("_Cutoff", p_endValue);
        }
    }

    void Update() {
        if (cMode == Mode.Idle) return;

        switch (cMode) {
            case Mode.Hide:
                SetTransition( 0, 1);

            break;

            case Mode.Show:
                SetTransition( 1, 0 );
            break;

        }
    }


}