using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorGameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;
    public int gridSize = 3;

    public Text scoreText;
    public Button replayButton;

    private int score = 0;
    private int differentIndex;
    private Color baseColor;
    private Color differentColor;

    void Start()
    {
        replayButton.onClick.AddListener(ResetGame);
        UpdateScore();
        CreateGrid();
    }

    void CreateGrid()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        List<GameObject> tiles = new List<GameObject>();
        int totalTiles = gridSize * gridSize;
        differentIndex = Random.Range(0, totalTiles);

        baseColor = GetRandomColor();
        differentColor = GetSlightlyDifferentColor(baseColor);

        UpdateCellSize();

        for (int i = 0; i < totalTiles; i++)
        {
            GameObject tile = Instantiate(tilePrefab, gridParent);
            tile.GetComponent<Image>().color = (i == differentIndex) ? differentColor : baseColor;

            int index = i;
            tile.GetComponent<Button>().onClick.AddListener(() => OnTileClicked(index));
            tiles.Add(tile);
        }
    }

    void OnTileClicked(int index)
    {
        if (index == differentIndex)
        {
            score++;
            gridSize++;
            UpdateScore();
            CreateGrid();
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }

    void ResetGame()
    {
        score = 0;
        gridSize = 3;
        UpdateScore();
        CreateGrid();
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score;
    }

    // void UpdateCellSize()
    // {
    //     GridLayoutGroup gridLayout = gridParent.GetComponent<GridLayoutGroup>();
    //     RectTransform gridRect = (RectTransform)gridParent;

    //     float spacing = 10f;
    //     float width = gridRect.rect.width;
    //     float height = gridRect.rect.height;

    //     float cellWidth = (width - (gridSize - 1) * spacing) / gridSize;
    //     float cellHeight = (height - (gridSize - 1) * spacing) / gridSize;

    //     gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    //     gridLayout.spacing = new Vector2(spacing, spacing);
    //     gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    //     gridLayout.constraintCount = gridSize;
    // }

    void UpdateCellSize()
    {
        GridLayoutGroup grid = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform rect = gridParent.GetComponent<RectTransform>();

        float spacing = 10f;
        float panelWidth = rect.rect.width;
        float panelHeight = rect.rect.height;

        float cellWidth = (panelWidth - spacing * (gridSize - 1)) / gridSize;
        float cellHeight = (panelHeight - spacing * (gridSize - 1)) / gridSize;
        float cellSize = Mathf.Min(cellWidth, cellHeight); // để ô vuông

        grid.cellSize = new Vector2(cellSize, cellSize);
        grid.spacing = new Vector2(spacing, spacing);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = gridSize;
    }

    Color GetRandomColor()
    {
        float r = Random.Range(0.2f, 0.9f);
        float g = Random.Range(0.2f, 0.9f);
        float b = Random.Range(0.2f, 0.9f);
        return new Color(r, g, b);
    }

    Color GetSlightlyDifferentColor(Color baseColor)
    {
        float diff = 0.08f;
        return new Color(
            Mathf.Clamp01(baseColor.r + Random.Range(-diff, diff)),
            Mathf.Clamp01(baseColor.g + Random.Range(-diff, diff)),
            Mathf.Clamp01(baseColor.b + Random.Range(-diff, diff))
        );
    }
}
