// ---------------------------------------------------------------
// Code copyright by Hedgehog Team, Creepy Cat, Barking Dog (2017)
// ---------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselImage : MonoBehaviour {
	public float fadeDuration = 2.0f;
	public float framesPerSecond = 0.1f;
	public Texture2D[] frames;

	private int indice = 0;
	private int pingpong=1;

	// Update is called once per frame
	void FixedUpdate () {
		float lerp = Mathf.PingPong (Time.time, fadeDuration) / fadeDuration;
		indice = (int) (Time.time * framesPerSecond);
		indice = indice % frames.Length;

		if (lerp>0.999f && pingpong==1){
			GetComponent<Renderer>().material.SetTexture( "_TexMat1",frames[indice]);
			pingpong=2;
		}

		if (lerp<0.001f && pingpong==2){
			GetComponent<Renderer>().material.SetTexture( "_TexMat2",frames[indice]);
			pingpong=1;
		}

		GetComponent<Renderer>().material.SetFloat( "_Blend", lerp );
	}
}
