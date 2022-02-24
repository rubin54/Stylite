using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeObserver : MonoBehaviour
{
    [SerializeField]
    private MissionController missionController;

    [SerializeField]
    private GameObject portraitPrefab;

    [SerializeField]
    private GameObject socketPrefab;

    [SerializeField]
    private Transform socket;

    [SerializeField]
    private TextMeshProUGUI roundText;


    [SerializeField]
    List<TextMeshProUGUI> newRoundTexts;

    [SerializeField]
    List<Image> newRoundImages;

    public AK.Wwise.Event NewRoundSFX;


    [SerializeField]
    private Color enemyColour = new Color(125f / 255f, 36f / 255f, 52f / 255f);

    [SerializeField]
    private Color playerColour = new Color(65f / 255f, 84f / 255f, 44f / 255f);

    private List<GameObject> portraits = new List<GameObject>();
    private List<GameObject> sockets = new List<GameObject>();

    private float fadeTime = 0;

    private int currentRound = 0;

    private int currentTurn = 0;

    public bool lastTypeWasPlayer = false;

    void Awake()
    {
        missionController.StartedNextTurn += OnStartedNextTurn;
        missionController.StartedNextRound += OnStartedNextRound;
    }

    private void Update()
    {
        if(fadeTime > 2)
        {
            foreach (var text in newRoundTexts)
            {
                Color Color = text.color;
                if (Color.a > 0)
                {
                    Color.a -= 0.3f * Time.deltaTime;
                }
                text.color = Color;
            }

            foreach (var image in newRoundImages)
            {
                Color Color = image.color;
                if (Color.a > 0)
                {
                    Color.a -= 0.3f * Time.deltaTime;
                }
                image.color = Color;
            }
        }

        if(fadeTime >= 0)
        {
            fadeTime += Time.deltaTime;
        }
    }

    public void OnStartedNextTurn()
    {
        currentTurn++;

        if(currentTurn < sockets.Count)
        {
            if (currentTurn != 0)
            {
                Image lastBorder = sockets[currentTurn - 1].GetComponent<Image>();
                NewRoundSFX.Post(gameObject);

                if (lastTypeWasPlayer)
                {
                    lastBorder.color = playerColour;
                }
                else
                {
                    lastBorder.color = enemyColour;
                }
            }

            lastTypeWasPlayer = missionController.SortedInitiatives[0].Unit.IsPlayer();

            Image border = sockets[currentTurn].GetComponent<Image>();

            border.color = Color.yellow;
        }
    }

    public void OnStartedNextRound()
    {
        fadeTime = 0;
        currentTurn = -1;

        currentRound++;
        roundText.text = "Round: " + currentRound;

        foreach (var text in newRoundTexts)
        {
            Color Color = text.color;
            Color.a = 1;
            text.color = Color;
        }

        foreach (var image in newRoundImages)
        {
            Color Color = image.color;
            Color.a = 1;
            image.color = Color;
        }


        foreach (var socket in sockets.ToArray())
        {
            Destroy(socket);
            sockets.Remove(socket);
        }

        foreach (var portrait in portraits.ToArray())
        {
            Destroy(portrait);
            portraits.Remove(portrait);
        }

        foreach (var entry in missionController.SortedInitiatives)
        {
            GameObject createdSocket = Instantiate(socketPrefab, socket);
            Image border = createdSocket.GetComponent<Image>();
            if (entry.Unit.IsEnemy())
            {
                border.color = enemyColour;
            }
            if (entry.Unit.IsPlayer())
            {
                border.color = playerColour;
            }

            sockets.Add(createdSocket);

            GameObject createdObject = Instantiate(portraitPrefab, createdSocket.transform);
            createdObject.GetComponent<RawImage>().texture = entry.Unit.Unit.UnitTemplate.GetComponent<RawImage>().texture;
            InitiativeUnitIcon unitIcon = createdObject.GetComponent<InitiativeUnitIcon>();
            if (unitIcon) unitIcon.Setup(entry);

            portraits.Add(createdObject);
        }
    }
}
