using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorCombate : MonoBehaviour
{
	public int dano;

	public Animator maoEsquerdaAnimator;
	public Animator maoDireitaAnimator;

	public AudioClip socarHitSom;
	private AudioSource socarHitAudioSource;

	public AudioClip socarMissSom;
	private AudioSource socarMissAudioSource;

	private Monstro monstroSelecionado;

	private int socoAtual = 0;

	internal static JogadorCombate instancia;

	private void Awake()
	{
		instancia = this;

		socarHitAudioSource = gameObject.AddComponent<AudioSource>();
		socarHitAudioSource.playOnAwake = false;
		socarHitAudioSource.clip = socarHitSom;

		socarMissAudioSource = gameObject.AddComponent<AudioSource>();
		socarMissAudioSource.playOnAwake = false;
		socarMissAudioSource.clip = socarMissSom;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Socar();
		}
	}

	private void Socar()
	{
		if (monstroSelecionado)
		{
			socarHitAudioSource.Play();

			bool morreu = monstroSelecionado.ReceberAtaque();

			if (morreu)
				DesselecionarMonstro(monstroSelecionado);
		}
		else
		{
			socarMissAudioSource.Play();
		}

		if (socoAtual == 0)
			maoEsquerdaAnimator.SetTrigger("Socar");
		else
			maoDireitaAnimator.SetTrigger("Socar");

		socoAtual = socoAtual == 0 ? 1 : 0;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Monstro"))
		{
			SelecionarMonstro(collider.GetComponent<Monstro>());
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("Monstro"))
		{
			DesselecionarMonstro(collider.GetComponent<Monstro>());
		}
	}

	private void SelecionarMonstro(Monstro monstro)
	{
		monstroSelecionado = monstro;
	}

	private void DesselecionarMonstro(Monstro monstro)
	{
		if (monstro == monstroSelecionado)
			monstroSelecionado = null;
	}
}
