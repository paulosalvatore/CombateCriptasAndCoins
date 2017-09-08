using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Knoback
{
	[Header("Eixo X")]
	public float lateral;

	[Header("Eixo Y")]
	public float pulo;

	[Header("Eixo Z")]
	public float recuo;
}

public class Monstro : MonoBehaviour
{
	private JogadorCombate jogador;

	public bool mover = true;

	[Range(0.5f, 10f)]
	public float velocidadeMovimento;
	public float distanciaDetectar;
	public float distanciaSeguir;
	public float distanciaAtacar;

	public float delayAtacar;

	public int hpMax;
	private int hpAtual;
	public Transform barraHP;
	private float scaleMaxBarra;

	public AudioClip atacarSom;
	private AudioSource atacarAudioSource;

	public Knoback knoback;

	private bool seguindoJogador = false;
	private float distanciaJogador;

	private bool ataqueLiberado = true;

	private bool jogadorDetectado = false;

	private Animator animator;
	private new Rigidbody rigidbody;
	private SpriteRenderer spriteRenderer;
	private Color corPadrao;

	private void Awake()
	{
		hpAtual = hpMax;

		animator = GetComponent<Animator>();

		rigidbody = GetComponent<Rigidbody>();
		rigidbody.isKinematic = !mover;

		spriteRenderer = GetComponent<SpriteRenderer>();
		corPadrao = spriteRenderer.color;

		atacarAudioSource = gameObject.AddComponent<AudioSource>();
		atacarAudioSource.playOnAwake = false;
		atacarAudioSource.clip = atacarSom;
	}

	private void Start()
	{
		jogador = JogadorCombate.instancia;

		scaleMaxBarra = barraHP.localScale.x;
	}

	private void Update()
	{
		ChecarMovimentacao();
	}

	private void ChecarMovimentacao()
	{
		if (!mover)
			return;

		distanciaJogador = Vector3.Distance(transform.position, jogador.transform.position);

		if (seguindoJogador)
		{
			if (distanciaJogador > distanciaSeguir)
			{
				PararSeguirJogador();
			}
			else if (distanciaJogador > distanciaAtacar)
			{
				Andar();
			}
			else if (distanciaJogador <= distanciaAtacar)
			{
				if (ataqueLiberado)
				{
					Atacar();
				}
				else
				{
					Parar();
				}
			}
		}
		else if (!jogadorDetectado && distanciaJogador <= distanciaDetectar)
		{
			Detectar();

			Invoke("IniciarSeguirJogador", 0.2f);
		}
	}

	private void Detectar()
	{
		jogadorDetectado = true;

		animator.SetTrigger("Detectar");
	}

	private void IniciarSeguirJogador()
	{
		seguindoJogador = true;
	}

	private void PararSeguirJogador()
	{
		Parar();

		jogadorDetectado = false;

		seguindoJogador = false;
	}

	private void Andar()
	{
		transform.position += transform.forward * -1f * velocidadeMovimento * Time.smoothDeltaTime;

		animator.SetBool("Andar", true);
	}

	private void Parar()
	{
		animator.SetBool("Andar", false);
	}

	private void Atacar()
	{
		ataqueLiberado = false;

		animator.SetTrigger("Atacar");

		atacarAudioSource.Play();

		Invoke("LiberarAtaque", delayAtacar);
	}

	private void LiberarAtaque()
	{
		ataqueLiberado = true;
	}

	public bool ReceberAtaque()
	{
		hpAtual -= jogador.dano;

		AtualizarBarraHP();

		rigidbody.AddForce(
			new Vector3(
				knoback.lateral,
				knoback.pulo,
				knoback.recuo
			)
		);

		if (hpAtual <= 0)
		{
			Morrer();

			return true;
		}
		else
			animator.SetTrigger("Hit");

		return false;
	}

	private void Morrer()
	{
		animator.SetTrigger("Morrer");

		Invoke("OcultarGameObject", 1f);
	}

	private void OcultarGameObject()
	{
		gameObject.SetActive(false);
	}

	private void AtualizarBarraHP()
	{
		barraHP.localScale = new Vector3(
			Mathf.Max(0, scaleMaxBarra * hpAtual / hpMax),
			barraHP.localScale.y,
			barraHP.localScale.z
		);
	}
}
