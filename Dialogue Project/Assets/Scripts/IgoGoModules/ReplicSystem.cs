using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Replic
{
    [Tooltip("Реплика")]
    public string text;
    [Tooltip("Цвет текста")]
    public Color color;
    [Tooltip("Аудиоклип реплики")]
    public AudioClip clip;
}

[RequireComponent(typeof (AudioSource))]
public class ReplicSystem : UsingObject
{
	[Tooltip("Панель UI, в которой показываются субтитры")]
    public GameObject subsPanel;
    [Tooltip("UI TEXT, где будут отображаться субтитры")]
    public Text subs;
    [Tooltip("Играть при запуске сцены")]
    public bool playOnAwake;
    [Space(20)]
    public Replic[] replics;
    [Space(20)]
    public UsingObject[] onEndUsingObjects; 

	private SimpleHandler onCompleteEvent;
    private AudioSource source;
    private int currentNumber;
    private bool isReplic;

	[Header("для загрузки сцен")]
	[Space(20)]
	public ReplicSceneLoader sceneLoader;

    void Start()
    {
        subsPanel.SetActive(false);
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
		
		if(sceneLoader != null)
		{
			onCompleteEvent += sceneLoader.CompleteReplic;
		}
		
        if (source.isPlaying)
        {
            source.Stop();
        }
		if(playOnAwake)
		{
			Use();
		}
    }

    void Update()
    {
        if(isReplic)
        {
            CheckReplic();
        }
    }

    

    private void SetReplic(int number)
    {
        subs.color = replics[number].color;
        subs.text = replics[number].text;
        source.clip = replics[number].clip;
    }
    private void CheckReplic()
    {
        if (!source.isPlaying)
        {
            Next();
        }
    }
    private void Next()
    {
        if (source.isPlaying)
        {
            isReplic = false;
            source.Stop();
        }

        if(currentNumber >= replics.Length)
        {
            subsPanel.SetActive(false);
			if(onCompleteEvent != null)
			{
				onCompleteEvent.Invoke();
			}

            foreach (var item in onEndUsingObjects)
            {
                if(item != null)
                {
                    item.Use();
                }
            }

            Destroy(gameObject);
        }
        else
        {
            SetReplic(currentNumber);
            source.Play();
            currentNumber++;
        }
    }

    public override void Use()
    {
        if(!source.isPlaying)
        {
            source.Play();
        }
        subsPanel.SetActive(true);
        isReplic = true;
    }
}
