using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject redPiecePrefab;
    public GameObject bluePiecePrefab;
    [HideInInspector] public Transform[] columns;
    [HideInInspector] public Button[] columnButtons;
    private TextMeshProUGUI winText;

    private int[,] board;
    private int currentPlayer = 1;

    void Start()
    {
        board = new int[6, 7];

        InitializeText();
        InitializeButtons();
        InitializeColumns();

        winText.gameObject.SetActive(false);
        SetupColumnButtons();
    }

    void InitializeText()
    {
        // Cria um novo objeto de texto na UI
        GameObject textGameObject = new GameObject("WinText");
        textGameObject.transform.SetParent(GameObject.Find("Canvas").transform);

        // Configura as propriedades do RectTransform
        RectTransform rectTransform = textGameObject.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0); // Centralizado
        rectTransform.sizeDelta = new Vector2(400, 100); // Tamanho

        // Adiciona o componente TextMeshProUGUI ao GameObject criado
        winText = textGameObject.AddComponent<TextMeshProUGUI>();
        winText.fontSize = 36;
        winText.alignment = TextAlignmentOptions.Center;
        winText.text = ""; // Texto inicial vazio
        winText.color = Color.white; // Cor do texto

        Debug.Log("Texto de vitória criado com sucesso.");
    }

    void InitializeColumns()
    {
        columns = new Transform[7];

        columns[0] = transform.Find("COL");
        for (int i = 1; i < columns.Length; i++)
        {
            columns[i] = transform.Find($"COL ({i})");
            if (columns[i] == null)
            {
                Debug.LogError($"COL ({i}) não foi encontrado!");
            }
        }
    }

    void InitializeButtons()
    {
        columnButtons = new Button[7];

        for (int i = 0; i < columnButtons.Length; i++)
        {
            columnButtons[i] = GameObject.Find($"btn{i + 1}").GetComponent<Button>();
            if (columnButtons[i] == null)
            {
                Debug.LogError($"btn{i + 1} não foi encontrado!");
            }
            else
            {
                Debug.Log($"Botão btn{i + 1} inicializado com sucesso.");
            }
        }
    }


    void SetupColumnButtons()
    {
        for (int i = 0; i < columnButtons.Length; i++)
        {
            int column = i;
            columnButtons[i].onClick.AddListener(() => PlacePiece(column));
            Debug.Log($"Botão da coluna {column + 1} configurado.");
        }
    }

    void PlacePiece(int column)
    {
        Debug.Log($"Tentando colocar peça na coluna {column + 1}.");

        for (int row = 0; row < 6; row++)
        {
            if (board[row, column] == 0)
            {
                board[row, column] = currentPlayer;
                Vector3 position = new Vector3(columns[column].position.x, columns[column].position.y + row * 0.2f, columns[column].position.z);
                GameObject piece = Instantiate(currentPlayer == 1 ? redPiecePrefab : bluePiecePrefab, position, Quaternion.identity);
                piece.transform.parent = transform;

                Debug.Log($"Peça {(currentPlayer == 1 ? "vermelha" : "azul")} colocada na coluna {column + 1}, linha {row + 1}.");

                if (CheckForWin(row, column))
                {
                    string color = currentPlayer == 1 ? "Vermelho" : "Azul";
                    winText.text = $"{color} venceu!";
                    winText.gameObject.SetActive(true);
                    Debug.Log($"{color} venceu!");
                }

                currentPlayer = currentPlayer == 1 ? 2 : 1;
                break;
            }
        }
    }

    bool CheckForWin(int row, int col)
    {
        int player = board[row, col];

        if (CheckDirection(row, col, 0, 1, player) + CheckDirection(row, col, 0, -1, player) >= 3)
            return true;

        if (CheckDirection(row, col, 1, 0, player) + CheckDirection(row, col, -1, 0, player) >= 3)
            return true;

        if (CheckDirection(row, col, 1, 1, player) + CheckDirection(row, col, -1, -1, player) >= 3)
            return true;

        if (CheckDirection(row, col, 1, -1, player) + CheckDirection(row, col, -1, 1, player) >= 3)
            return true;

        return false;
    }

    int CheckDirection(int row, int col, int rowDir, int colDir, int player)
    {
        int count = 0;
        int r = row + rowDir;
        int c = col + colDir;

        while (r >= 0 && r < 6 && c >= 0 && c < 7 && board[r, c] == player)
        {
            count++;
            r += rowDir;
            c += colDir;
        }

        return count;
    }
}
